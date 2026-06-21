using UnityEngine;
using UnityEngine.UI;

namespace IdleGuild.UI
{
  public class CraftingPanel : UIPanel
  {
    [SerializeField] private Transform recipeContainer;
    [SerializeField] private CraftingRecipeRowView recipeRowPrefab;
    [SerializeField] private Text stationLabelText;

    public Transform RecipeContainer => recipeContainer;
    public CraftingRecipeRowView RecipeRowPrefab => recipeRowPrefab;

    public void SetStationLabel(string label)
    {
      if (stationLabelText != null)
      {
        stationLabelText.text = label;
      }
    }
  }
}
