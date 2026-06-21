using UnityEngine;

namespace IdleGuildDemo.Data
{
    [CreateAssetMenu(menuName = "Idle Guild/Data/Class Definition", fileName = "ClassDefinition")]
    public sealed class ClassDefinition : ScriptableObject
    {
        [SerializeField] private string id;
        [SerializeField] private string displayName;
        [SerializeField] private string description;
        [SerializeField] private int damageBonus;
        [SerializeField] private float dropRateMultiplierBonus;
        [SerializeField] private float afkGainMultiplierBonus;

        public string Id => id;
        public string DisplayName => displayName;
        public string Description => description;
        public int DamageBonus => damageBonus;
        public float DropRateMultiplierBonus => dropRateMultiplierBonus;
        public float AfkGainMultiplierBonus => afkGainMultiplierBonus;
    }
}
