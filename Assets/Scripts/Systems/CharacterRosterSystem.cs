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

        public TaskAssignmentResult AssignIdle(string characterId)
        {
            return AssignTask(characterId, GameConstants.TaskIdle, string.Empty);
        }

        public TaskAssignmentResult AssignSlimeCombat(string characterId)
        {
            return AssignTask(characterId, GameConstants.TaskCombat, GameConstants.EnemySlimeId);
        }

        public TaskAssignmentResult AssignCopperMining(string characterId)
        {
            return AssignTask(characterId, GameConstants.TaskMining, GameConstants.ZoneMineCopperId);
        }

        public TaskAssignmentResult AssignTask(string characterId, string taskType, string targetId)
        {
            TaskAssignmentResult result = new TaskAssignmentResult
            {
                characterId = characterId ?? string.Empty,
                taskType = string.IsNullOrEmpty(taskType) ? GameConstants.TaskIdle : taskType,
                targetId = targetId ?? string.Empty
            };

            CharacterState character = GetCharacterById(characterId);
            if (character == null)
            {
                result.failureReason = "Character is missing.";
                return result;
            }

            character.Normalize();
            if (!character.isUnlocked)
            {
                result.failureReason = "Character is locked.";
                return result;
            }

            if (!IsValidTask(result.taskType, result.targetId))
            {
                result.failureReason = "Task is not available.";
                return result;
            }

            TaskState previousTask = character.currentTask ?? new TaskState();
            previousTask.Normalize();
            result.previousTaskType = previousTask.taskType;
            result.previousTargetId = previousTask.targetId;

            string startedUtc = DateTime.UtcNow.ToString("o");
            result.startedUtc = startedUtc;
            character.currentTask = new TaskState
            {
                taskType = result.taskType,
                targetId = GetNormalizedTarget(result.taskType, result.targetId),
                startedUtc = startedUtc
            };
            result.targetId = character.currentTask.targetId;
            result.success = true;
            return result;
        }

        public bool CanAssignTask(string characterId, string taskType, string targetId)
        {
            CharacterState character = GetCharacterById(characterId);
            return character != null
                && character.isUnlocked
                && IsValidTask(string.IsNullOrEmpty(taskType) ? GameConstants.TaskIdle : taskType, targetId);
        }

        public static bool IsValidTask(string taskType, string targetId)
        {
            if (string.IsNullOrEmpty(taskType) || taskType == GameConstants.TaskIdle)
            {
                return true;
            }

            if (taskType == GameConstants.TaskCombat)
            {
                return targetId == GameConstants.EnemySlimeId;
            }

            if (taskType == GameConstants.TaskMining)
            {
                return targetId == GameConstants.ZoneMineCopperId;
            }

            return false;
        }

        private static string GetNormalizedTarget(string taskType, string targetId)
        {
            if (string.IsNullOrEmpty(taskType) || taskType == GameConstants.TaskIdle)
            {
                return string.Empty;
            }

            return targetId ?? string.Empty;
        }
    }
}
