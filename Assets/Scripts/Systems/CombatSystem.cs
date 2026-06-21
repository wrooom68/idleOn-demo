using IdleGuildDemo.Core;
using IdleGuildDemo.Data;
using IdleGuildDemo.Runtime;

namespace IdleGuildDemo.Systems
{
    /// <summary>
    /// Owns future automatic combat state updates and combat reward events.
    /// </summary>
    public sealed class CombatSystem
    {
        private readonly InventorySystem inventorySystem;
        private readonly ProgressionSystem progressionSystem;
        private readonly StatsSystem statsSystem;

        public CombatSystem(InventorySystem inventorySystem, ProgressionSystem progressionSystem, StatsSystem statsSystem)
        {
            this.inventorySystem = inventorySystem;
            this.progressionSystem = progressionSystem ?? new ProgressionSystem();
            this.statsSystem = statsSystem ?? new StatsSystem();
        }

        public void TickCombat(CharacterState character, float deltaTime)
        {
            // TODO: Drive timed auto-attacks and conceptual respawns from saved task state.
        }

        public CombatEnemyState CreateEnemyState(EnemyDefinition enemyDefinition)
        {
            if (enemyDefinition == null)
            {
                return null;
            }

            int maxHp = enemyDefinition.MaxHp < 1 ? 1 : enemyDefinition.MaxHp;
            return new CombatEnemyState
            {
                enemyId = enemyDefinition.Id ?? string.Empty,
                maxHp = maxHp,
                currentHp = maxHp
            };
        }

        public CombatTickResult Attack(CharacterState character, CombatEnemyState enemyState, EnemyDefinition enemyDefinition)
        {
            CombatTickResult result = new CombatTickResult
            {
                enemyState = enemyState
            };

            if (character == null)
            {
                result.failureReason = "Character is missing.";
                return result;
            }

            if (enemyState == null)
            {
                result.failureReason = "Enemy state is missing.";
                return result;
            }

            if (enemyDefinition == null)
            {
                result.failureReason = "Enemy definition is missing.";
                return result;
            }

            character.Normalize();
            enemyState.Normalize();

            if (enemyState.IsDefeated)
            {
                result.enemyDefeated = true;
                result.failureReason = "Enemy is already defeated.";
                return result;
            }

            CharacterStats stats = statsSystem.CalculateStats(character);
            int damage = stats.damage < 1 ? 1 : stats.damage;

            enemyState.currentHp -= damage;
            if (enemyState.currentHp < 0)
            {
                enemyState.currentHp = 0;
            }

            result.attacked = true;
            result.damageDealt = damage;
            result.enemyDefeated = enemyState.IsDefeated;

            if (result.enemyDefeated)
            {
                result.xpGained = enemyDefinition.XpReward;
                progressionSystem.AddXp(character, result.xpGained);

                result.coinsGained = GetCoinReward(enemyDefinition);
                result.itemDroppedId = GetItemDropId(enemyDefinition);
                result.itemDroppedQuantity = GetItemDropQuantity(enemyDefinition);

                if (!string.IsNullOrEmpty(result.itemDroppedId) && result.itemDroppedQuantity > 0)
                {
                    inventorySystem?.AddItem(result.itemDroppedId, result.itemDroppedQuantity);
                }

                result.message = "Enemy defeated.";
                // TODO: Emit a quest progress event when quest systems subscribe to combat results.
            }

            return result;
        }

        public CombatTickResult AttackSlime(CharacterState character, EnemyDefinition slimeDefinition)
        {
            return Attack(character, CreateEnemyState(slimeDefinition), slimeDefinition);
        }

        private static int GetCoinReward(EnemyDefinition enemyDefinition)
        {
            return enemyDefinition != null && enemyDefinition.Id == GameConstants.EnemySlimeId
                ? GameConstants.CombatSlimeBaseCoins
                : 0;
        }

        private static string GetItemDropId(EnemyDefinition enemyDefinition)
        {
            return enemyDefinition != null && enemyDefinition.Id == GameConstants.EnemySlimeId
                ? GameConstants.ItemSlimeGooId
                : string.Empty;
        }

        private static int GetItemDropQuantity(EnemyDefinition enemyDefinition)
        {
            return enemyDefinition != null && enemyDefinition.Id == GameConstants.EnemySlimeId
                ? GameConstants.CombatSlimeGooDropQuantity
                : 0;
        }
    }
}
