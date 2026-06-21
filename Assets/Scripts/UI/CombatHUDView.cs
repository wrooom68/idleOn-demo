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
    /// Thin placeholder view for automatic combat HUD state.
    /// </summary>
    public sealed class CombatHUDView : MonoBehaviour
    {
        [SerializeField] private EnemyDefinition slimeDefinition;
        [SerializeField] private Button backToTownButton;
        [SerializeField] private Button startCombatButton;
        [SerializeField] private Button attackButton;
        [SerializeField] private Image enemyHpFill;
        [SerializeField] private Text characterText;
        [SerializeField] private Text enemyText;
        [SerializeField] private Text rewardText;
        [SerializeField] private Text statusText;

        private CombatEnemyState enemyState;

        private void OnEnable()
        {
            if (backToTownButton != null)
            {
                backToTownButton.onClick.AddListener(BackToTown);
            }

            if (startCombatButton != null)
            {
                startCombatButton.onClick.AddListener(StartSlimeCombat);
            }

            if (attackButton != null)
            {
                attackButton.onClick.AddListener(AttackOnce);
            }

            Refresh();
        }

        private void OnDisable()
        {
            if (backToTownButton != null)
            {
                backToTownButton.onClick.RemoveListener(BackToTown);
            }

            if (startCombatButton != null)
            {
                startCombatButton.onClick.RemoveListener(StartSlimeCombat);
            }

            if (attackButton != null)
            {
                attackButton.onClick.RemoveListener(AttackOnce);
            }
        }

        public void BackToTown()
        {
            SceneManager.LoadScene(GameConstants.TownSceneName);
        }

        public void StartSlimeCombat()
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
                GameConstants.TaskCombat,
                GameConstants.EnemySlimeId);

            enemyState = slimeDefinition != null
                ? services.CombatSystem.CreateEnemyState(slimeDefinition)
                : null;

            services.SaveSystem.Save(services.SaveData);
            SetStatus(slimeDefinition == null ? "Assign a Slime enemy definition." : "Slime combat started.");
            Refresh();
        }

        public void AttackOnce()
        {
            if (!TryGetServices(out ServiceRegistry services))
            {
                SetStatus("Runtime is not ready.");
                return;
            }

            if (slimeDefinition == null)
            {
                SetStatus("Assign a Slime enemy definition.");
                return;
            }

            CharacterState character = services.CharacterRosterSystem.GetActiveCharacter();
            if (character == null)
            {
                SetStatus("No active character.");
                return;
            }

            if (enemyState == null || enemyState.IsDefeated)
            {
                enemyState = services.CombatSystem.CreateEnemyState(slimeDefinition);
            }

            CombatTickResult result = services.CombatSystem.Attack(character, enemyState, slimeDefinition);
            if (!string.IsNullOrEmpty(result.failureReason))
            {
                SetStatus(result.failureReason);
            }
            else
            {
                SetStatus(result.enemyDefeated ? "Slime defeated." : $"Hit for {result.damageDealt}.");
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
                SetText(characterText, $"{character.displayName} L{character.level}");
            }

            string enemyName = slimeDefinition != null && !string.IsNullOrEmpty(slimeDefinition.DisplayName)
                ? slimeDefinition.DisplayName
                : "Slime";
            string hpText = enemyState != null ? $"{enemyState.currentHp}/{enemyState.maxHp}" : "--";
            SetText(enemyText, $"{enemyName} HP {hpText}");

            if (enemyHpFill != null)
            {
                enemyHpFill.fillAmount = enemyState != null && enemyState.maxHp > 0
                    ? (float)enemyState.currentHp / enemyState.maxHp
                    : 0f;
            }
        }

        private void SetRewardText(CombatTickResult result)
        {
            if (rewardText == null || result == null)
            {
                return;
            }

            if (!result.enemyDefeated)
            {
                rewardText.text = string.Empty;
                return;
            }

            rewardText.text = $"XP +{result.xpGained}, Coins +{result.coinsGained}, {result.itemDroppedId} +{result.itemDroppedQuantity}";
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
            services = ServiceRegistry.Instance;
            return services.IsInitialized;
        }
    }
}
