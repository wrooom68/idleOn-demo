using IdleGuildDemo.Core;
using IdleGuildDemo.Runtime;

namespace IdleGuildDemo.Systems
{
    /// <summary>
    /// Owns four-talent validation, spending, ranks, and stat modifiers.
    /// </summary>
    public sealed class TalentSystem
    {
        public const int MaxTalentRank = 5;
        public const int DamagePerRank = 1;
        public const float MiningSpeedPerRank = 0.05f;
        public const float XpGainPerRank = 0.05f;
        public const float AfkGainPerRank = 0.05f;

        public bool IsValidTalentId(string talentId)
        {
            return talentId == GameConstants.DamageTalentId
                || talentId == GameConstants.MiningSpeedTalentId
                || talentId == GameConstants.XpGainTalentId
                || talentId == GameConstants.AfkGainTalentId;
        }

        public string GetTalentDisplayName(string talentId)
        {
            switch (talentId)
            {
                case GameConstants.DamageTalentId:
                    return "Damage";
                case GameConstants.MiningSpeedTalentId:
                    return "Mining Speed";
                case GameConstants.XpGainTalentId:
                    return "XP Gain";
                case GameConstants.AfkGainTalentId:
                    return "AFK Gain";
                default:
                    return "Talent";
            }
        }

        public int GetTalentRank(CharacterState character, string talentId)
        {
            if (character == null || !IsValidTalentId(talentId))
            {
                return 0;
            }

            character.Normalize();
            TalentState talent = FindTalent(character, talentId);
            return talent != null ? talent.rank : 0;
        }

        public bool CanSpendTalentPoint(CharacterState character, string talentId)
        {
            return character != null
                && IsValidTalentId(talentId)
                && character.unspentTalentPoints > 0
                && GetTalentRank(character, talentId) < MaxTalentRank;
        }

        public TalentSpendResult SpendTalentPoint(CharacterState character, string talentId)
        {
            TalentSpendResult result = new TalentSpendResult
            {
                talentId = talentId ?? string.Empty,
                maxRank = MaxTalentRank
            };

            if (character == null)
            {
                result.failureReason = "Character is missing.";
                return result;
            }

            character.Normalize();
            result.remainingTalentPoints = character.unspentTalentPoints;

            if (!IsValidTalentId(talentId))
            {
                result.failureReason = "Invalid talent ID.";
                return result;
            }

            TalentState talent = FindTalent(character, talentId);
            if (talent == null)
            {
                talent = new TalentState(talentId, 0);
                character.talents.Add(talent);
            }

            talent.Normalize();
            result.oldRank = talent.rank;

            if (talent.rank >= MaxTalentRank)
            {
                result.newRank = talent.rank;
                result.failureReason = "Talent is already at max rank.";
                return result;
            }

            if (character.unspentTalentPoints <= 0)
            {
                result.newRank = talent.rank;
                result.failureReason = "No unspent talent points.";
                return result;
            }

            talent.rank++;
            character.unspentTalentPoints--;
            result.newRank = talent.rank;
            result.remainingTalentPoints = character.unspentTalentPoints;
            result.success = true;
            return result;
        }

        public void ApplyTalentModifiers(CharacterState character, CharacterStats stats)
        {
            if (character == null || stats == null)
            {
                return;
            }

            character.Normalize();
            foreach (TalentState talent in character.talents)
            {
                if (talent == null || talent.rank <= 0)
                {
                    continue;
                }

                int safeRank = talent.rank > MaxTalentRank ? MaxTalentRank : talent.rank;
                switch (talent.talentId)
                {
                    case GameConstants.DamageTalentId:
                        stats.damage += DamagePerRank * safeRank;
                        break;
                    case GameConstants.MiningSpeedTalentId:
                        stats.miningSpeedMultiplier += MiningSpeedPerRank * safeRank;
                        break;
                    case GameConstants.XpGainTalentId:
                        stats.xpGainMultiplier += XpGainPerRank * safeRank;
                        break;
                    case GameConstants.AfkGainTalentId:
                        stats.afkGainMultiplier += AfkGainPerRank * safeRank;
                        break;
                }
            }
        }

        private static TalentState FindTalent(CharacterState character, string talentId)
        {
            if (character?.talents == null)
            {
                return null;
            }

            foreach (TalentState talent in character.talents)
            {
                if (talent != null && talent.talentId == talentId)
                {
                    return talent;
                }
            }

            return null;
        }
    }
}
