using System;
using System.Collections.Generic;

namespace IdleGuildDemo.Runtime
{
    [Serializable]
    public sealed class InventoryState
    {
        public List<InventoryStack> stacks = new List<InventoryStack>();

        public void EnsureList()
        {
            if (stacks == null)
            {
                stacks = new List<InventoryStack>();
            }
        }

        public void Normalize()
        {
            EnsureList();

            for (var i = stacks.Count - 1; i >= 0; i--)
            {
                var stack = stacks[i];
                if (stack == null)
                {
                    stacks.RemoveAt(i);
                    continue;
                }

                stack.Normalize();
                if (!stack.IsValid())
                {
                    stacks.RemoveAt(i);
                }
            }

            for (var i = 0; i < stacks.Count; i++)
            {
                var stack = stacks[i];
                for (var j = stacks.Count - 1; j > i; j--)
                {
                    var duplicate = stacks[j];
                    if (duplicate.itemId == stack.itemId)
                    {
                        stack.quantity += duplicate.quantity;
                        stacks.RemoveAt(j);
                    }
                }
            }
        }
    }
}
