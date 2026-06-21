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
            this.itemId = itemId;
            this.quantity = quantity;
        }
    }
}
