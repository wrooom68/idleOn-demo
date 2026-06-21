using IdleGuildDemo.Core;
using IdleGuildDemo.Runtime;

namespace IdleGuildDemo.Systems
{
    /// <summary>
    /// Owns future derived stat calculations from equipment, class, and talents.
    /// </summary>
    public sealed class StatsSystem
    {
        private readonly ClassSelectionSystem classSelectionSystem = new ClassSelectionSystem();
        private readonly TalentSystem talentSystem = new TalentSystem();

        public CharacterStats CalculateStats(CharacterState character)
        {
            CharacterStats stats = new CharacterStats();

            if (character == null)
            {
                return stats;
            }

            character.Normalize();
            classSelectionSystem.ApplyClassModifiers(character, stats);
            talentSystem.ApplyTalentModifiers(character, stats);
            return stats;
        }
    }
}
