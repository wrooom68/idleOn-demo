using IdleGuildDemo.Core;
using IdleGuildDemo.Runtime;

namespace IdleGuildDemo.Systems
{
    /// <summary>
    /// Owns character XP, level, class unlock, and talent point progression rules.
    /// </summary>
    public sealed class ProgressionSystem
    {
        public int GetXpRequiredForLevel(int level)
        {
            int safeLevel = level < 1 ? 1 : level;
            return 25 + safeLevel * 20;
        }

        public ProgressionResult AddXp(CharacterState character, int amount)
        {
            ProgressionResult result = new ProgressionResult();

            if (character == null)
            {
                return result;
            }

            character.Normalize();
            result.oldLevel = character.level;
            result.newLevel = character.level;

            if (amount <= 0)
            {
                return result;
            }

            result.xpAdded = amount;
            character.currentXp += amount;

            while (character.currentXp >= GetXpRequiredForLevel(character.level))
            {
                character.currentXp -= GetXpRequiredForLevel(character.level);
                character.level++;
                character.unspentTalentPoints++;
                result.levelsGained++;
                result.talentPointsGained++;
            }

            result.newLevel = character.level;
            result.leveledUp = result.levelsGained > 0;
            return result;
        }

        public bool CanChooseClass(CharacterState character)
        {
            return character != null
                && character.level >= GameConstants.ClassUnlockLevel
                && !HasChosenClass(character);
        }

        public bool HasChosenClass(CharacterState character)
        {
            return character != null && !string.IsNullOrEmpty(character.selectedClassId);
        }

        public bool IsValidClassId(string classId)
        {
            return classId == GameConstants.WarriorClassId
                || classId == GameConstants.ArcherClassId
                || classId == GameConstants.MageClassId;
        }

        public ClassSelectionResult ChooseClass(CharacterState character, string classId)
        {
            ClassSelectionResult result = new ClassSelectionResult
            {
                selectedClassId = classId ?? string.Empty
            };

            if (character == null)
            {
                result.failureReason = "Character is missing.";
                return result;
            }

            character.Normalize();

            if (!IsValidClassId(classId))
            {
                result.failureReason = "Invalid class ID.";
                return result;
            }

            if (character.level < GameConstants.ClassUnlockLevel)
            {
                result.failureReason = "Class choice unlocks at level 5.";
                return result;
            }

            if (HasChosenClass(character))
            {
                result.failureReason = "Class has already been chosen.";
                result.selectedClassId = character.selectedClassId;
                return result;
            }

            character.selectedClassId = classId;
            result.success = true;
            result.selectedClassId = classId;
            return result;
        }

        public bool IsValidTalentId(string talentId)
        {
            return talentId == GameConstants.DamageTalentId
                || talentId == GameConstants.MiningSpeedTalentId
                || talentId == GameConstants.XpGainTalentId
                || talentId == GameConstants.AfkGainTalentId;
        }

        public int GetTalentRank(CharacterState character, string talentId)
        {
            if (character == null || !IsValidTalentId(talentId))
            {
                return 0;
            }

            character.Normalize();

            foreach (TalentState talent in character.talents)
            {
                if (talent.talentId == talentId)
                {
                    return talent.rank;
                }
            }

            return 0;
        }

        public TalentSpendResult SpendTalentPoint(CharacterState character, string talentId)
        {
            TalentSpendResult result = new TalentSpendResult
            {
                talentId = talentId ?? string.Empty
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

            if (character.unspentTalentPoints <= 0)
            {
                result.failureReason = "No unspent talent points.";
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
            talent.rank++;
            character.unspentTalentPoints--;
            result.newRank = talent.rank;
            result.remainingTalentPoints = character.unspentTalentPoints;
            result.success = true;
            return result;
        }

        private static TalentState FindTalent(CharacterState character, string talentId)
        {
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
