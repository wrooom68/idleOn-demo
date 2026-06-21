using System;
using System.Collections.Generic;

namespace IdleGuildDemo.Runtime
{
    [Serializable]
    public sealed class PlayerProfile
    {
        public string playerId = "local_player";
        public string displayName = "Reviewer";
        public List<CharacterState> characters = new List<CharacterState>();
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
                characters = new List<CharacterState>
                {
                    new CharacterState
                    {
                        id = "character_01",
                        displayName = "Ruchir",
                        level = 1,
                        xp = 0,
                        classId = string.Empty,
                        talentPoints = 0,
                        isUnlocked = true
                    }
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

            if (characters.Count == 0)
            {
                characters.Add(PlayerProfile.CreateDefault().characters[0]);
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
    }
}
