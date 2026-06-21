using System;

namespace IdleGuildDemo.Systems
{
    [Serializable]
    public sealed class QuestClaimResult
    {
        public bool success;
        public string questId = string.Empty;
        public string nextQuestId = string.Empty;
        public int xpReward;
        public int coinsReward;
        public string rewardItemId = string.Empty;
        public int rewardItemQuantity;
        public bool unlockedSecondCharacter;
        public string failureReason = string.Empty;
    }
}
