using System;
using System.Collections.Generic;

namespace IdleGuildDemo.Runtime
{
    [Serializable]
    public sealed class CharacterState
    {
        public string id = string.Empty;
        public string displayName = string.Empty;
        public int level = 1;
        public int xp;
        public string classId = string.Empty;
        public int talentPoints;
        public bool isUnlocked;
        public TaskState currentTask = new TaskState();
        public List<TalentState> talents = new List<TalentState>();
    }
}
