using UnityEngine;

namespace IdleGuild.UI
{
  /// Wires navigation-bar button clicks to UIRootController at runtime.
  public class UIWiring : MonoBehaviour
  {
    [SerializeField] private UIRootController root;
    [SerializeField] private NavigationBarView navigationBar;
    [SerializeField] private AfkResultsModal afkResultsModal;

    private void Awake()
    {
      if (root == null || navigationBar == null)
      {
        return;
      }

      if (navigationBar.InventoryButton != null)
      {
        navigationBar.InventoryButton.onClick.AddListener(root.ShowInventory);
      }

      if (navigationBar.CraftingButton != null)
      {
        navigationBar.CraftingButton.onClick.AddListener(root.ShowCrafting);
      }

      if (navigationBar.CharactersButton != null)
      {
        navigationBar.CharactersButton.onClick.AddListener(root.ShowCharacters);
      }

      if (navigationBar.TalentsButton != null)
      {
        navigationBar.TalentsButton.onClick.AddListener(root.ShowTalents);
      }

      if (navigationBar.SimulateAfkButton != null)
      {
        navigationBar.SimulateAfkButton.onClick.AddListener(root.ShowAfkResults);
      }

      if (afkResultsModal != null && afkResultsModal.CloseButton != null)
      {
        afkResultsModal.CloseButton.onClick.AddListener(afkResultsModal.Hide);
      }
    }
  }
}
