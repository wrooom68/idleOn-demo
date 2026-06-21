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
    /// <summary>
    /// Thin panel that renders inventory and delegates crafting rules to CraftingSystem.
    /// </summary>
    public sealed class InventoryCraftingPanel : MonoBehaviour
    {
        [SerializeField] private GameObject root;
        [SerializeField] private Button closeButton;
        [SerializeField] private Text inventoryText;
        [SerializeField] private Text copperBarRecipeText;
        [SerializeField] private Text copperSwordRecipeText;
        [SerializeField] private Text copperPickaxeRecipeText;
        [SerializeField] private Button craftCopperBarButton;
        [SerializeField] private Button craftCopperSwordButton;
        [SerializeField] private Button craftCopperPickaxeButton;
        [SerializeField] private Text statusText;
        [SerializeField] private ToastView toastView;
        [SerializeField] private TownHUDView townHudView;
        [SerializeField] private RecipeDefinition copperBarRecipe;
        [SerializeField] private RecipeDefinition copperSwordRecipe;
        [SerializeField] private RecipeDefinition copperPickaxeRecipe;
        [SerializeField] private QuestDefinition[] questDefinitions;

        private void OnEnable()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (closeButton != null)
            {
                closeButton.onClick.AddListener(Hide);
            }

            if (craftCopperBarButton != null)
            {
                craftCopperBarButton.onClick.AddListener(CraftCopperBar);
            }

            if (craftCopperSwordButton != null)
            {
                craftCopperSwordButton.onClick.AddListener(CraftCopperSword);
            }

            if (craftCopperPickaxeButton != null)
            {
                craftCopperPickaxeButton.onClick.AddListener(CraftCopperPickaxe);
            }
        }

        private void OnDisable()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (closeButton != null)
            {
                closeButton.onClick.RemoveListener(Hide);
            }

            if (craftCopperBarButton != null)
            {
                craftCopperBarButton.onClick.RemoveListener(CraftCopperBar);
            }

            if (craftCopperSwordButton != null)
            {
                craftCopperSwordButton.onClick.RemoveListener(CraftCopperSword);
            }

            if (craftCopperPickaxeButton != null)
            {
                craftCopperPickaxeButton.onClick.RemoveListener(CraftCopperPickaxe);
            }
        }

        public void Show()
        {
            SetVisible(true);
            Refresh();
        }

        public void Hide()
        {
            SetVisible(false);
        }

        public void Refresh()
        {
            if (!TryGetServices(out ServiceRegistry services))
            {
                SetStatus("Runtime is not ready.");
                return;
            }

            SetText(inventoryText, FormatInventory(services));
            RefreshRecipe(services, copperBarRecipe, copperBarRecipeText, craftCopperBarButton);
            RefreshRecipe(services, copperSwordRecipe, copperSwordRecipeText, craftCopperSwordButton);
            RefreshRecipe(services, copperPickaxeRecipe, copperPickaxeRecipeText, craftCopperPickaxeButton);
        }

        public void CraftCopperBar()
        {
            CraftRecipe(copperBarRecipe);
        }

        public void CraftCopperSword()
        {
            CraftRecipe(copperSwordRecipe);
        }

        public void CraftCopperPickaxe()
        {
            CraftRecipe(copperPickaxeRecipe);
        }

        private void CraftRecipe(RecipeDefinition recipe)
        {
            if (!TryGetServices(out ServiceRegistry services))
            {
                SetStatus("Runtime is not ready.");
                return;
            }

            CraftingResult result = services.CraftingSystem.Craft(recipe);
            if (!result.success)
            {
                SetStatus(FormatCraftFailure(result));
                Refresh();
                return;
            }

            QuestUpdateResult questResult = services.QuestSystem.ReportItemCrafted(
                result.outputItemId,
                result.outputQuantity,
                questDefinitions);

            services.SaveSystem.Save(services.SaveData);
            string message = $"Crafted {FormatItemName(result.outputItemId)} x{result.outputQuantity}.";
            if (questResult.completed)
            {
                message += " Quest complete.";
            }

            SetStatus(message);
            toastView?.Show(message);
            Refresh();
            townHudView?.Refresh();
        }

        private void RefreshRecipe(ServiceRegistry services, RecipeDefinition recipe, Text text, Button button)
        {
            string recipeText = FormatRecipe(recipe);
            List<InventoryStack> missing = services.CraftingSystem.GetMissingIngredients(recipe);
            if (missing.Count > 0)
            {
                recipeText += $"\nMissing: {FormatStacks(missing)}";
            }
            else if (recipe != null)
            {
                recipeText += "\nReady to craft.";
            }

            SetText(text, recipeText);
            if (button != null)
            {
                button.interactable = services.CraftingSystem.CanCraft(recipe);
            }
        }

        private static string FormatInventory(ServiceRegistry services)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"Coins: {services.PlayerProfile.coins}");
            builder.AppendLine($"Copper Ore: {services.InventorySystem.GetQuantity(GameConstants.ItemCopperOreId)}");
            builder.AppendLine($"Copper Bar: {services.InventorySystem.GetQuantity(GameConstants.ItemCopperBarId)}");
            builder.AppendLine($"Slime Goo: {services.InventorySystem.GetQuantity(GameConstants.ItemSlimeGooId)}");
            builder.AppendLine($"Copper Sword: {services.InventorySystem.GetQuantity(GameConstants.ItemCopperSwordId)}");
            builder.AppendLine($"Copper Pickaxe: {services.InventorySystem.GetQuantity(GameConstants.ItemCopperPickaxeId)}");
            return builder.ToString().TrimEnd();
        }

        private static string FormatRecipe(RecipeDefinition recipe)
        {
            if (recipe == null)
            {
                return "Recipe missing.";
            }

            return $"{recipe.DisplayName}\nNeeds: {FormatIngredients(recipe)}\nMakes: {FormatItemName(recipe.OutputItemId)} x{recipe.OutputQuantity}";
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

        private void SetVisible(bool visible)
        {
            if (root != null)
            {
                root.SetActive(visible);
            }
            else
            {
                gameObject.SetActive(visible);
            }
        }

        private void SetStatus(string message)
        {
            SetText(statusText, message);
            if (!string.IsNullOrEmpty(message))
            {
                toastView?.Show(message);
            }
        }

        private static void SetText(Text text, string value)
        {
            if (text != null)
            {
                text.text = value ?? string.Empty;
            }
        }

        private static bool TryGetServices(out ServiceRegistry services)
        {
            GameBootstrap.EnsureInitialized();
            services = ServiceRegistry.Instance;
            if (!services.IsInitialized)
            {
                Debug.LogError("ServiceRegistry is not initialized. Add GameBootstrap to the scene.");
            }

            return services.IsInitialized;
        }
    }
}
