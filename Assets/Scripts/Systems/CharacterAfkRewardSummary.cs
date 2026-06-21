using System;
using System.Collections.Generic;
using IdleGuildDemo.Runtime;

namespace IdleGuildDemo.Systems
{
    [Serializable]
    public sealed class CharacterAfkRewardSummary
    {
        public string characterId = string.Empty;
        public string characterName = string.Empty;
        public string taskType = string.Empty;
        public string targetId = string.Empty;
        public double elapsedMinutes;
        public int xpGained;
        public int coinsGained;
        public List<InventoryStack> itemsGained = new List<InventoryStack>();
        public bool hadRewards;
    }
}
