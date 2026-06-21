using UnityEngine;

namespace IdleGuildDemo.Data
{
    [CreateAssetMenu(menuName = "Idle Guild/Data/Class Definition", fileName = "ClassDefinition")]
    public sealed class ClassDefinition : ScriptableObject
    {
        [SerializeField] private string id;
        [SerializeField] private string displayName;
        [SerializeField] private string description;

        public string Id => id;
        public string DisplayName => displayName;
        public string Description => description;
    }
}
