using UnityEngine;
using UnityEngine.UI;

namespace IdleGuildDemo.UI
{
    /// <summary>
    /// Thin placeholder view for character stats, class choice, and talents.
    /// </summary>
    public sealed class CharacterProgressionPanel : MonoBehaviour
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private Transform classButtonRoot;
        [SerializeField] private Transform talentNodeRoot;

        // TODO: Render character/class/talent state from systems.
        // TODO: Keep stat math and progression rules outside this view.
    }
}
