using System.Collections.Generic;
using IdleGuildDemo.Core;
using IdleGuildDemo.Data;
using IdleGuildDemo.Runtime;
using IdleGuildDemo.Systems;
using UnityEngine;
using UnityEngine.UI;

namespace IdleGuildDemo.UI
{
    public class QuestTrackerView : MonoBehaviour
    {
        [SerializeField] private Text questTitleText;
        [SerializeField] private Text questObjectiveText;
        [SerializeField] private Image progressFill;
        [SerializeField] private Text progressText;
        [SerializeField] private Text claimStateText;

        public void RefreshFromQuestSystem(
            QuestSystem questSystem,
            IReadOnlyList<QuestDefinition> questDefinitions,
            PlayerProfile profile)
        {
            if (questSystem == null || questDefinitions == null || profile == null)
            {
                Clear();
                return;
            }

            profile.Normalize();
            QuestDefinition quest = questSystem.GetCurrentQuest(questDefinitions);
            if (quest == null)
            {
                SetComplete();
                return;
            }

            bool canClaimReward = questSystem.CanClaimReward(questDefinitions);
            SetQuest(quest, profile.questProgress, canClaimReward);
        }

        public void SetQuest(QuestDefinition quest, QuestProgressState progressState, bool canClaimReward)
        {
            if (quest == null)
            {
                Clear();
                return;
            }

            int required = GetRequiredAmount(quest);
            int current = progressState != null ? progressState.currentAmount : 0;
            SetQuest(quest.DisplayName, GetObjectiveLabel(quest), current, required);
            SetClaimReady(canClaimReward);
        }

        public void SetQuest(string title, string objective, int current, int target)
        {
            int safeTarget = target < 1 ? 1 : target;
            int safeCurrent = Mathf.Clamp(current, 0, safeTarget);

            SetText(questTitleText, string.IsNullOrEmpty(title) ? "Quest" : title);
            SetText(questObjectiveText, objective ?? string.Empty);
            SetText(progressText, $"{safeCurrent}/{safeTarget}");
            SetProgress(safeTarget > 0 ? (float)safeCurrent / safeTarget : 0f);
        }

        public void SetClaimReady(bool canClaimReward)
        {
            SetText(claimStateText, canClaimReward ? "Ready to claim" : string.Empty);
        }

        public void SetComplete()
        {
            SetText(questTitleText, "Quest Chain Complete");
            SetText(questObjectiveText, "All tutorial quests are complete.");
            SetText(progressText, string.Empty);
            SetText(claimStateText, string.Empty);
            SetProgress(1f);
        }

        public void Clear()
        {
            SetText(questTitleText, "No active quest");
            SetText(questObjectiveText, string.Empty);
            SetText(progressText, string.Empty);
            SetText(claimStateText, string.Empty);
            SetProgress(0f);
        }

        public static string GetObjectiveLabel(QuestDefinition quest)
        {
            if (quest == null)
            {
                return string.Empty;
            }

            switch (quest.ObjectiveType)
            {
                case QuestObjectiveType.KillEnemy:
                    return $"Defeat {GetRequiredAmount(quest)} {FormatEnemyName(quest.TargetId)}";
                case QuestObjectiveType.CollectItem:
                    return $"Collect {GetRequiredAmount(quest)} {FormatItemName(quest.TargetId)}";
                case QuestObjectiveType.CraftItem:
                    return $"Craft {GetRequiredAmount(quest)} {FormatItemName(quest.TargetId)}";
                case QuestObjectiveType.ReachLevel:
                    return $"Reach level {GetRequiredAmount(quest)}";
                case QuestObjectiveType.ChooseClass:
                    return "Choose a class";
                case QuestObjectiveType.UnlockCharacter:
                    return "Unlock Character 2";
                default:
                    return quest.Description ?? string.Empty;
            }
        }

        public static string FormatItemName(string itemId)
        {
            switch (itemId)
            {
                case GameConstants.ItemCopperOreId:
                    return "Copper Ore";
                case GameConstants.ItemCopperBarId:
                    return "Copper Bar";
                case GameConstants.ItemSlimeGooId:
                    return "Slime Goo";
                case GameConstants.ItemCopperSwordId:
                    return "Copper Sword";
                case GameConstants.ItemCopperPickaxeId:
                    return "Copper Pickaxe";
                default:
                    return string.IsNullOrEmpty(itemId) ? "Item" : itemId;
            }
        }

        private static string FormatEnemyName(string enemyId)
        {
            switch (enemyId)
            {
                case GameConstants.EnemySlimeId:
                    return "Slimes";
                default:
                    return string.IsNullOrEmpty(enemyId) ? "Enemies" : enemyId;
            }
        }

        private static int GetRequiredAmount(QuestDefinition quest)
        {
            return quest != null && quest.RequiredAmount > 0 ? quest.RequiredAmount : 1;
        }

        private void SetProgress(float normalized)
        {
            if (progressFill != null)
            {
                progressFill.fillAmount = Mathf.Clamp01(normalized);
            }
        }

        private static void SetText(Text text, string value)
        {
            if (text != null)
            {
                text.text = value;
            }
        }
    }
}
