using IdleGuildDemo.Runtime;
using IdleGuildDemo.Save;
using IdleGuildDemo.Systems;
using UnityEngine;

namespace IdleGuildDemo.Core
{
    /// <summary>
    /// Creates save data and backend systems for future scene controllers.
    /// </summary>
    public sealed class GameBootstrap : MonoBehaviour
    {
        [SerializeField] private bool dontDestroyOnLoad = true;

        public static bool EnsureInitialized()
        {
            if (ServiceRegistry.Instance.IsInitialized)
            {
                return true;
            }

            if (!Application.isPlaying)
            {
                return false;
            }

            GameBootstrap existingBootstrap = FindObjectOfType<GameBootstrap>();
            if (existingBootstrap == null)
            {
                var bootstrapObject = new GameObject("GameBootstrap");
                bootstrapObject.AddComponent<GameBootstrap>();
                return ServiceRegistry.Instance.IsInitialized;
            }

            existingBootstrap.InitializeIfNeeded();
            return ServiceRegistry.Instance.IsInitialized;
        }

        private void Awake()
        {
            if (ServiceRegistry.Instance.IsInitialized)
            {
                Destroy(gameObject);
                return;
            }

            InitializeIfNeeded();
        }

        private void InitializeIfNeeded()
        {
            if (ServiceRegistry.Instance.IsInitialized)
            {
                return;
            }

            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }

            SaveSystem saveSystem = new SaveSystem();
            SaveData saveData = saveSystem.LoadOrCreate();
            saveData.Normalize();

            PlayerProfile profile = saveData.profile;
            InventorySystem inventorySystem = new InventorySystem(profile.inventory);
            ProgressionSystem progressionSystem = new ProgressionSystem();
            StatsSystem statsSystem = new StatsSystem();
            CharacterRosterSystem characterRosterSystem = new CharacterRosterSystem(profile);
            CombatSystem combatSystem = new CombatSystem(inventorySystem, progressionSystem, statsSystem);
            GatheringSystem gatheringSystem = new GatheringSystem(inventorySystem, progressionSystem, statsSystem);
            CraftingSystem craftingSystem = new CraftingSystem(inventorySystem);
            QuestSystem questSystem = new QuestSystem(
                profile,
                inventorySystem,
                progressionSystem,
                characterRosterSystem);
            OfflineProgressionSystem offlineProgressionSystem = new OfflineProgressionSystem(
                inventorySystem,
                progressionSystem,
                statsSystem);

            ServiceRegistry.Instance.Initialize(
                saveSystem,
                saveData,
                profile,
                inventorySystem,
                progressionSystem,
                statsSystem,
                characterRosterSystem,
                combatSystem,
                gatheringSystem,
                craftingSystem,
                questSystem,
                offlineProgressionSystem);

            // TODO: Trigger the future AFK rewards modal after UI flow exists.
        }
    }
}
