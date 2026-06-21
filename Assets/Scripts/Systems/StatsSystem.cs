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

        public CharacterStats CalculateStats(CharacterState character)
        {
            CharacterStats stats = new CharacterStats();

            if (character == null)
            {
                return stats;
            }

            character.Normalize();
            classSelectionSystem.ApplyClassModifiers(character, stats);
            ApplyTalentBonuses(character, stats);
            return stats;
        }

        private static void ApplyTalentBonuses(CharacterState character, CharacterStats stats)
        {
            if (character.talents == null)
            {
                return;
            }

            foreach (TalentState talent in character.talents)
            {
                if (talent == null || talent.rank <= 0)
                {
                    continue;
                }

                switch (talent.talentId)
                {
                    case GameConstants.DamageTalentId:
                        stats.damage += talent.rank;
                        break;
                    case GameConstants.MiningSpeedTalentId:
                        stats.miningSpeedMultiplier += 0.05f * talent.rank;
                        break;
                    case GameConstants.XpGainTalentId:
                        stats.xpGainMultiplier += 0.05f * talent.rank;
                        break;
                    case GameConstants.AfkGainTalentId:
                        stats.afkGainMultiplier += 0.05f * talent.rank;
                        break;
                }
            }
        }
    }
}
