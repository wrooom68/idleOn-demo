using System;
using System.Collections.Generic;
using IdleGuildDemo.Core;
using IdleGuildDemo.Runtime;

namespace IdleGuildDemo.Systems
{
    public sealed class CharacterRosterSystem
    {
        private readonly PlayerProfile profile;

        public CharacterRosterSystem(PlayerProfile profile)
        {
            this.profile = profile ?? PlayerProfile.CreateDefault();
            this.profile.Normalize();
        }

        public IReadOnlyList<CharacterState> GetCharacters()
        {
            profile.Normalize();
            return profile.characters;
        }

        public CharacterState GetActiveCharacter()
        {
            profile.Normalize();
            return GetCharacterById(profile.activeCharacterId);
        }

        public bool SetActiveCharacter(string characterId)
        {
            if (!HasCharacter(characterId))
            {
                return false;
            }

            profile.activeCharacterId = characterId;
            return true;
        }

        public CharacterState GetCharacterById(string characterId)
        {
            if (string.IsNullOrEmpty(characterId))
            {
                return null;
            }

            profile.Normalize();

            foreach (CharacterState character in profile.characters)
            {
                if (character != null && character.characterId == characterId)
                {
                    return character;
                }
            }

            return null;
        }

        public bool HasCharacter(string characterId)
        {
            return GetCharacterById(characterId) != null;
        }

        public CharacterState UnlockSecondCharacter()
        {
            CharacterState existing = GetCharacterById(GameConstants.SecondCharacterId);
            if (existing != null)
            {
                existing.isUnlocked = true;
                existing.Normalize();
                return existing;
            }

            CharacterState character = new CharacterState
            {
                characterId = GameConstants.SecondCharacterId,
                displayName = "Character 2",
                level = 1,
                currentXp = 0,
                unspentTalentPoints = 0,
                selectedClassId = string.Empty,
                isUnlocked = true,
                currentTask = new TaskState
                {
                    taskType = GameConstants.TaskIdle
                }
            };

            character.Normalize();
            profile.characters.Add(character);
            profile.Normalize();
            return character;
        }

        public bool IsSecondCharacterUnlocked()
        {
            CharacterState character = GetCharacterById(GameConstants.SecondCharacterId);
            return character != null && character.isUnlocked;
        }

        public void AssignTask(string characterId, string taskType, string targetId)
        {
            CharacterState character = GetCharacterById(characterId);
            if (character == null)
            {
                return;
            }

            character.currentTask = new TaskState
            {
                taskType = string.IsNullOrEmpty(taskType) ? GameConstants.TaskIdle : taskType,
                targetId = targetId ?? string.Empty,
                startedUtc = DateTime.UtcNow.ToString("o")
            };
        }
    }
}
