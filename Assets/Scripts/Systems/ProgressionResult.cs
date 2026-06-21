using System;

namespace IdleGuildDemo.Systems
{
    [Serializable]
    public sealed class ProgressionResult
    {
        public int xpAdded;
        public int oldLevel;
        public int newLevel;
        public int levelsGained;
        public int talentPointsGained;
        public bool leveledUp;
    }
}
