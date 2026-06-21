using UnityEngine;

namespace IdleGuildDemo.Data
{
    public enum QuestObjectiveType
    {
        KillEnemy,
        CollectItem,
        CraftItem,
        ReachLevel,
        ChooseClass,
        UnlockCharacter
    }

    [CreateAssetMenu(menuName = "Idle Guild/Data/Quest Definition", fileName = "QuestDefinition")]
    public sealed class QuestDefinition : ScriptableObject
    {
        [SerializeField] private string id;
        [SerializeField] private string displayName;
        [SerializeField] private string description;
        [SerializeField] private QuestObjectiveType objectiveType;
        [SerializeField] private string targetId;
        [SerializeField] private int requiredAmount = 1;
        [SerializeField] private string nextQuestId;
        [SerializeField] private int rewardXp;
        [SerializeField] private int rewardCoins;
        [SerializeField] private string rewardItemId;
        [SerializeField] private int rewardItemQuantity;
        [SerializeField] private bool unlocksSecondCharacter;

        public string Id => id;
        public string DisplayName => displayName;
        public string Description => description;
        public QuestObjectiveType ObjectiveType => objectiveType;
        public string TargetId => targetId;
        public int RequiredAmount => requiredAmount;
        public string NextQuestId => nextQuestId;
        public int RewardXp => rewardXp;
        public int RewardCoins => rewardCoins;
        public string RewardItemId => rewardItemId;
        public int RewardItemQuantity => rewardItemQuantity;
        public bool UnlocksSecondCharacter => unlocksSecondCharacter;
    }
}
