using IdleGuildDemo.Runtime;

namespace IdleGuildDemo.Systems
{
    /// <summary>
    /// Owns future recipe validation and crafting item mutations.
    /// </summary>
    public sealed class CraftingSystem
    {
        public bool CanCraft(InventoryState inventory, string recipeId)
        {
            // TODO: Implement recipe checks from recipe definitions.
            return false;
        }

        public void Craft(InventoryState inventory, string recipeId)
        {
            // TODO: Implement crafting after recipe and inventory rules are defined.
        }
    }
}
