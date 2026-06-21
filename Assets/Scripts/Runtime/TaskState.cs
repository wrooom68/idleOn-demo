using System;

namespace IdleGuildDemo.Runtime
{
    [Serializable]
    public sealed class TaskState
    {
        public string taskType = string.Empty;
        public string zoneId = string.Empty;
        public string targetId = string.Empty;
        public float progress;
    }
}
