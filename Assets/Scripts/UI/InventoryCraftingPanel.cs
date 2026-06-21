using UnityEngine;
using UnityEngine.UI;

namespace IdleGuildDemo.UI
{
    /// <summary>
    /// Thin placeholder view for the combined inventory and crafting panel.
    /// </summary>
    public sealed class InventoryCraftingPanel : MonoBehaviour
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private Transform inventorySlotRoot;
        [SerializeField] private Transform recipeRowRoot;

        // TODO: Render inventory and recipe state from InventorySystem and CraftingSystem.
        // TODO: Do not calculate recipe validity in this view.
    }
}
