using IdleGuildDemo.Runtime;

namespace IdleGuildDemo.Systems
{
    /// <summary>
    /// Owns stackable inventory reads and future item mutation rules.
    /// </summary>
    public sealed class InventorySystem
    {
        public int GetQuantity(InventoryState inventory, string itemId)
        {
            // TODO: Implement inventory lookup.
            return 0;
        }

        public void AddItem(InventoryState inventory, string itemId, int quantity)
        {
            // TODO: Implement stack mutation after inventory rules are defined.
        }
    }
}
