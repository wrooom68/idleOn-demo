using UnityEngine;

namespace IdleGuildDemo.Data
{
    [CreateAssetMenu(menuName = "Idle Guild/Data/Enemy Definition", fileName = "EnemyDefinition")]
    public sealed class EnemyDefinition : ScriptableObject
    {
        [SerializeField] private string id;
        [SerializeField] private string displayName;
        [SerializeField] private int maxHp = 1;
        [SerializeField] private int xpReward;

        public string Id => id;
        public string DisplayName => displayName;
        public int MaxHp => maxHp;
        public int XpReward => xpReward;
    }
}
