using System;
using System.Collections.Generic;
using IdleGuildDemo.Runtime;

namespace IdleGuildDemo.Systems
{
    [Serializable]
    public sealed class CraftingResult
    {
        public bool success;
        public string recipeId = string.Empty;
        public string outputItemId = string.Empty;
        public int outputQuantity;
        public string failureReason = string.Empty;
        public List<InventoryStack> consumedItems = new List<InventoryStack>();
        public List<InventoryStack> missingItems = new List<InventoryStack>();
    }
}
