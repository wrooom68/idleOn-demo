using IdleGuildDemo.Core;
using IdleGuildDemo.Runtime;

namespace IdleGuildDemo.Systems
{
    /// <summary>
    /// Owns Beginner-to-specialized-class validation and stat modifiers.
    /// </summary>
    public sealed class ClassSelectionSystem
    {
        public const string BeginnerClassId = "beginner";
        public const int WarriorDamageBonus = 2;
        public const float ArcherDropRateBonus = 0.10f;
        public const float MageAfkGainBonus = 0.10f;

        public string GetCurrentClassId(CharacterState character)
        {
            if (character == null || string.IsNullOrEmpty(character.selectedClassId))
            {
                return BeginnerClassId;
            }

            return character.selectedClassId;
        }

        public string GetClassDisplayName(string classId)
        {
            switch (classId)
            {
                case GameConstants.WarriorClassId:
                    return "Warrior";
                case GameConstants.ArcherClassId:
                    return "Archer";
                case GameConstants.MageClassId:
                    return "Mage";
                case BeginnerClassId:
                case "":
                case null:
                    return "Beginner";
                default:
                    return classId;
            }
        }

        public bool CanChooseClass(CharacterState character)
        {
            return character != null
                && character.level >= GameConstants.ClassUnlockLevel
                && !HasChosenSpecializedClass(character);
        }

        public bool HasChosenSpecializedClass(CharacterState character)
        {
            return character != null && IsSpecializedClassId(character.selectedClassId);
        }

        public bool IsKnownClassId(string classId)
        {
            return string.IsNullOrEmpty(classId)
                || classId == BeginnerClassId
                || IsSpecializedClassId(classId);
        }

        public bool IsSpecializedClassId(string classId)
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
            result.previousClassId = GetCurrentClassId(character);

            if (!IsSpecializedClassId(classId))
            {
                result.failureReason = "Invalid class ID.";
                return result;
            }

            if (character.level < GameConstants.ClassUnlockLevel)
            {
                result.failureReason = "Class choice unlocks at level 5.";
                return result;
            }

            if (HasChosenSpecializedClass(character))
            {
                result.failureReason = "Class has already been chosen.";
                result.selectedClassId = character.selectedClassId;
                result.selectedClassDisplayName = GetClassDisplayName(character.selectedClassId);
                return result;
            }

            character.selectedClassId = classId;
            result.success = true;
            result.selectedClassId = classId;
            result.selectedClassDisplayName = GetClassDisplayName(classId);
            return result;
        }

        public void ApplyClassModifiers(CharacterState character, CharacterStats stats)
        {
            if (stats == null)
            {
                return;
            }

            ApplyClassModifiers(GetCurrentClassId(character), stats);
        }

        public void ApplyClassModifiers(string classId, CharacterStats stats)
        {
            if (stats == null)
            {
                return;
            }

            switch (classId)
            {
                case GameConstants.WarriorClassId:
                    stats.damage += WarriorDamageBonus;
                    break;
                case GameConstants.ArcherClassId:
                    stats.dropRateMultiplier += ArcherDropRateBonus;
                    break;
                case GameConstants.MageClassId:
                    stats.afkGainMultiplier += MageAfkGainBonus;
                    break;
            }
        }
    }
}
