using System;
using System.Collections.Generic;

namespace IdleGuildDemo.Runtime
{
    [Serializable]
    public sealed class SaveData
    {
        public PlayerProfile playerProfile = new PlayerProfile();
        public InventoryState inventory = new InventoryState();
        public List<CharacterState> characters = new List<CharacterState>();
        public List<QuestProgressState> quests = new List<QuestProgressState>();
        public string currentQuestId = string.Empty;
        public long lastSavedUnixTime;
    }
}
