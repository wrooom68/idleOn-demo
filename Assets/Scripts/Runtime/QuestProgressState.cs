using System;

namespace IdleGuildDemo.Runtime
{
    [Serializable]
    public sealed class QuestProgressState
    {
        public string questId = string.Empty;
        public int currentAmount;
        public int requiredAmount;
        public bool isComplete;
        public bool rewardClaimed;
    }
}
