using System;

namespace IdleGuildDemo.Systems
{
    [Serializable]
    public sealed class QuestUpdateResult
    {
        public bool updated;
        public bool completed;
        public string questId = string.Empty;
        public int currentAmount;
        public int requiredAmount;
        public string message = string.Empty;
    }
}
