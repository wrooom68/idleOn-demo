using IdleGuildDemo.Runtime;
using IdleGuildDemo.Save;
using IdleGuildDemo.Systems;

namespace IdleGuildDemo.Core
{
    /// <summary>
    /// Central runtime access point for backend systems created by GameBootstrap.
    /// </summary>
    public sealed class ServiceRegistry
    {
        public static ServiceRegistry Instance { get; } = new ServiceRegistry();

        public bool IsInitialized { get; private set; }
        public SaveSystem SaveSystem { get; private set; }
        public SaveData SaveData { get; private set; }
        public PlayerProfile PlayerProfile { get; private set; }
        public InventorySystem InventorySystem { get; private set; }
        public ProgressionSystem ProgressionSystem { get; private set; }
        public StatsSystem StatsSystem { get; private set; }
        public CharacterRosterSystem CharacterRosterSystem { get; private set; }
        public CombatSystem CombatSystem { get; private set; }
        public GatheringSystem GatheringSystem { get; private set; }
        public CraftingSystem CraftingSystem { get; private set; }
        public QuestSystem QuestSystem { get; private set; }
        public OfflineProgressionSystem OfflineProgressionSystem { get; private set; }

        private ServiceRegistry()
        {
        }

        public void Initialize(
            SaveSystem saveSystem,
            SaveData saveData,
            PlayerProfile playerProfile,
            InventorySystem inventorySystem,
            ProgressionSystem progressionSystem,
            StatsSystem statsSystem,
            CharacterRosterSystem characterRosterSystem,
            CombatSystem combatSystem,
            GatheringSystem gatheringSystem,
            CraftingSystem craftingSystem,
            QuestSystem questSystem,
            OfflineProgressionSystem offlineProgressionSystem)
        {
            SaveSystem = saveSystem;
            SaveData = saveData;
            PlayerProfile = playerProfile;
            InventorySystem = inventorySystem;
            ProgressionSystem = progressionSystem;
            StatsSystem = statsSystem;
            CharacterRosterSystem = characterRosterSystem;
            CombatSystem = combatSystem;
            GatheringSystem = gatheringSystem;
            CraftingSystem = craftingSystem;
            QuestSystem = questSystem;
            OfflineProgressionSystem = offlineProgressionSystem;
            IsInitialized = saveSystem != null
                && saveData != null
                && playerProfile != null
                && inventorySystem != null
                && progressionSystem != null
                && statsSystem != null
                && characterRosterSystem != null
                && combatSystem != null
                && gatheringSystem != null
                && craftingSystem != null
                && questSystem != null
                && offlineProgressionSystem != null;
        }

        public void Clear()
        {
            SaveSystem = null;
            SaveData = null;
            PlayerProfile = null;
            InventorySystem = null;
            ProgressionSystem = null;
            StatsSystem = null;
            CharacterRosterSystem = null;
            CombatSystem = null;
            GatheringSystem = null;
            CraftingSystem = null;
            QuestSystem = null;
            OfflineProgressionSystem = null;
            IsInitialized = false;
        }
    }
}
