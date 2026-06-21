using IdleGuildDemo.Core;
using IdleGuildDemo.Runtime;

namespace IdleGuildDemo.Systems
{
    /// <summary>
    /// Owns character XP, level, class unlock, and talent point progression rules.
    /// </summary>
    public sealed class ProgressionSystem
    {
        private readonly ClassSelectionSystem classSelectionSystem = new ClassSelectionSystem();
        private readonly TalentSystem talentSystem = new TalentSystem();

        public int GetXpRequiredForLevel(int level)
        {
            int safeLevel = level < 1 ? 1 : level;
            return GameConstants.ProgressionBaseXpRequired + safeLevel * GameConstants.ProgressionXpRequiredPerLevel;
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
            return classSelectionSystem.CanChooseClass(character);
        }

        public bool HasChosenClass(CharacterState character)
        {
            return classSelectionSystem.HasChosenSpecializedClass(character);
        }

        public bool IsValidClassId(string classId)
        {
            return classSelectionSystem.IsSpecializedClassId(classId);
        }

        public ClassSelectionResult ChooseClass(CharacterState character, string classId)
        {
            return classSelectionSystem.ChooseClass(character, classId);
        }

        public bool IsValidTalentId(string talentId)
        {
            return talentSystem.IsValidTalentId(talentId);
        }

        public int GetTalentRank(CharacterState character, string talentId)
        {
            return talentSystem.GetTalentRank(character, talentId);
        }

        public bool CanSpendTalentPoint(CharacterState character, string talentId)
        {
            return talentSystem.CanSpendTalentPoint(character, talentId);
        }

        public TalentSpendResult SpendTalentPoint(CharacterState character, string talentId)
        {
            return talentSystem.SpendTalentPoint(character, talentId);
        }
    }
}
