using System;

namespace IdleGuildDemo.Systems
{
    [Serializable]
    public sealed class TaskAssignmentResult
    {
        public bool success;
        public string characterId = string.Empty;
        public string previousTaskType = string.Empty;
        public string previousTargetId = string.Empty;
        public string taskType = string.Empty;
        public string targetId = string.Empty;
        public string startedUtc = string.Empty;
        public string failureReason = string.Empty;
    }
}
