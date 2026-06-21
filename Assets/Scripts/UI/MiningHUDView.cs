using IdleGuildDemo.Core;
using IdleGuildDemo.Runtime;
using IdleGuildDemo.Systems;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace IdleGuildDemo.UI
{
    /// <summary>
    /// Thin placeholder view for mining HUD state.
    /// </summary>
    public sealed class MiningHUDView : MonoBehaviour
    {
        [SerializeField] private Button backToTownButton;
        [SerializeField] private Button startMiningButton;
        [SerializeField] private Button tickMiningButton;
        [SerializeField] private Image miningProgressFill;
        [SerializeField] private Text characterText;
        [SerializeField] private Text progressText;
        [SerializeField] private Text rewardText;
        [SerializeField] private Text statusText;
        [SerializeField] private float manualTickSeconds = 2f;

        private GatheringState gatheringState;

        private void OnEnable()
        {
            if (!Application.isPlaying) return;

            if (backToTownButton != null)
            {
                backToTownButton.onClick.AddListener(BackToTown);
            }

            if (startMiningButton != null)
            {
                startMiningButton.onClick.AddListener(StartCopperMining);
            }

            if (tickMiningButton != null)
            {
                tickMiningButton.onClick.AddListener(TickMiningOnce);
            }

            Refresh();
        }

        private void OnDisable()
        {
            if (!Application.isPlaying) return;

            if (backToTownButton != null)
            {
                backToTownButton.onClick.RemoveListener(BackToTown);
            }

            if (startMiningButton != null)
            {
                startMiningButton.onClick.RemoveListener(StartCopperMining);
            }

            if (tickMiningButton != null)
            {
                tickMiningButton.onClick.RemoveListener(TickMiningOnce);
            }
        }

        public void BackToTown()
        {
            SceneManager.LoadScene(GameConstants.TownSceneName);
        }

        public void StartCopperMining()
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

            services.CharacterRosterSystem.AssignTask(
                character.characterId,
                GameConstants.TaskMining,
                GameConstants.ZoneMineCopperId);

            gatheringState = services.GatheringSystem.CreateCopperMiningState(character);
            services.SaveSystem.Save(services.SaveData);
            SetStatus("Copper mining started.");
            Refresh();
        }

        public void TickMiningOnce()
        {
            TickMining(manualTickSeconds);
        }

        public void TickMining(float deltaSeconds)
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

            if (gatheringState == null)
            {
                gatheringState = services.GatheringSystem.CreateCopperMiningState(character);
            }

            GatheringTickResult result = services.GatheringSystem.TickCopperMining(
                character,
                gatheringState,
                deltaSeconds);

            if (!string.IsNullOrEmpty(result.failureReason))
            {
                SetStatus(result.failureReason);
            }
            else
            {
                SetStatus(result.completed ? "Copper ore gathered." : "Mining progressed.");
            }

            SetRewardText(result);
            services.SaveSystem.Save(services.SaveData);
            Refresh();
        }

        public void Refresh()
        {
            if (!TryGetServices(out ServiceRegistry services))
            {
                SetStatus("Runtime is not ready.");
                return;
            }

            CharacterState character = services.CharacterRosterSystem.GetActiveCharacter();
            if (character != null)
            {
                SetText(characterText, $"{character.displayName} L{character.level} XP {character.currentXp}");
            }

            float elapsed = gatheringState?.elapsedSeconds ?? 0f;
            float required = gatheringState?.requiredSeconds ?? 0f;
            SetText(progressText, required > 0f ? $"{elapsed:0.0}/{required:0.0}s" : "--");

            if (miningProgressFill != null)
            {
                miningProgressFill.fillAmount = required > 0f ? Mathf.Clamp01(elapsed / required) : 0f;
            }
        }

        private void SetRewardText(GatheringTickResult result)
        {
            if (rewardText == null || result == null)
            {
                return;
            }

            rewardText.text = result.completed
                ? $"XP +{result.xpGained}, {result.itemGainedId} +{result.itemGainedQuantity}"
                : string.Empty;
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
