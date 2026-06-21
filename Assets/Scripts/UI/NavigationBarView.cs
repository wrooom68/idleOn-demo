using UnityEngine;
using UnityEngine.UI;

namespace IdleGuild.UI
{
  public class NavigationBarView : MonoBehaviour
  {
    [SerializeField] private Button inventoryButton;
    [SerializeField] private Button craftingButton;
    [SerializeField] private Button charactersButton;
    [SerializeField] private Button talentsButton;
    [SerializeField] private Button simulateAfkButton;

    public Button InventoryButton => inventoryButton;
    public Button CraftingButton => craftingButton;
    public Button CharactersButton => charactersButton;
    public Button TalentsButton => talentsButton;
    public Button SimulateAfkButton => simulateAfkButton;
  }
}
