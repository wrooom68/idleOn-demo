using System;
using IdleGuildDemo.Core;
using IdleGuildDemo.Data;
using IdleGuildDemo.Runtime;
using IdleGuildDemo.Systems;
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
        [SerializeField] private Button claimQuestButton;
        [SerializeField] private Text activeCharacterText;
        [SerializeField] private Text levelText;
        [SerializeField] private Text xpText;
        [SerializeField] private Text taskText;
        [SerializeField] private Text coinsText;
        [SerializeField] private Text inventoryText;
        [SerializeField] private Text questTitleText;
        [SerializeField] private Text questObjectiveText;
        [SerializeField] private Text questProgressText;
        [SerializeField] private Image questProgressFill;
        [SerializeField] private QuestTrackerView questTrackerView;
        [SerializeField] private Text statusText;
        [SerializeField] private HUDView hudView;
        [SerializeField] private InventoryPanel inventoryPanel;
        [SerializeField] private CraftingPanel craftingPanel;
        [SerializeField] private CharacterPanel characterPanel;
        [SerializeField] private InventoryCraftingPanel inventoryCraftingPanel;
        [SerializeField] private CharacterProgressionPanel characterProgressionPanel;
        [SerializeField] private AfkResultsModal afkResultsModal;
        [SerializeField] private LootLogView lootLogView;
        [SerializeField] private ToastView toastView;
        [SerializeField] private ItemDefinition[] itemDefinitions;
        [SerializeField] private RecipeDefinition[] recipeDefinitions;
        [SerializeField] private QuestDefinition[] questDefinitions;

        private void OnEnable()
        {
            if (!Application.isPlaying) return;

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

            if (claimQuestButton != null)
            {
                claimQuestButton.onClick.AddListener(ClaimQuestReward);
            }

            Refresh();
        }

        private void OnDisable()
        {
            if (!Application.isPlaying) return;

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

            if (claimQuestButton != null)
            {
                claimQuestButton.onClick.RemoveListener(ClaimQuestReward);
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
            SyncAutoQuestProgress(services, character);
            SetText(activeCharacterText, character.displayName);
            SetText(levelText, $"Level {character.level} | XP {character.currentXp}");
            SetText(xpText, $"XP {character.currentXp}");
            SetText(taskText, GetTaskLabel(character.currentTask));
            SetText(coinsText, GetCurrencyAndInventoryLabel(services));
            SetText(inventoryText, GetInventoryLabel(services));
            RefreshBoundPanels(services);
            RefreshQuest(services);
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
            if (inventoryPanel != null)
            {
                if (!TryGetServices(out ServiceRegistry services))
                {
                    SetStatus("Runtime is not ready.");
                    return;
                }

                inventoryPanel.RefreshFromServices(services, itemDefinitions);
                inventoryPanel.Show();
                SetStatus("Inventory opened.");
                return;
            }

            if (inventoryCraftingPanel != null)
            {
                inventoryCraftingPanel.Show();
                SetStatus("Inventory and crafting opened.");
                return;
            }

            SetStatus("Inventory panel is not assigned.");
        }

        public void OpenCharacterPanel()
        {
            if (characterPanel != null)
            {
                if (!TryGetServices(out ServiceRegistry services))
                {
                    SetStatus("Runtime is not ready.");
                    return;
                }

                characterPanel.RefreshFromServices(services);
                characterPanel.Show();
                SetStatus("Character roster opened.");
                return;
            }

            if (characterProgressionPanel != null)
            {
                characterProgressionPanel.Show();
                SetStatus("Character and talents opened.");
                return;
            }

            SetStatus("Character panel is not assigned.");
        }

        public void SimulateAfkTwoHours()
        {
            if (!TryGetServices(out ServiceRegistry services))
            {
                SetStatus("Runtime is not ready.");
                return;
            }

            AfkRewardSummary summary = services.OfflineProgressionSystem.SimulateAndApplyRewards(
                services.PlayerProfile,
                TimeSpan.FromHours(GameConstants.OfflineDemoSimulatedHours));
            services.SaveSystem.Save(services.SaveData);
            if (afkResultsModal != null)
            {
                afkResultsModal.SetResults(summary);
                afkResultsModal.Show();
            }

            lootLogView?.AddAfkRewards(summary);
            SetStatus("AFK rewards applied.");
            Refresh();
        }

        public void ClaimQuestReward()
        {
            if (!TryGetServices(out ServiceRegistry services))
            {
                SetStatus("Runtime is not ready.");
                return;
            }

            QuestClaimResult result = services.QuestSystem.ClaimCurrentQuest(questDefinitions);
            if (!result.success)
            {
                SetStatus(string.IsNullOrEmpty(result.failureReason) ? "Quest is not ready to claim." : result.failureReason);
                Refresh();
                return;
            }

            services.SaveSystem.Save(services.SaveData);
            string message = result.unlockedSecondCharacter
                ? "Quest claimed. Character 2 unlocked."
                : "Quest claimed.";
            SetStatus(message);
            Refresh();
        }

        public void OpenCrafting()
        {
            if (!TryGetServices(out ServiceRegistry services))
            {
                SetStatus("Runtime is not ready.");
                return;
            }

            if (craftingPanel != null)
            {
                craftingPanel.Bind(services.CraftingSystem, recipeDefinitions, itemDefinitions);
                craftingPanel.SetQuestDefinitions(questDefinitions);
                craftingPanel.Show();
                SetStatus("Crafting opened.");
                return;
            }

            OpenInventory();
        }

        public void OpenCharacterProgression()
        {
            if (characterProgressionPanel != null)
            {
                characterProgressionPanel.Show();
                SetStatus("Character progression opened.");
                return;
            }

            OpenCharacterPanel();
        }

        private void RefreshBoundPanels(ServiceRegistry services)
        {
            hudView?.RefreshFromServices(services, "Town");
            hudView?.RefreshQuest(services.QuestSystem, questDefinitions, services.PlayerProfile);
            inventoryPanel?.RefreshFromServices(services, itemDefinitions);
            craftingPanel?.Bind(services.CraftingSystem, recipeDefinitions, itemDefinitions);
            craftingPanel?.SetQuestDefinitions(questDefinitions);
            characterPanel?.RefreshFromServices(services);
        }

        private static string GetInventoryLabel(ServiceRegistry services)
        {
            int slimeGoo = services.InventorySystem.GetQuantity(GameConstants.ItemSlimeGooId);
            int copperOre = services.InventorySystem.GetQuantity(GameConstants.ItemCopperOreId);
            int copperBar = services.InventorySystem.GetQuantity(GameConstants.ItemCopperBarId);
            return $"Inventory: Slime Goo {slimeGoo}, Copper Ore {copperOre}, Copper Bar {copperBar}";
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
                ? $"Task: {FormatTaskType(task.taskType)}"
                : $"Task: {FormatTaskType(task.taskType)} - {FormatTargetName(task.targetId)}";
        }

        private void RefreshQuest(ServiceRegistry services)
        {
            QuestDefinition quest = services.QuestSystem.GetCurrentQuest(questDefinitions);
            if (quest == null)
            {
                questTrackerView?.SetComplete();
                SetText(questTitleText, "Quest Chain Complete");
                SetText(questObjectiveText, "All tutorial quests are complete.");
                SetText(questProgressText, string.Empty);
                SetProgress(questProgressFill, 1f);
                SetInteractable(claimQuestButton, false);
                return;
            }

            services.PlayerProfile.questProgress.Normalize();
            int required = quest.RequiredAmount > 0 ? quest.RequiredAmount : 1;
            int current = services.PlayerProfile.questProgress.currentAmount;
            if (current > required)
            {
                current = required;
            }

            bool canClaim = services.QuestSystem.CanClaimCurrentQuest(questDefinitions);
            questTrackerView?.SetQuest(quest, services.PlayerProfile.questProgress, canClaim);
            SetText(questTitleText, quest.DisplayName);
            SetText(questObjectiveText, GetQuestObjectiveLabel(quest));
            SetText(questProgressText, $"{current}/{required}");
            SetProgress(questProgressFill, required > 0 ? (float)current / required : 0f);
            SetInteractable(claimQuestButton, canClaim);
        }

        private void SyncAutoQuestProgress(ServiceRegistry services, CharacterState character)
        {
            QuestDefinition quest = services.QuestSystem.GetCurrentQuest(questDefinitions);
            if (quest == null)
            {
                return;
            }

            if (quest.ObjectiveType == QuestObjectiveType.CollectItem)
            {
                int quantity = services.InventorySystem.GetQuantity(quest.TargetId);
                int delta = quantity - services.PlayerProfile.questProgress.currentAmount;
                if (delta > 0)
                {
                    services.QuestSystem.ReportItemCollected(quest.TargetId, delta, questDefinitions);
                }
            }
            else if (quest.ObjectiveType == QuestObjectiveType.ReachLevel)
            {
                services.QuestSystem.ReportLevelReached(character.level, questDefinitions);
            }
            else if (quest.ObjectiveType == QuestObjectiveType.ChooseClass && !string.IsNullOrEmpty(character.selectedClassId))
            {
                services.QuestSystem.ReportClassChosen(character.selectedClassId, questDefinitions);
            }
        }

        private static string GetQuestObjectiveLabel(QuestDefinition quest)
        {
            switch (quest.ObjectiveType)
            {
                case QuestObjectiveType.KillEnemy:
                    return $"Defeat {GetRequiredAmount(quest)} Slimes";
                case QuestObjectiveType.CollectItem:
                    return $"Collect {GetRequiredAmount(quest)} {FormatItemName(quest.TargetId)}";
                case QuestObjectiveType.CraftItem:
                    return $"Craft {GetRequiredAmount(quest)} {FormatItemName(quest.TargetId)}";
                case QuestObjectiveType.ReachLevel:
                    return $"Reach level {GetRequiredAmount(quest)}";
                case QuestObjectiveType.ChooseClass:
                    return "Choose a class";
                case QuestObjectiveType.UnlockCharacter:
                    return "Unlock Character 2";
                default:
                    return quest.Description;
            }
        }

        private static int GetRequiredAmount(QuestDefinition quest)
        {
            return quest != null && quest.RequiredAmount > 0 ? quest.RequiredAmount : 1;
        }

        private static string FormatTaskType(string taskType)
        {
            switch (taskType)
            {
                case GameConstants.TaskCombat:
                    return "Fighting";
                case GameConstants.TaskMining:
                    return "Mining";
                default:
                    return string.IsNullOrEmpty(taskType) ? "Idle" : taskType;
            }
        }

        private static string FormatTargetName(string targetId)
        {
            switch (targetId)
            {
                case GameConstants.EnemySlimeId:
                    return "Slimes";
                case GameConstants.ZoneMineCopperId:
                    return "Copper";
                default:
                    return string.IsNullOrEmpty(targetId) ? "None" : targetId;
            }
        }

        private static string FormatItemName(string itemId)
        {
            switch (itemId)
            {
                case GameConstants.ItemCopperOreId:
                    return "Copper Ore";
                case GameConstants.ItemCopperBarId:
                    return "Copper Bar";
                case GameConstants.ItemSlimeGooId:
                    return "Slime Goo";
                case GameConstants.ItemCopperSwordId:
                    return "Copper Sword";
                case GameConstants.ItemCopperPickaxeId:
                    return "Copper Pickaxe";
                default:
                    return string.IsNullOrEmpty(itemId) ? "Item" : itemId;
            }
        }

        private void SetStatus(string message)
        {
            SetText(statusText, message);
            if (!string.IsNullOrEmpty(message))
            {
                toastView?.Show(message);
            }
        }

        private static void SetText(Text text, string value)
        {
            if (text != null)
            {
                text.text = value;
            }
        }

        private static void SetProgress(Image image, float normalized)
        {
            if (image != null)
            {
                image.fillAmount = Mathf.Clamp01(normalized);
            }
        }

        private static void SetInteractable(Button button, bool interactable)
        {
            if (button != null)
            {
                button.interactable = interactable;
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
