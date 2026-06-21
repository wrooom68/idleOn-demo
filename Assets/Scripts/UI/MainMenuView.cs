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

        public Button NewGameButton => newGameButton;
        public Button ContinueButton => continueButton;

        private void OnEnable()
        {
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

            SetEvaluationNote("Designed to show all major systems in under 30 minutes");
            Refresh();
        }

        private void OnDisable()
        {
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
            SaveSystem saveSystem = GetSaveSystem();
            saveSystem.DeleteSave();
            SaveData saveData = SaveData.CreateNew();
            saveSystem.Save(saveData);
            RegisterRuntime(saveSystem, saveData);
            LoadTownScene();
        }

        public void ContinueGame()
        {
            SaveSystem saveSystem = GetSaveSystem();
            SaveData saveData = saveSystem.LoadOrCreate();
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
            SaveSystem saveSystem = GetSaveSystem();
            saveSystem.DeleteSave();
            ServiceRegistry.Instance.Clear();
            SetStatus("Save reset.");
            Refresh();
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

        private void Refresh()
        {
            SetContinueAvailable(GetSaveSystem().SaveExists());
        }

        private void SetStatus(string message)
        {
            if (statusText != null)
            {
                statusText.text = message;
            }
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
