using System;

namespace IdleGuildDemo.Systems
{
    [Serializable]
    public sealed class CharacterStats
    {
        public int damage = 3;
        public float miningSpeedMultiplier = 1f;
        public float xpGainMultiplier = 1f;
        public float dropRateMultiplier = 1f;
        public float afkGainMultiplier = 1f;
    }
}
