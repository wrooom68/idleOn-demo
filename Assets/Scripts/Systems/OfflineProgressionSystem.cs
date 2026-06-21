using System.Collections.Generic;
using IdleGuildDemo.Runtime;

namespace IdleGuildDemo.Systems
{
    /// <summary>
    /// Owns future offline/AFK reward calculations from saved timestamps and character tasks.
    /// </summary>
    public sealed class OfflineProgressionSystem
    {
        public List<InventoryStack> CalculateRewards(SaveData saveData, long currentUnixTime)
        {
            // TODO: Calculate capped offline rewards without applying them directly.
            return new List<InventoryStack>();
        }
    }
}
