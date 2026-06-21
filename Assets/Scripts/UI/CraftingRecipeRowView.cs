using UnityEngine;
using UnityEngine.UI;

namespace IdleGuildDemo.UI
{
  public class CraftingRecipeRowView : MonoBehaviour
  {
    [SerializeField] private Text recipeNameText;
    [SerializeField] private Text materialsText;
    [SerializeField] private Button craftButton;
    [SerializeField] private Image resultIcon;

    public Button CraftButton => craftButton;

    public void SetRecipe(string recipeName, string materialsSummary, Sprite resultSprite, bool canCraft)
    {
      if (recipeNameText != null)
      {
        recipeNameText.text = recipeName;
      }

      if (materialsText != null)
      {
        materialsText.text = materialsSummary;
      }

      if (resultIcon != null)
      {
        resultIcon.sprite = resultSprite;
        resultIcon.enabled = resultSprite != null;
      }

      if (craftButton != null)
      {
        craftButton.interactable = canCraft;
      }
    }
  }
}
