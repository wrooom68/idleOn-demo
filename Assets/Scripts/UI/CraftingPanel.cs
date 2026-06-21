using System.Collections.Generic;
using System.Text;
using IdleGuildDemo.Core;
using IdleGuildDemo.Data;
using IdleGuildDemo.Runtime;
using IdleGuildDemo.Systems;
using UnityEngine;
using UnityEngine.UI;

namespace IdleGuildDemo.UI
{
  public class CraftingPanel : UIPanel
  {
    [SerializeField] private Transform recipeContainer;
    [SerializeField] private CraftingRecipeRowView recipeRowPrefab;
    [SerializeField] private Text stationLabelText;
    [SerializeField] private Text statusText;
    [SerializeField] private ToastView toastView;
    [SerializeField] private QuestDefinition[] questDefinitions;

    public Transform RecipeContainer => recipeContainer;
    public CraftingRecipeRowView RecipeRowPrefab => recipeRowPrefab;

    public event System.Action<CraftingResult> CraftCompleted;

    private readonly List<CraftingRecipeRowView> _rows = new List<CraftingRecipeRowView>();
    private IReadOnlyList<RecipeDefinition> _recipes;
    private IReadOnlyList<ItemDefinition> _items;
    private CraftingSystem _craftingSystem;

    public void Bind(
      CraftingSystem craftingSystem,
      IReadOnlyList<RecipeDefinition> recipes,
      IReadOnlyList<ItemDefinition> items = null)
    {
      _craftingSystem = craftingSystem;
      _recipes = recipes;
      _items = items;
      Refresh();
    }

    public void SetQuestDefinitions(QuestDefinition[] quests)
    {
      questDefinitions = quests;
    }

    public void Refresh()
    {
      if (_craftingSystem == null || _recipes == null)
      {
        ClearRows();
        SetStatus("Crafting unavailable.");
        return;
      }

      EnsureRowCount(_recipes.Count);
      for (int i = 0; i < _rows.Count; i++)
      {
        CraftingRecipeRowView row = _rows[i];
        if (row == null)
        {
          continue;
        }

        if (i >= _recipes.Count)
        {
          row.SetRecipeId(string.Empty);
          row.SetRecipe(string.Empty, string.Empty, null, false);
          continue;
        }

        RecipeDefinition recipe = _recipes[i];
        ItemDefinition outputItem = FindItem(_items, recipe != null ? recipe.OutputItemId : string.Empty);
        string recipeName = recipe != null && !string.IsNullOrEmpty(recipe.DisplayName)
          ? recipe.DisplayName
          : FormatItemName(recipe != null ? recipe.OutputItemId : string.Empty);
        row.SetRecipeId(recipe != null ? recipe.Id : string.Empty);
        row.SetRecipe(
          recipeName,
          FormatRecipeMaterials(recipe, _craftingSystem.GetMissingIngredients(recipe)),
          outputItem != null ? outputItem.Icon : null,
          _craftingSystem.CanCraft(recipe));
        row.BindCraft(recipeId =>
        {
          CraftRecipeById(recipeId);
        });
      }

      SetStatus("Select a recipe to craft.");
    }

    public CraftingResult CraftRecipeById(string recipeId)
    {
      RecipeDefinition recipe = FindRecipe(_recipes, recipeId);
      return CraftRecipe(recipe);
    }

    public CraftingResult CraftRecipe(RecipeDefinition recipe)
    {
      CraftingResult result = _craftingSystem != null
        ? _craftingSystem.Craft(recipe)
        : new CraftingResult { failureReason = "Crafting unavailable." };

      Refresh();

      if (result.success)
      {
        QuestUpdateResult questResult = ReportCraftedQuestProgress(result);
        string itemName = FormatItemName(result.outputItemId);
        SetStatus(questResult != null && questResult.completed
          ? $"Crafted {itemName} x{result.outputQuantity}. Quest complete."
          : $"Crafted {itemName} x{result.outputQuantity}.");
        toastView?.ShowItemCrafted(itemName);
      }
      else
      {
        SetStatus(FormatCraftFailure(result));
      }

      CraftCompleted?.Invoke(result);
      return result;
    }

    private QuestUpdateResult ReportCraftedQuestProgress(CraftingResult result)
    {
      if (result == null || !result.success)
      {
        return null;
      }

      if (!TryGetServices(out ServiceRegistry services))
      {
        return null;
      }

      QuestUpdateResult questResult = services.QuestSystem.ReportItemCrafted(
        result.outputItemId,
        result.outputQuantity,
        questDefinitions);
      services.SaveSystem.Save(services.SaveData);
      return questResult;
    }

