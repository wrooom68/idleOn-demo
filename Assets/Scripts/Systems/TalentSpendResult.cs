using System;

namespace IdleGuildDemo.Systems
{
    [Serializable]
    public sealed class TalentSpendResult
    {
        public bool success;
        public string talentId = string.Empty;
        public int oldRank;
        public int newRank;
        public int remainingTalentPoints;
        public string failureReason = string.Empty;
    }
}
