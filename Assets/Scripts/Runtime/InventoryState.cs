using System;
using System.Collections.Generic;

namespace IdleGuildDemo.Runtime
{
    [Serializable]
    public sealed class InventoryState
    {
        public List<InventoryStack> stacks = new List<InventoryStack>();
    }
}
