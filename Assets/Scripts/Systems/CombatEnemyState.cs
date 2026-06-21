using System;

namespace IdleGuildDemo.Systems
{
    [Serializable]
    public sealed class CombatEnemyState
    {
        public string enemyId = string.Empty;
        public int maxHp;
        public int currentHp;

        public bool IsDefeated => currentHp <= 0;

        public void Normalize()
        {
            if (enemyId == null)
            {
                enemyId = string.Empty;
            }

            if (maxHp < 0)
            {
                maxHp = 0;
            }

            if (currentHp < 0)
            {
                currentHp = 0;
            }

            if (maxHp > 0 && currentHp > maxHp)
            {
                currentHp = maxHp;
            }
        }
    }
}
