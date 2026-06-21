using System;
using IdleGuildDemo.Core;
using IdleGuildDemo.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace IdleGuildDemo.UI
{
    /// <summary>
    /// Thin placeholder view for the town hub HUD.
    /// </summary>
    public sealed class TownHUDView : MonoBehaviour
    {
        [SerializeField] private Button inventoryButton;
        [SerializeField] private Button characterPanelButton;
        [SerializeField] private Button combatButton;
        [SerializeField] private Button miningButton;
        [SerializeField] private Button simulateAfkButton;
        [SerializeField] private Text activeCharacterText;
        [SerializeField] private Text levelText;
        [SerializeField] private Text xpText;
        [SerializeField] private Text taskText;
        [SerializeField] private Text coinsText;
        [SerializeField] private Text inventoryText;
        [SerializeField] private Text statusText;

        private void OnEnable()
        {
            if (inventoryButton != null)
            {
                inventoryButton.onClick.AddListener(OpenInventory);
            }

            if (characterPanelButton != null)
            {
                characterPanelButton.onClick.AddListener(OpenCharacterPanel);
            }

            if (combatButton != null)
            {
                combatButton.onClick.AddListener(GoToCombat);
            }

            if (miningButton != null)
            {
                miningButton.onClick.AddListener(GoToMining);
            }

            if (simulateAfkButton != null)
            {
                simulateAfkButton.onClick.AddListener(SimulateAfkTwoHours);
            }

            Refresh();
        }

        private void OnDisable()
        {
            if (inventoryButton != null)
            {
                inventoryButton.onClick.RemoveListener(OpenInventory);
            }

            if (characterPanelButton != null)
            {
                characterPanelButton.onClick.RemoveListener(OpenCharacterPanel);
            }

            if (combatButton != null)
            {
                combatButton.onClick.RemoveListener(GoToCombat);
            }

            if (miningButton != null)
            {
                miningButton.onClick.RemoveListener(GoToMining);
            }

            if (simulateAfkButton != null)
            {
                simulateAfkButton.onClick.RemoveListener(SimulateAfkTwoHours);
            }
        }

        public void Refresh()
        {
            if (!TryGetServices(out ServiceRegistry services))
            {
                SetStatus("Runtime is not ready.");
                return;
            }

            CharacterState character = services.CharacterRosterSystem.GetActiveCharacter();
            if (character == null)
            {
                SetStatus("No active character.");
                return;
            }

            character.Normalize();
            SetText(activeCharacterText, character.displayName);
            SetText(levelText, $"Level {character.level} | XP {character.currentXp}");
            SetText(xpText, $"XP {character.currentXp}");
            SetText(taskText, GetTaskLabel(character.currentTask));
            SetText(coinsText, GetCurrencyAndInventoryLabel(services));
            SetText(inventoryText, GetInventoryLabel(services));
        }

        public void GoToCombat()
        {
            SceneManager.LoadScene(GameConstants.CombatSceneName);
        }

        public void GoToMining()
        {
            SceneManager.LoadScene(GameConstants.MineSceneName);
        }

        public void OpenInventory()
        {
            SetStatus("Inventory panel hook ready.");
        }

        public void OpenCharacterPanel()
        {
            SetStatus("Character panel hook ready.");
        }

        public void SimulateAfkTwoHours()
        {
            if (!TryGetServices(out ServiceRegistry services))
            {
                SetStatus("Runtime is not ready.");
                return;
            }

            services.OfflineProgressionSystem.SimulateAndApplyRewards(
                services.PlayerProfile,
                TimeSpan.FromHours(GameConstants.OfflineDemoSimulatedHours));
            services.SaveSystem.Save(services.SaveData);
            SetStatus("Simulated 2 hours AFK.");
            Refresh();
        }

        private static string GetInventoryLabel(ServiceRegistry services)
        {
            int slimeGoo = services.InventorySystem.GetQuantity(GameConstants.ItemSlimeGooId);
            int copperOre = services.InventorySystem.GetQuantity(GameConstants.ItemCopperOreId);
            return $"Inventory: Slime Goo {slimeGoo}, Copper Ore {copperOre}";
        }

        private static string GetCurrencyAndInventoryLabel(ServiceRegistry services)
        {
            int slimeGoo = services.InventorySystem.GetQuantity(GameConstants.ItemSlimeGooId);
            int copperOre = services.InventorySystem.GetQuantity(GameConstants.ItemCopperOreId);
            return $"Coins {services.PlayerProfile.coins} | Slime Goo {slimeGoo} | Copper Ore {copperOre}";
        }

        private static string GetTaskLabel(TaskState task)
        {
            if (task == null || string.IsNullOrEmpty(task.taskType) || task.taskType == GameConstants.TaskIdle)
            {
                return "Task: idle";
            }

            return string.IsNullOrEmpty(task.targetId)
                ? $"Task: {task.taskType}"
                : $"Task: {task.taskType} / {task.targetId}";
        }

        private void SetStatus(string message)
        {
            SetText(statusText, message);
        }

        private static void SetText(Text text, string value)
        {
            if (text != null)
            {
                text.text = value;
            }
        }

        private static bool TryGetServices(out ServiceRegistry services)
        {
            GameBootstrap.EnsureInitialized();
            services = ServiceRegistry.Instance;
            if (!services.IsInitialized)
            {
                Debug.LogError("ServiceRegistry is not initialized. Add GameBootstrap to the scene.");
            }

            return services.IsInitialized;
        }
    }
}
