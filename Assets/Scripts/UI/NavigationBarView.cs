using UnityEngine;
using UnityEngine.UI;

namespace IdleGuildDemo.UI
{
  public class NavigationBarView : MonoBehaviour
  {
    [SerializeField] private Button inventoryButton;
    [SerializeField] private Button craftingButton;
    [SerializeField] private Button charactersButton;
    [SerializeField] private Button talentsButton;
    [SerializeField] private Button simulateAfkButton;
    [SerializeField] private Text activeViewText;

    public Button InventoryButton => inventoryButton;
    public Button CraftingButton => craftingButton;
    public Button CharactersButton => charactersButton;
    public Button TalentsButton => talentsButton;
    public Button SimulateAfkButton => simulateAfkButton;

    private UIRootController _rootController;
    private TownHUDView _townHudView;

    public void Bind(UIRootController rootController, TownHUDView townHudView)
    {
      Unbind();
      _rootController = rootController;
      _townHudView = townHudView;

      inventoryButton?.onClick.AddListener(ShowInventory);
      craftingButton?.onClick.AddListener(ShowCrafting);
      charactersButton?.onClick.AddListener(ShowCharacters);
      talentsButton?.onClick.AddListener(ShowTalents);
      simulateAfkButton?.onClick.AddListener(SimulateAfk);
      ApplyDefaultLabels();
    }

    public void Unbind()
    {
      inventoryButton?.onClick.RemoveListener(ShowInventory);
      craftingButton?.onClick.RemoveListener(ShowCrafting);
      charactersButton?.onClick.RemoveListener(ShowCharacters);
      talentsButton?.onClick.RemoveListener(ShowTalents);
      simulateAfkButton?.onClick.RemoveListener(SimulateAfk);
    }

    public void SetActiveView(string label)
    {
      if (activeViewText != null)
      {
        activeViewText.text = label ?? string.Empty;
      }
    }

    public void ApplyDefaultLabels()
    {
      SetButtonText(inventoryButton, "Inventory");
      SetButtonText(craftingButton, "Crafting");
      SetButtonText(charactersButton, "Characters");
      SetButtonText(talentsButton, "Talents");
      SetButtonText(simulateAfkButton, "Simulate 2h AFK");
    }

    private void ShowInventory()
    {
      _townHudView?.OpenInventory();
      _rootController?.ShowInventory();
      SetActiveView("Inventory");
    }

    private void ShowCrafting()
    {
      _townHudView?.OpenCrafting();
      _rootController?.ShowCrafting();
      SetActiveView("Crafting");
    }

    private void ShowCharacters()
    {
      _townHudView?.OpenCharacterPanel();
      _rootController?.ShowCharacters();
      SetActiveView("Characters");
    }

    private void ShowTalents()
    {
      _townHudView?.OpenCharacterProgression();
      _rootController?.ShowTalents();
      SetActiveView("Talents");
    }

    private void SimulateAfk()
    {
      _townHudView?.SimulateAfkTwoHours();
      _rootController?.ShowAfkResults();
      SetActiveView("AFK Results");
    }

    private static void SetButtonText(Button button, string label)
    {
      if (button == null)
      {
        return;
      }

      Text text = button.GetComponentInChildren<Text>();
      if (text != null)
      {
        text.text = label;
      }
    }
  }
}
