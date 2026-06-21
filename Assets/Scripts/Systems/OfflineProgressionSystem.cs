using System;
using System.Globalization;
using IdleGuildDemo.Core;
using IdleGuildDemo.Runtime;

namespace IdleGuildDemo.Systems
{
    /// <summary>
    /// Owns future offline/AFK reward calculations from saved timestamps and character tasks.
    /// </summary>
    public sealed class OfflineProgressionSystem
    {
        private readonly InventorySystem inventorySystem;
        private readonly ProgressionSystem progressionSystem;
        private readonly StatsSystem statsSystem;

        public OfflineProgressionSystem(
            InventorySystem inventorySystem,
            ProgressionSystem progressionSystem,
            StatsSystem statsSystem)
        {
            this.inventorySystem = inventorySystem;
            this.progressionSystem = progressionSystem ?? new ProgressionSystem();
            this.statsSystem = statsSystem ?? new StatsSystem();
        }

        public AfkRewardSummary CalculateAndApplyRewards(PlayerProfile profile, DateTime nowUtc)
        {
            return CalculateAndApplyRewards(profile, nowUtc, null);
        }

        public AfkRewardSummary SimulateAndApplyRewards(PlayerProfile profile, TimeSpan duration)
        {
            DateTime nowUtc = DateTime.UtcNow;
            double minutes = duration.TotalMinutes < 0d ? 0d : duration.TotalMinutes;
            return CalculateAndApplyRewards(profile, nowUtc, minutes);
        }

        public double GetCappedMinutes(DateTime startUtc, DateTime nowUtc, out bool wasCapped)
        {
            DateTime safeStart = EnsureUtc(startUtc);
            DateTime safeNow = EnsureUtc(nowUtc);
            double rawMinutes = (safeNow - safeStart).TotalMinutes;
            if (rawMinutes < 0d)
            {
                rawMinutes = 0d;
            }

            double maxMinutes = GameConstants.OfflineMaxHours * 60d;
            wasCapped = rawMinutes > maxMinutes;
            return wasCapped ? maxMinutes : rawMinutes;
        }

        private AfkRewardSummary CalculateAndApplyRewards(PlayerProfile profile, DateTime nowUtc, double? overrideMinutes)
        {
            AfkRewardSummary summary = new AfkRewardSummary();
            if (profile == null)
            {
                return summary;
            }

            profile.Normalize();
            DateTime safeNow = EnsureUtc(nowUtc);

            foreach (CharacterState character in profile.characters)
            {
                CharacterAfkRewardSummary characterSummary = CalculateAndApplyCharacterRewards(
                    profile,
                    character,
                    safeNow,
                    overrideMinutes,
                    summary);

                summary.characterRewards.Add(characterSummary);
                summary.hasAnyRewards |= characterSummary.hadRewards;
            }

            return summary;
        }

        private CharacterAfkRewardSummary CalculateAndApplyCharacterRewards(
            PlayerProfile profile,
            CharacterState character,
            DateTime nowUtc,
            double? overrideMinutes,
            AfkRewardSummary totalSummary)
        {
            CharacterAfkRewardSummary summary = new CharacterAfkRewardSummary();
            if (character == null)
            {
                return summary;
            }

            character.Normalize();
            TaskState task = character.currentTask ?? new TaskState();
            task.Normalize();

            summary.characterId = character.characterId ?? string.Empty;
            summary.characterName = character.displayName ?? string.Empty;
            summary.taskType = string.IsNullOrEmpty(task.taskType) ? GameConstants.TaskIdle : task.taskType;
            summary.targetId = task.targetId ?? string.Empty;

            double elapsedMinutes;
            bool wasCapped = false;
            if (overrideMinutes.HasValue)
            {
                double rawMinutes = overrideMinutes.Value < 0d ? 0d : overrideMinutes.Value;
                double maxMinutes = GameConstants.OfflineMaxHours * 60d;
                wasCapped = rawMinutes > maxMinutes;
                elapsedMinutes = wasCapped ? maxMinutes : rawMinutes;
                totalSummary.rawElapsedMinutes = Math.Max(totalSummary.rawElapsedMinutes, rawMinutes);
            }
            else
            {
                if (!TryParseUtc(task.startedUtc, out DateTime startedUtc))
                {
                    task.startedUtc = nowUtc.ToString("o");
                    return summary;
                }

                double rawMinutes = (nowUtc - startedUtc).TotalMinutes;
                if (rawMinutes < 0d)
                {
                    rawMinutes = 0d;
                }

                elapsedMinutes = GetCappedMinutes(startedUtc, nowUtc, out wasCapped);
                totalSummary.rawElapsedMinutes = Math.Max(totalSummary.rawElapsedMinutes, rawMinutes);
            }

            summary.elapsedMinutes = elapsedMinutes;
            totalSummary.cappedElapsedMinutes = Math.Max(totalSummary.cappedElapsedMinutes, elapsedMinutes);
            totalSummary.wasCapped |= wasCapped;

            if (elapsedMinutes <= 0d || summary.taskType == GameConstants.TaskIdle)
            {
                task.startedUtc = nowUtc.ToString("o");
                return summary;
            }

            CharacterStats stats = statsSystem.CalculateStats(character);
            float afkMultiplier = stats.afkGainMultiplier;
            if (afkMultiplier < 0f)
            {
                afkMultiplier = 0f;
            }

            float xpMultiplier = stats.xpGainMultiplier;
            if (xpMultiplier < 0f)
            {
                xpMultiplier = 0f;
            }

            summary.levelBefore = character.level;
            if (summary.taskType == GameConstants.TaskCombat && summary.targetId == GameConstants.EnemySlimeId)
            {
                ApplyCombatRewards(profile, character, summary, elapsedMinutes, afkMultiplier, xpMultiplier);
            }
            else if (summary.taskType == GameConstants.TaskMining && summary.targetId == GameConstants.ZoneMineCopperId)
            {
                ApplyMiningRewards(character, summary, elapsedMinutes, afkMultiplier, xpMultiplier);
            }

            summary.levelAfter = character.level;
            summary.leveledUp = summary.levelAfter > summary.levelBefore;
            task.startedUtc = nowUtc.ToString("o");
            return summary;
        }

