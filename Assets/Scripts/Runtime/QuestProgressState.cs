using System;
using System.Collections.Generic;
using IdleGuildDemo.Core;

namespace IdleGuildDemo.Runtime
{
    [Serializable]
    public sealed class QuestProgressState
    {
        public string currentQuestId = GameConstants.QuestKillSlimesId;
        public int currentAmount;
        public bool isComplete;
        public List<string> completedQuestIds = new List<string>();

        // Legacy per-quest fields kept JSON-friendly for older saves created before the linear chain state.
        public string questId = string.Empty;
        public int requiredAmount;
        public bool rewardClaimed;

        public void Normalize()
        {
            if (completedQuestIds == null)
            {
                completedQuestIds = new List<string>();
            }

            if (string.IsNullOrEmpty(currentQuestId))
            {
                currentQuestId = completedQuestIds.Contains(GameConstants.QuestUnlockCharacter2Id)
                    ? string.Empty
                    : string.IsNullOrEmpty(questId) ? GameConstants.QuestKillSlimesId : questId;
            }

            questId = currentQuestId;

            if (currentAmount < 0)
            {
                currentAmount = 0;
            }

            if (requiredAmount < 0)
            {
                requiredAmount = 0;
            }

            for (int i = completedQuestIds.Count - 1; i >= 0; i--)
            {
                if (string.IsNullOrEmpty(completedQuestIds[i]))
                {
                    completedQuestIds.RemoveAt(i);
                }
            }
        }
    }
}
