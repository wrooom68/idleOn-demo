using System.Collections.Generic;
using IdleGuildDemo.Data;
using IdleGuildDemo.Runtime;

namespace IdleGuildDemo.Systems
{
    /// <summary>
    /// Owns future linear quest-chain progress and reward claim rules.
    /// </summary>
    public sealed class QuestSystem
    {
        private readonly PlayerProfile profile;
        private readonly InventorySystem inventorySystem;
        private readonly ProgressionSystem progressionSystem;
        private readonly CharacterRosterSystem rosterSystem;

        public QuestSystem(
            PlayerProfile profile,
            InventorySystem inventorySystem,
            ProgressionSystem progressionSystem,
            CharacterRosterSystem rosterSystem)
        {
            this.profile = profile ?? PlayerProfile.CreateDefault();
            this.profile.Normalize();
            this.inventorySystem = inventorySystem;
            this.progressionSystem = progressionSystem ?? new ProgressionSystem();
            this.rosterSystem = rosterSystem ?? new CharacterRosterSystem(this.profile);
        }

        public QuestDefinition GetCurrentQuest(IReadOnlyList<QuestDefinition> questDefinitions)
        {
            profile.Normalize();
            return FindQuest(questDefinitions, profile.questProgress.currentQuestId);
        }

        public QuestUpdateResult ReportKill(string enemyId, IReadOnlyList<QuestDefinition> questDefinitions)
        {
            return ReportProgress(QuestObjectiveType.KillEnemy, enemyId, 1, questDefinitions);
        }

        public QuestUpdateResult ReportItemCollected(string itemId, int amount, IReadOnlyList<QuestDefinition> questDefinitions)
        {
            return ReportProgress(QuestObjectiveType.CollectItem, itemId, amount, questDefinitions);
        }

        public QuestUpdateResult ReportItemCrafted(string itemId, int amount, IReadOnlyList<QuestDefinition> questDefinitions)
        {
            return ReportProgress(QuestObjectiveType.CraftItem, itemId, amount, questDefinitions);
        }

        public QuestUpdateResult ReportLevelReached(int level, IReadOnlyList<QuestDefinition> questDefinitions)
        {
            QuestDefinition quest = GetCurrentQuest(questDefinitions);
            if (quest == null || quest.ObjectiveType != QuestObjectiveType.ReachLevel)
            {
                return CreateUpdateResult(quest);
            }

            return SetProgress(quest, level);
        }

        public QuestUpdateResult ReportClassChosen(string classId, IReadOnlyList<QuestDefinition> questDefinitions)
        {
            QuestDefinition quest = GetCurrentQuest(questDefinitions);
            if (quest == null || quest.ObjectiveType != QuestObjectiveType.ChooseClass)
            {
                return CreateUpdateResult(quest);
            }

            if (!string.IsNullOrEmpty(quest.TargetId) && quest.TargetId != classId)
            {
                return CreateUpdateResult(quest);
            }

            return ReportProgress(QuestObjectiveType.ChooseClass, quest.TargetId, 1, questDefinitions);
        }

        public bool CanClaimCurrentQuest(IReadOnlyList<QuestDefinition> questDefinitions)
        {
            QuestDefinition quest = GetCurrentQuest(questDefinitions);
            if (quest == null)
            {
                return false;
            }

            MarkAutoCompletableQuest(quest);
            return profile.questProgress.isComplete;
        }

        public QuestClaimResult ClaimCurrentQuest(IReadOnlyList<QuestDefinition> questDefinitions)
        {
            QuestClaimResult result = new QuestClaimResult();
            QuestDefinition quest = GetCurrentQuest(questDefinitions);
            if (quest == null)
            {
                result.failureReason = "Current quest is missing.";
                return result;
            }

            result.questId = quest.Id ?? string.Empty;
            result.nextQuestId = quest.NextQuestId ?? string.Empty;
            result.xpReward = quest.RewardXp;
            result.coinsReward = quest.RewardCoins;
            result.rewardItemId = quest.RewardItemId ?? string.Empty;
            result.rewardItemQuantity = quest.RewardItemQuantity;

            if (!CanClaimCurrentQuest(questDefinitions))
            {
                result.failureReason = "Current quest is not complete.";
                return result;
            }

            CharacterState activeCharacter = rosterSystem.GetActiveCharacter();
            progressionSystem.AddXp(activeCharacter, quest.RewardXp);

            if (quest.RewardCoins > 0)
            {
                profile.coins += quest.RewardCoins;
            }

            if (!string.IsNullOrEmpty(quest.RewardItemId) && quest.RewardItemQuantity > 0)
            {
                inventorySystem?.AddItem(quest.RewardItemId, quest.RewardItemQuantity);
            }

            if (quest.UnlocksSecondCharacter)
            {
                rosterSystem.UnlockSecondCharacter();
                result.unlockedSecondCharacter = true;
            }

            AddCompletedQuest(quest.Id);
            AdvanceToNextQuest(quest);

            result.success = true;
            return result;
        }

