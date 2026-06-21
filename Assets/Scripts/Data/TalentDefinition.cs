using UnityEngine;

namespace IdleGuildDemo.Data
{
    [CreateAssetMenu(menuName = "Idle Guild/Data/Talent Definition", fileName = "TalentDefinition")]
    public sealed class TalentDefinition : ScriptableObject
    {
        [SerializeField] private string id;
        [SerializeField] private string displayName;
        [SerializeField] private string description;
        [SerializeField] private int maxRank = 5;
        [SerializeField] private int damagePerRank;
        [SerializeField] private float miningSpeedPerRank;
        [SerializeField] private float xpGainPerRank;
        [SerializeField] private float afkGainPerRank;

        public string Id => id;
        public string DisplayName => displayName;
        public string Description => description;
        public int MaxRank => maxRank;
        public int DamagePerRank => damagePerRank;
        public float MiningSpeedPerRank => miningSpeedPerRank;
        public float XpGainPerRank => xpGainPerRank;
        public float AfkGainPerRank => afkGainPerRank;
    }
}
