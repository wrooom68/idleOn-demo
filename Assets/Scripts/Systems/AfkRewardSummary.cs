using System;
using System.Collections.Generic;

namespace IdleGuildDemo.Systems
{
    [Serializable]
    public sealed class AfkRewardSummary
    {
        public double rawElapsedMinutes;
        public double cappedElapsedMinutes;
        public bool wasCapped;
        public List<CharacterAfkRewardSummary> characterRewards = new List<CharacterAfkRewardSummary>();
        public bool hasAnyRewards;
    }
}
