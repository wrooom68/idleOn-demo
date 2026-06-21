using IdleGuildDemo.Runtime;

namespace IdleGuildDemo.Systems
{
    /// <summary>
    /// Owns future derived stat calculations from equipment, class, and talents.
    /// </summary>
    public sealed class StatsSystem
    {
        public float GetDamageMultiplier(CharacterState character)
        {
            // TODO: Implement derived combat stats.
            return 1f;
        }

        public float GetMiningSpeedMultiplier(CharacterState character)
        {
            // TODO: Implement derived gathering stats.
            return 1f;
        }
    }
}
