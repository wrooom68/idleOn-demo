using UnityEngine;
using UnityEngine.UI;

namespace IdleGuildDemo.UI
{
    /// <summary>
    /// Thin placeholder view for the town hub HUD.
    /// </summary>
    public sealed class TownHUDView : MonoBehaviour
    {
        [SerializeField] private Button inventoryButton;
        [SerializeField] private Button combatButton;
        [SerializeField] private Button miningButton;
        [SerializeField] private Button simulateAfkButton;

        // TODO: Render character, quest, and inventory state from systems.
        // TODO: Call public system/navigation methods from button handlers later.
    }
}