    public void SetStationLabel(string label)
    {
      if (stationLabelText != null)
      {
        stationLabelText.text = label;
      }
    }

    public void SetStatus(string status)
    {
      if (statusText != null)
      {
        statusText.text = status ?? string.Empty;
      }
    }

    public void ClearRows()
    {
      for (int i = 0; i < _rows.Count; i++)
      {
        if (_rows[i] != null)
        {
          _rows[i].SetRecipe(string.Empty, string.Empty, null, false);
        }
      }
    }

    private void EnsureRowCount(int count)
    {
      if (recipeContainer == null || recipeRowPrefab == null)
      {
        return;
      }

      while (_rows.Count < count)
      {
        _rows.Add(Instantiate(recipeRowPrefab, recipeContainer));
      }
    }

    private static string FormatRecipeMaterials(RecipeDefinition recipe, List<InventoryStack> missingItems)
    {
      if (recipe == null)
      {
        return "Recipe missing.";
      }

      StringBuilder builder = new StringBuilder();
      builder.Append($"Needs: {FormatIngredients(recipe)}");
      builder.Append($"\nMakes: {FormatItemName(recipe.OutputItemId)} x{recipe.OutputQuantity}");
      if (missingItems != null && missingItems.Count > 0)
      {
        builder.Append($"\nMissing: {FormatStacks(missingItems)}");
      }
      else
      {
        builder.Append("\nReady to craft.");
      }

      return builder.ToString();
    }

    private static string FormatIngredients(RecipeDefinition recipe)
    {
      if (recipe?.Ingredients == null)
      {
        return "none";
      }

      StringBuilder builder = new StringBuilder();
      foreach (RecipeIngredient ingredient in recipe.Ingredients)
      {
        if (ingredient == null || ingredient.quantity <= 0 || string.IsNullOrEmpty(ingredient.itemId))
        {
          continue;
        }

        if (builder.Length > 0)
        {
          builder.Append(", ");
        }

        builder.Append($"{FormatItemName(ingredient.itemId)} x{ingredient.quantity}");
      }

      return builder.Length == 0 ? "none" : builder.ToString();
    }

    private static string FormatCraftFailure(CraftingResult result)
    {
      if (result == null)
      {
        return "Crafting failed.";
      }

      if (result.missingItems != null && result.missingItems.Count > 0)
      {
        return $"Missing: {FormatStacks(result.missingItems)}";
      }

      return string.IsNullOrEmpty(result.failureReason) ? "Crafting failed." : result.failureReason;
    }

    private static string FormatStacks(List<InventoryStack> stacks)
    {
      if (stacks == null || stacks.Count == 0)
      {
        return "none";
      }

      StringBuilder builder = new StringBuilder();
      foreach (InventoryStack stack in stacks)
      {
        if (stack == null || stack.quantity <= 0)
        {
          continue;
        }

        if (builder.Length > 0)
        {
          builder.Append(", ");
        }

        builder.Append($"{FormatItemName(stack.itemId)} x{stack.quantity}");
      }

      return builder.Length == 0 ? "none" : builder.ToString();
    }

    private static RecipeDefinition FindRecipe(IReadOnlyList<RecipeDefinition> recipes, string recipeId)
    {
      if (recipes == null || string.IsNullOrEmpty(recipeId))
      {
        return null;
      }

      foreach (RecipeDefinition recipe in recipes)
      {
        if (recipe != null && recipe.Id == recipeId)
        {
          return recipe;
        }
      }

      return null;
    }

    private static ItemDefinition FindItem(IReadOnlyList<ItemDefinition> items, string itemId)
    {
      if (items == null || string.IsNullOrEmpty(itemId))
      {
        return null;
      }

      foreach (ItemDefinition item in items)
      {
        if (item != null && item.Id == itemId)
        {
          return item;
        }
      }

      return null;
    }

    private static string FormatItemName(string itemId)
    {
      switch (itemId)
      {
        case GameConstants.ItemCopperOreId:
          return "Copper Ore";
        case GameConstants.ItemCopperBarId:
          return "Copper Bar";
        case GameConstants.ItemSlimeGooId:
          return "Slime Goo";
        case GameConstants.ItemCopperSwordId:
          return "Copper Sword";
        case GameConstants.ItemCopperPickaxeId:
          return "Copper Pickaxe";
        default:
          return string.IsNullOrEmpty(itemId) ? "Item" : itemId;
      }
    }

    private static bool TryGetServices(out ServiceRegistry services)
    {
      GameBootstrap.EnsureInitialized();
      services = ServiceRegistry.Instance;
      return services.IsInitialized;
    }
  }
}
