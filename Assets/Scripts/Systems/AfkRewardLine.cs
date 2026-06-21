using System;

namespace IdleGuildDemo.Systems
{
    [Serializable]
    public sealed class AfkRewardLine
    {
        public string itemId = string.Empty;
        public int quantity;
        public int xp;
        public int coins;
        public string label = string.Empty;
    }
}
