using System;
using System.Collections.Generic;

namespace IdleGuildDemo.Runtime
{
    [Serializable]
    public sealed class CharacterState
    {
        public string characterId = string.Empty;
        public string displayName = string.Empty;
        public int level = 1;
        public int currentXp;
        public int unspentTalentPoints;
        public string selectedClassId = string.Empty;
        public bool isUnlocked;
        public List<TalentState> talents = new List<TalentState>();
        public TaskState currentTask = new TaskState();

        public void Normalize()
        {
            if (characterId == null)
            {
                characterId = string.Empty;
            }

            if (displayName == null)
            {
                displayName = string.Empty;
            }

            if (level < 1)
            {
                level = 1;
            }

            if (currentXp < 0)
            {
                currentXp = 0;
            }

            if (unspentTalentPoints < 0)
            {
                unspentTalentPoints = 0;
            }

            if (selectedClassId == null)
            {
                selectedClassId = string.Empty;
            }

            if (talents == null)
            {
                talents = new List<TalentState>();
            }

            for (int i = talents.Count - 1; i >= 0; i--)
            {
                TalentState talent = talents[i];
                if (talent == null)
                {
                    talents.RemoveAt(i);
                    continue;
                }

                talent.Normalize();
            }

            if (currentTask == null)
            {
                currentTask = new TaskState();
            }
        }
    }
}
