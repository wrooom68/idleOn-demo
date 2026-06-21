using System;

namespace IdleGuildDemo.Systems
{
    [Serializable]
    public sealed class GatheringTickResult
    {
        public bool progressed;
        public bool completed;
        public float elapsedSeconds;
        public float requiredSeconds;
        public string itemGainedId = string.Empty;
        public int itemGainedQuantity;
        public int xpGained;
        public string message = string.Empty;
        public string failureReason = string.Empty;
    }
}