        private QuestUpdateResult ReportProgress(
            QuestObjectiveType objectiveType,
            string targetId,
            int amount,
            IReadOnlyList<QuestDefinition> questDefinitions)
        {
            QuestDefinition quest = GetCurrentQuest(questDefinitions);
            if (quest == null || amount <= 0)
            {
                return CreateUpdateResult(quest);
            }

            if (quest.ObjectiveType != objectiveType)
            {
                return CreateUpdateResult(quest);
            }

            if (!string.IsNullOrEmpty(quest.TargetId) && quest.TargetId != targetId)
            {
                return CreateUpdateResult(quest);
            }

            return AddProgress(quest, amount);
        }

        private QuestUpdateResult AddProgress(QuestDefinition quest, int amount)
        {
            profile.Normalize();
            int requiredAmount = GetRequiredAmount(quest);
            int nextAmount = profile.questProgress.currentAmount + amount;
            if (nextAmount > requiredAmount)
            {
                nextAmount = requiredAmount;
            }

            return SetProgress(quest, nextAmount, true);
        }

        private QuestUpdateResult SetProgress(QuestDefinition quest, int amount, bool forceUpdated = false)
        {
            profile.Normalize();
            int requiredAmount = GetRequiredAmount(quest);
            int safeAmount = amount < 0 ? 0 : amount;
            if (safeAmount > requiredAmount)
            {
                safeAmount = requiredAmount;
            }

            bool changed = profile.questProgress.currentAmount != safeAmount || forceUpdated;
            profile.questProgress.currentQuestId = quest.Id ?? string.Empty;
            profile.questProgress.questId = profile.questProgress.currentQuestId;
            profile.questProgress.requiredAmount = requiredAmount;
            profile.questProgress.currentAmount = safeAmount;
            profile.questProgress.isComplete = safeAmount >= requiredAmount;

            QuestUpdateResult result = CreateUpdateResult(quest);
            result.updated = changed;
            result.completed = profile.questProgress.isComplete;
            result.message = result.completed ? "Quest complete." : string.Empty;
            return result;
        }

        private QuestUpdateResult CreateUpdateResult(QuestDefinition quest)
        {
            profile.Normalize();
            return new QuestUpdateResult
            {
                questId = quest != null ? quest.Id ?? string.Empty : profile.questProgress.currentQuestId,
                currentAmount = profile.questProgress.currentAmount,
                requiredAmount = quest != null ? GetRequiredAmount(quest) : 0,
                completed = profile.questProgress.isComplete
            };
        }

        private void MarkAutoCompletableQuest(QuestDefinition quest)
        {
            if (quest.ObjectiveType == QuestObjectiveType.UnlockCharacter)
            {
                SetProgress(quest, GetRequiredAmount(quest));
            }
        }

        private void AdvanceToNextQuest(QuestDefinition quest)
        {
            profile.questProgress.currentQuestId = quest.NextQuestId ?? string.Empty;
            profile.questProgress.questId = profile.questProgress.currentQuestId;
            profile.questProgress.currentAmount = 0;
            profile.questProgress.requiredAmount = 0;
            profile.questProgress.isComplete = false;
            profile.questProgress.rewardClaimed = false;
            profile.currentQuestId = profile.questProgress.currentQuestId;
        }

        private void AddCompletedQuest(string questId)
        {
            if (string.IsNullOrEmpty(questId))
            {
                return;
            }

            profile.questProgress.Normalize();
            if (!profile.questProgress.completedQuestIds.Contains(questId))
            {
                profile.questProgress.completedQuestIds.Add(questId);
            }
        }

        private static QuestDefinition FindQuest(IReadOnlyList<QuestDefinition> questDefinitions, string questId)
        {
            if (questDefinitions == null || string.IsNullOrEmpty(questId))
            {
                return null;
            }

            foreach (QuestDefinition quest in questDefinitions)
            {
                if (quest != null && quest.Id == questId)
                {
                    return quest;
                }
            }

            return null;
        }

        private static int GetRequiredAmount(QuestDefinition quest)
        {
            return quest != null && quest.RequiredAmount > 0 ? quest.RequiredAmount : 1;
        }
    }
}
