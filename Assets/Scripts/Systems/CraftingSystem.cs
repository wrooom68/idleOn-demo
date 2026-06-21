using System.Collections.Generic;
using IdleGuildDemo.Data;
using IdleGuildDemo.Runtime;

namespace IdleGuildDemo.Systems
{
    /// <summary>
    /// Owns future recipe validation and crafting item mutations.
    /// </summary>
    public sealed class CraftingSystem
    {
        private readonly InventorySystem inventorySystem;

        public CraftingSystem(InventorySystem inventorySystem)
        {
            this.inventorySystem = inventorySystem;
        }

        public bool CanCraft(RecipeDefinition recipe)
        {
            return IsRecipeValid(recipe) && GetMissingIngredients(recipe).Count == 0;
        }

        public List<InventoryStack> GetMissingIngredients(RecipeDefinition recipe)
        {
            List<InventoryStack> missingItems = new List<InventoryStack>();
            if (!IsRecipeValid(recipe))
            {
                return missingItems;
            }

            List<InventoryStack> requiredIngredients = GetRequiredIngredients(recipe);
            foreach (InventoryStack ingredient in requiredIngredients)
            {
                int available = inventorySystem?.GetQuantity(ingredient.itemId) ?? 0;
                if (available < ingredient.quantity)
                {
                    missingItems.Add(new InventoryStack(ingredient.itemId, ingredient.quantity - available));
                }
            }

            return missingItems;
        }

        public CraftingResult Craft(RecipeDefinition recipe)
        {
            CraftingResult result = new CraftingResult();

            if (recipe == null)
            {
                result.failureReason = "Recipe is missing.";
                return result;
            }

            result.recipeId = recipe.Id ?? string.Empty;
            result.outputItemId = recipe.OutputItemId ?? string.Empty;
            result.outputQuantity = recipe.OutputQuantity;

            if (inventorySystem == null)
            {
                result.failureReason = "Inventory system is missing.";
                return result;
            }

            if (!IsRecipeValid(recipe))
            {
                result.failureReason = "Recipe output or ingredients are invalid.";
                return result;
            }

            result.missingItems = GetMissingIngredients(recipe);
            if (result.missingItems.Count > 0)
            {
                result.failureReason = "Missing ingredients.";
                return result;
            }

            List<InventoryStack> requiredIngredients = GetRequiredIngredients(recipe);
            if (!inventorySystem.TryRemoveItems(requiredIngredients))
            {
                result.failureReason = "Failed to consume ingredients.";
                return result;
            }

            if (!inventorySystem.AddItem(recipe.OutputItemId, recipe.OutputQuantity))
            {
                result.failureReason = "Failed to add crafted item.";
                return result;
            }

            result.success = true;
            result.consumedItems = CopyStacks(requiredIngredients);
            return result;
        }

        private static bool IsRecipeValid(RecipeDefinition recipe)
        {
            return recipe != null
                && !string.IsNullOrEmpty(recipe.OutputItemId)
                && recipe.OutputQuantity > 0
                && !HasInvalidPositiveIngredient(recipe);
        }

        private static bool HasInvalidPositiveIngredient(RecipeDefinition recipe)
        {
            if (recipe?.Ingredients == null)
            {
                return false;
            }

            foreach (RecipeIngredient ingredient in recipe.Ingredients)
            {
                if (ingredient != null && ingredient.quantity > 0 && string.IsNullOrEmpty(ingredient.itemId))
                {
                    return true;
                }
            }

            return false;
        }

        private static List<InventoryStack> GetRequiredIngredients(RecipeDefinition recipe)
        {
            List<InventoryStack> requiredIngredients = new List<InventoryStack>();
            if (recipe?.Ingredients == null)
            {
                return requiredIngredients;
            }

            foreach (RecipeIngredient ingredient in recipe.Ingredients)
            {
                // Non-positive quantities are ignored so malformed optional rows do not create impossible costs.
                if (ingredient == null || string.IsNullOrEmpty(ingredient.itemId) || ingredient.quantity <= 0)
                {
                    continue;
                }

                InventoryStack existing = FindStack(requiredIngredients, ingredient.itemId);
                if (existing == null)
                {
                    requiredIngredients.Add(new InventoryStack(ingredient.itemId, ingredient.quantity));
                }
                else
                {
                    existing.quantity += ingredient.quantity;
                }
            }

            return requiredIngredients;
        }

        private static InventoryStack FindStack(List<InventoryStack> stacks, string itemId)
        {
            foreach (InventoryStack stack in stacks)
            {
                if (stack.itemId == itemId)
                {
                    return stack;
                }
            }

            return null;
        }

        private static List<InventoryStack> CopyStacks(List<InventoryStack> stacks)
        {
            List<InventoryStack> copy = new List<InventoryStack>();
            foreach (InventoryStack stack in stacks)
            {
                copy.Add(new InventoryStack(stack.itemId, stack.quantity));
            }

            return copy;
        }
    }
}
