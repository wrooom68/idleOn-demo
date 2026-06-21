using System;

namespace IdleGuildDemo.Systems
{
    [Serializable]
    public sealed class CombatTickResult
    {
        public bool attacked;
        public int damageDealt;
        public bool enemyDefeated;
        public int xpGained;
        public int coinsGained;
        public string itemDroppedId = string.Empty;
        public int itemDroppedQuantity;
        public CombatEnemyState enemyState;
        public string message = string.Empty;
        public string failureReason = string.Empty;
    }
}
