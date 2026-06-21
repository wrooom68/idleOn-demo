using UnityEngine;

namespace IdleGuildDemo.Data
{
    [CreateAssetMenu(menuName = "Idle Guild/Data/Quest Definition", fileName = "QuestDefinition")]
    public sealed class QuestDefinition : ScriptableObject
    {
        [SerializeField] private string id;
        [SerializeField] private string displayName;
        [SerializeField] private string description;

        public string Id => id;
        public string DisplayName => displayName;
        public string Description => description;
    }
}
