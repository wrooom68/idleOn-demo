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
        [SerializeField] private ToastView toastView;
        [SerializeField] private QuestDefinition[] questDefinitions;

        private CombatEnemyState enemyState;

        public EnemyDefinition SlimeDefinition => slimeDefinition;

        public CombatTickResult AttackEnemyState(CombatEnemyState state)
        {
            if (!TryGetServices(out ServiceRegistry services))
            {
                SetStatus("Runtime is not ready.");
                return null;
            }

            CharacterState character = services.CharacterRosterSystem.GetActiveCharacter();
            if (character == null)
            {
                SetStatus("No active character.");
                return null;
            }

            // Sync the main HUD's tracked enemyState so the big top HP bar reflects this slime
            this.enemyState = state;

            CombatTickResult result = services.CombatSystem.Attack(character, state, slimeDefinition);
            if (result.enemyDefeated && result.coinsGained > 0)
            {
                services.PlayerProfile.coins += result.coinsGained;
            }

            QuestUpdateResult questResult = null;
            if (result.enemyDefeated)
            {
                questResult = services.QuestSystem.ReportKill(GameConstants.EnemySlimeId, questDefinitions);
            }

            if (!string.IsNullOrEmpty(result.failureReason))
            {
                SetStatus(result.failureReason);
            }
            else
            {
                string message = result.enemyDefeated ? "Slime defeated." : $"Hit for {result.damageDealt}.";
                if (questResult != null && questResult.completed)
                {
                    message += " Quest complete.";
                }

                SetStatus(message);
            }

            SetRewardText(result);
            services.SaveSystem.Save(services.SaveData);
            Refresh();

            return result;
        }

        private void OnEnable()
        {
            if (!Application.isPlaying) return;

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
            if (!Application.isPlaying) return;

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
            if (result.enemyDefeated && result.coinsGained > 0)
            {
                services.PlayerProfile.coins += result.coinsGained;
            }

            QuestUpdateResult questResult = null;
            if (result.enemyDefeated)
            {
                questResult = services.QuestSystem.ReportKill(GameConstants.EnemySlimeId, questDefinitions);
            }

            if (!string.IsNullOrEmpty(result.failureReason))
            {
                SetStatus(result.failureReason);
            }
            else
            {
                string message = result.enemyDefeated ? "Slime defeated." : $"Hit for {result.damageDealt}.";
                if (questResult != null && questResult.completed)
                {
                    message += " Quest complete.";
                }

                SetStatus(message);
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

            rewardText.text = $"XP +{result.xpGained}, Coins +{result.coinsGained}, {FormatItemName(result.itemDroppedId)} +{result.itemDroppedQuantity}";
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

        private static string FormatItemName(string itemId)
        {
            switch (itemId)
            {
                case GameConstants.ItemSlimeGooId:
                    return "Slime Goo";
                case GameConstants.ItemCopperOreId:
                    return "Copper Ore";
                case GameConstants.ItemCopperBarId:
                    return "Copper Bar";
                case GameConstants.ItemCopperSwordId:
                    return "Copper Sword";
                case GameConstants.ItemCopperPickaxeId:
                    return "Copper Pickaxe";
                default:
                    return string.IsNullOrEmpty(itemId) ? "Item" : itemId;
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
