using System;

namespace IdleGuildDemo.Systems
{
    [Serializable]
    public sealed class GatheringState
    {
        public string targetId = string.Empty;
        public float elapsedSeconds;
        public float requiredSeconds;

        public bool IsComplete => elapsedSeconds >= requiredSeconds;

        public void Normalize()
        {
            if (targetId == null)
            {
                targetId = string.Empty;
            }

            if (elapsedSeconds < 0f)
            {
                elapsedSeconds = 0f;
            }

            if (requiredSeconds < 0f)
            {
                requiredSeconds = 0f;
            }
        }
    }
}
