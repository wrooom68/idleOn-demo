using System;

namespace IdleGuildDemo.Runtime
{
    [Serializable]
    public sealed class InventoryStack
    {
        public string itemId = string.Empty;
        public int quantity;

        public InventoryStack()
        {
        }

        public InventoryStack(string itemId, int quantity)
        {
            this.itemId = itemId ?? string.Empty;
            this.quantity = Math.Max(0, quantity);
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(itemId) && quantity > 0;
        }

        public void Normalize()
        {
            if (itemId == null)
            {
                itemId = string.Empty;
            }

            if (quantity < 0)
            {
                quantity = 0;
            }
        }
    }
}
