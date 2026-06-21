using System;
using IdleGuildDemo.Core;
using IdleGuildDemo.Runtime;
using IdleGuildDemo.Save;
using IdleGuildDemo.Systems;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace IdleGuildDemo.UI
{
    public sealed class MainMenuView : MonoBehaviour
    {
        [SerializeField] private Button newGameButton;
        [SerializeField] private Button continueButton;
        [SerializeField] private Button resetButton;
        [SerializeField] private Text evaluationNoteText;
        [SerializeField] private Text statusText;
        [SerializeField] private bool requireResetConfirmation = true;

        public Button NewGameButton => newGameButton;
        public Button ContinueButton => continueButton;
        public Button ResetButton => resetButton;

        private bool resetConfirmationPending;

        private void OnEnable()
        {
            if (!Application.isPlaying) return;

            if (newGameButton != null)
            {
                newGameButton.onClick.AddListener(NewGame);
            }

            if (continueButton != null)
            {
                continueButton.onClick.AddListener(ContinueGame);
            }

            if (resetButton != null)
            {
                resetButton.onClick.AddListener(DeleteSave);
            }

            ApplyDefaultText();
            RefreshMenuState();
        }

        private void OnDisable()
        {
            if (!Application.isPlaying) return;

            if (newGameButton != null)
            {
                newGameButton.onClick.RemoveListener(NewGame);
            }

            if (continueButton != null)
            {
                continueButton.onClick.RemoveListener(ContinueGame);
            }

            if (resetButton != null)
            {
                resetButton.onClick.RemoveListener(DeleteSave);
            }
        }

        public void NewGame()
        {
            ClearResetConfirmation();
            SetStatus("Starting a fresh demo save...");
            SaveSystem saveSystem = GetSaveSystem();
            saveSystem.DeleteSave();
            SaveData saveData = SaveData.CreateNew();
            saveSystem.Save(saveData);
            RegisterRuntime(saveSystem, saveData);
            LoadTownScene();
        }

        public void ContinueGame()
        {
            ClearResetConfirmation();
            SaveSystem saveSystem = GetSaveSystem();
            if (!saveSystem.TryLoadExisting(out SaveData saveData))
            {
                SetStatus("No valid save found. Choose New Game to start the reviewer path.");
                RefreshMenuState();
                return;
            }

            SetStatus("Continuing saved demo progress...");
            saveData.Normalize();
            RegisterRuntime(saveSystem, saveData);
            LoadTownScene();
        }

        public void LoadTownScene()
        {
            SceneManager.LoadScene(GameConstants.TownSceneName);
        }

        public void DeleteSave()
        {
            if (requireResetConfirmation && !resetConfirmationPending)
            {
                resetConfirmationPending = true;
                SetButtonText(resetButton, "Confirm Reset");
                SetStatus("Press Reset Save again to permanently clear the local save.");
                return;
            }

            ResetSaveImmediately();
        }

        public void ResetSaveImmediately()
        {
            SaveSystem saveSystem = GetSaveSystem();
            saveSystem.DeleteSave();
            ServiceRegistry.Instance.Clear();
            ClearResetConfirmation();
            SetStatus("Save reset. New Game will start from the beginning.");
            RefreshMenuState();
        }

        public void SetContinueAvailable(bool available)
        {
            if (continueButton != null)
            {
                continueButton.interactable = available;
            }
        }

        public void SetEvaluationNote(string note)
        {
            if (evaluationNoteText != null)
            {
                evaluationNoteText.text = note;
            }
        }

        public void RefreshMenuState()
        {
            bool hasSave = GetSaveSystem().TryLoadExisting(out SaveData saveData);
            SetContinueAvailable(hasSave);

            if (string.IsNullOrEmpty(GetStatus()))
            {
                SetStatus(hasSave ? GetSaveFoundStatus(saveData) : "No save found. Start a new demo run.");
            }
        }

        private void SetStatus(string message)
        {
            if (statusText != null)
            {
                statusText.text = message;
            }
        }

        private string GetStatus()
        {
            return statusText != null ? statusText.text : string.Empty;
        }

        private void ApplyDefaultText()
        {
            SetEvaluationNote("Designed to show all major systems in under 30 minutes");
            SetButtonText(newGameButton, "New Game");
            SetButtonText(continueButton, "Continue");
            SetButtonText(resetButton, resetConfirmationPending ? "Confirm Reset" : "Reset Save");
        }

        private static void SetButtonText(Button button, string label)
        {
            if (button == null)
            {
                return;
            }

            Text text = button.GetComponentInChildren<Text>();
            if (text != null)
            {
                text.text = label;
            }
        }

        private static string GetSaveFoundStatus(SaveData saveData)
        {
            if (saveData == null || string.IsNullOrEmpty(saveData.lastSavedUtc))
            {
                return "Save found. Continue or start fresh.";
            }

            if (DateTime.TryParse(saveData.lastSavedUtc, out DateTime lastSavedUtc))
            {
                return $"Save found. Last saved {lastSavedUtc.ToLocalTime():yyyy-MM-dd HH:mm}.";
            }

            return "Save found. Continue or start fresh.";
        }

        private void ClearResetConfirmation()
        {
            resetConfirmationPending = false;
            SetButtonText(resetButton, "Reset Save");
        }

        private static SaveSystem GetSaveSystem()
        {
            GameBootstrap.EnsureInitialized();
            return ServiceRegistry.Instance.IsInitialized
                ? ServiceRegistry.Instance.SaveSystem
                : new SaveSystem();
        }

        private static void RegisterRuntime(SaveSystem saveSystem, SaveData saveData)
        {
            saveData.Normalize();
            PlayerProfile profile = saveData.profile;
            InventorySystem inventorySystem = new InventorySystem(profile.inventory);
            ProgressionSystem progressionSystem = new ProgressionSystem();
            StatsSystem statsSystem = new StatsSystem();
            CharacterRosterSystem rosterSystem = new CharacterRosterSystem(profile);
            CombatSystem combatSystem = new CombatSystem(inventorySystem, progressionSystem, statsSystem);
            GatheringSystem gatheringSystem = new GatheringSystem(inventorySystem, progressionSystem, statsSystem);
            CraftingSystem craftingSystem = new CraftingSystem(inventorySystem);
            QuestSystem questSystem = new QuestSystem(profile, inventorySystem, progressionSystem, rosterSystem);
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
                rosterSystem,
                combatSystem,
                gatheringSystem,
                craftingSystem,
                questSystem,
                offlineProgressionSystem);
        }
    }
}
