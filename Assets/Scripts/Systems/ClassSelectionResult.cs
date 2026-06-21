using System;

namespace IdleGuildDemo.Systems
{
    [Serializable]
    public sealed class ClassSelectionResult
    {
        public bool success;
        public string selectedClassId = string.Empty;
        public string failureReason = string.Empty;
    }
}
