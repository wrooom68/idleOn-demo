using System.Collections.Generic;
using IdleGuildDemo.Runtime;

namespace IdleGuildDemo.Systems
{
    /// <summary>
    /// Owns stackable inventory reads and item mutation rules.
    /// </summary>
    public sealed class InventorySystem
    {
        private readonly InventoryState state;

        public InventorySystem(InventoryState state)
        {
            this.state = state ?? new InventoryState();
            this.state.Normalize();
        }

        public int GetQuantity(string itemId)
        {
            if (string.IsNullOrEmpty(itemId))
            {
                return 0;
            }

            state.Normalize();

            for (var i = 0; i < state.stacks.Count; i++)
            {
                var stack = state.stacks[i];
                if (stack.itemId == itemId)
                {
                    return stack.quantity;
                }
            }

            return 0;
        }

        public bool HasItem(string itemId, int quantity)
        {
            if (string.IsNullOrEmpty(itemId) || quantity <= 0)
            {
                return false;
            }

            return GetQuantity(itemId) >= quantity;
        }

        public bool HasItems(IEnumerable<InventoryStack> costs)
        {
            if (costs == null)
            {
                return true;
            }

            foreach (var cost in costs)
            {
                if (cost == null || !HasItem(cost.itemId, cost.quantity))
                {
                    return false;
                }
            }

            return true;
        }

        public bool AddItem(string itemId, int quantity)
        {
            if (string.IsNullOrEmpty(itemId) || quantity <= 0)
            {
                return false;
            }

            state.Normalize();

            for (var i = 0; i < state.stacks.Count; i++)
            {
                var stack = state.stacks[i];
                if (stack.itemId == itemId)
                {
                    stack.quantity += quantity;
                    return true;
                }
            }

            state.stacks.Add(new InventoryStack(itemId, quantity));
            return true;
        }

        public bool RemoveItem(string itemId, int quantity)
        {
            if (string.IsNullOrEmpty(itemId) || quantity <= 0)
            {
                return false;
            }

            state.Normalize();

            for (var i = 0; i < state.stacks.Count; i++)
            {
                var stack = state.stacks[i];
                if (stack.itemId != itemId)
                {
                    continue;
                }

                if (stack.quantity < quantity)
                {
                    return false;
                }

                stack.quantity -= quantity;
                if (stack.quantity <= 0)
                {
                    state.stacks.RemoveAt(i);
                }

                return true;
            }

            return false;
        }

        public bool TryRemoveItems(IEnumerable<InventoryStack> costs)
        {
            if (!HasItems(costs))
            {
                return false;
            }

            if (costs == null)
            {
                return true;
            }

            foreach (var cost in costs)
            {
                RemoveItem(cost.itemId, cost.quantity);
            }

            state.Normalize();
            return true;
        }

        public IReadOnlyList<InventoryStack> GetStacks()
        {
            state.Normalize();
            return state.stacks;
        }

        public void Clear()
        {
            state.EnsureList();
            state.stacks.Clear();
        }
    }
}
