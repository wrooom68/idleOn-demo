using System;
using System.Collections.Generic;
using IdleGuildDemo.Core;

namespace IdleGuildDemo.Runtime
{
    [Serializable]
    public sealed class PlayerProfile
    {
        public string playerId = "local_player";
        public string displayName = "Reviewer";
        public List<CharacterState> characters = new List<CharacterState>();
        public string activeCharacterId = GameConstants.StartingCharacterId;
        public InventoryState inventory = new InventoryState();
        public int coins;
        public List<QuestProgressState> quests = new List<QuestProgressState>();
        public string currentQuestId = string.Empty;

        public static PlayerProfile CreateDefault()
        {
            return new PlayerProfile
            {
                playerId = "local_player",
                displayName = "Reviewer",
                coins = 0,
                activeCharacterId = GameConstants.StartingCharacterId,
                characters = new List<CharacterState>
                {
                    CreateStartingCharacter()
                },
                inventory = new InventoryState(),
                quests = new List<QuestProgressState>(),
                currentQuestId = string.Empty
            };
        }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(playerId))
            {
                playerId = "local_player";
            }

            if (string.IsNullOrEmpty(displayName))
            {
                displayName = "Reviewer";
            }

            if (characters == null)
            {
                characters = new List<CharacterState>();
            }

            for (int i = characters.Count - 1; i >= 0; i--)
            {
                if (characters[i] == null)
                {
                    characters.RemoveAt(i);
                }
            }

            if (characters.Count == 0)
            {
                characters.Add(CreateStartingCharacter());
            }

            foreach (CharacterState character in characters)
            {
                character?.Normalize();
            }

            if (!HasCharacter(activeCharacterId))
            {
                activeCharacterId = characters[0].characterId;
            }

            if (inventory == null)
            {
                inventory = new InventoryState();
            }

            inventory.Normalize();

            if (coins < 0)
            {
                coins = 0;
            }

            if (quests == null)
            {
                quests = new List<QuestProgressState>();
            }

            if (currentQuestId == null)
            {
                currentQuestId = string.Empty;
            }
        }

        private bool HasCharacter(string characterId)
        {
            if (string.IsNullOrEmpty(characterId))
            {
                return false;
            }

            foreach (CharacterState character in characters)
            {
                if (character != null && character.characterId == characterId)
                {
                    return true;
                }
            }

            return false;
        }

        private static CharacterState CreateStartingCharacter()
        {
            return new CharacterState
            {
                characterId = GameConstants.StartingCharacterId,
                displayName = "Ruchir",
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
        }
    }
}
