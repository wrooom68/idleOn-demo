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
    }
}
