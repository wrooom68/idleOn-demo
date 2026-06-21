using System;

namespace IdleGuildDemo.Runtime
{
    [Serializable]
    public sealed class TaskState
    {
        public string taskType = string.Empty;
        public string targetId = string.Empty;
        public string startedUtc = string.Empty;

        public void Normalize()
        {
            if (taskType == null)
            {
                taskType = string.Empty;
            }

            if (targetId == null)
            {
                targetId = string.Empty;
            }

            if (startedUtc == null)
            {
                startedUtc = string.Empty;
            }
        }
    }
}