        private void ApplyCombatRewards(
            PlayerProfile profile,
            CharacterState character,
            CharacterAfkRewardSummary summary,
            double elapsedMinutes,
            float afkMultiplier,
            float xpMultiplier)
        {
            summary.xpGained = CalculateReward(elapsedMinutes, GameConstants.OfflineCombatXpPerMinute, afkMultiplier * xpMultiplier);
            summary.coinsGained = CalculateReward(elapsedMinutes, GameConstants.OfflineCombatCoinsPerMinute, afkMultiplier);
            int slimeGoo = CalculateReward(elapsedMinutes, GameConstants.OfflineSlimeGooPerMinute, afkMultiplier);

            ProgressionResult progression = progressionSystem.AddXp(character, summary.xpGained);
            if (summary.coinsGained > 0)
            {
                profile.coins += summary.coinsGained;
            }

            AddItemReward(summary, GameConstants.ItemSlimeGooId, slimeGoo);
            summary.hadRewards = summary.xpGained > 0 || summary.coinsGained > 0 || slimeGoo > 0;
            RaiseEnemyKills(GameConstants.EnemySlimeId, slimeGoo);
            RaiseLevelReached(progression);
        }

        private void ApplyMiningRewards(
            CharacterState character,
            CharacterAfkRewardSummary summary,
            double elapsedMinutes,
            float afkMultiplier,
            float xpMultiplier)
        {
            summary.xpGained = CalculateReward(elapsedMinutes, GameConstants.OfflineMiningXpPerMinute, afkMultiplier * xpMultiplier);
            int copperOre = CalculateReward(elapsedMinutes, GameConstants.OfflineCopperOrePerMinute, afkMultiplier);

            ProgressionResult progression = progressionSystem.AddXp(character, summary.xpGained);
            AddItemReward(summary, GameConstants.ItemCopperOreId, copperOre);
            summary.hadRewards = summary.xpGained > 0 || copperOre > 0;
            RaiseLevelReached(progression);
        }

        private void AddItemReward(CharacterAfkRewardSummary summary, string itemId, int quantity)
        {
            if (string.IsNullOrEmpty(itemId) || quantity <= 0)
            {
                return;
            }

            inventorySystem?.AddItem(itemId, quantity);
            summary.itemsGained.Add(new InventoryStack(itemId, quantity));
            QuestGameplayEvents.RaiseItemCollected(itemId, quantity);
        }

        private static void RaiseEnemyKills(string enemyId, int killCount)
        {
            for (int i = 0; i < killCount; i++)
            {
                QuestGameplayEvents.RaiseEnemyKilled(enemyId);
            }
        }

        private static void RaiseLevelReached(ProgressionResult progression)
        {
            if (progression != null && progression.leveledUp)
            {
                QuestGameplayEvents.RaiseLevelReached(progression.newLevel);
            }
        }

        private static int CalculateReward(double elapsedMinutes, int perMinute, float multiplier)
        {
            return (int)Math.Floor(elapsedMinutes * perMinute * multiplier);
        }

        private static bool TryParseUtc(string utcText, out DateTime utc)
        {
            if (DateTime.TryParse(
                utcText,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                out utc))
            {
                utc = EnsureUtc(utc);
                return true;
            }

            utc = default;
            return false;
        }

        private static DateTime EnsureUtc(DateTime value)
        {
            return value.Kind == DateTimeKind.Utc ? value : value.ToUniversalTime();
        }
    }
}
