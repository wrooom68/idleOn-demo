using IdleGuildDemo.Core;
using IdleGuildDemo.Runtime;

namespace IdleGuildDemo.Systems
{
    /// <summary>
    /// Owns future gathering task progress and mining reward rules.
    /// </summary>
    public sealed class GatheringSystem
    {
        private readonly InventorySystem inventorySystem;
        private readonly ProgressionSystem progressionSystem;
        private readonly StatsSystem statsSystem;

        public GatheringSystem(InventorySystem inventorySystem, ProgressionSystem progressionSystem, StatsSystem statsSystem)
        {
            this.inventorySystem = inventorySystem;
            this.progressionSystem = progressionSystem ?? new ProgressionSystem();
            this.statsSystem = statsSystem ?? new StatsSystem();
        }

        public void TickGathering(CharacterState character, float deltaTime)
        {
            // TODO: Route assigned gathering tasks into concrete activity tick methods.
        }

        public GatheringState CreateCopperMiningState(CharacterState character)
        {
            return new GatheringState
            {
                targetId = GameConstants.ZoneMineCopperId,
                elapsedSeconds = 0f,
                requiredSeconds = GetCopperMiningDurationSeconds(character)
            };
        }

        public GatheringTickResult TickCopperMining(CharacterState character, GatheringState state, float deltaSeconds)
        {
            GatheringTickResult result = new GatheringTickResult();

            if (character == null)
            {
                result.failureReason = "Character is missing.";
                return result;
            }

            if (state == null)
            {
                result.failureReason = "Gathering state is missing.";
                return result;
            }

            character.Normalize();
            state.Normalize();
            state.targetId = string.IsNullOrEmpty(state.targetId) ? GameConstants.ZoneMineCopperId : state.targetId;
            state.requiredSeconds = GetCopperMiningDurationSeconds(character);

            if (deltaSeconds <= 0f)
            {
                result.elapsedSeconds = state.elapsedSeconds;
                result.requiredSeconds = state.requiredSeconds;
                return result;
            }

            state.elapsedSeconds += deltaSeconds;
            result.progressed = true;

            if (state.IsComplete)
            {
                inventorySystem?.AddItem(GameConstants.ItemCopperOreId, GameConstants.MiningCopperOreRewardQuantity);
                progressionSystem.AddXp(character, GameConstants.MiningCopperXpReward);

                result.completed = true;
                result.itemGainedId = GameConstants.ItemCopperOreId;
                result.itemGainedQuantity = GameConstants.MiningCopperOreRewardQuantity;
                result.xpGained = GameConstants.MiningCopperXpReward;
                result.message = "Copper ore gathered.";

                state.elapsedSeconds = 0f;
                // TODO: Emit a quest progress event when quest systems subscribe to gathering results.
            }

            result.elapsedSeconds = state.elapsedSeconds;
            result.requiredSeconds = state.requiredSeconds;
            return result;
        }

        public float GetCopperMiningDurationSeconds(CharacterState character)
        {
            CharacterStats stats = statsSystem.CalculateStats(character);
            float multiplier = stats.miningSpeedMultiplier < 0.1f ? 0.1f : stats.miningSpeedMultiplier;
            return GameConstants.MiningCopperBaseDurationSeconds / multiplier;
        }
    }
}
