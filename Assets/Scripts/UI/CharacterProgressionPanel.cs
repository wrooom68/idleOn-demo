using IdleGuildDemo.Core;
using IdleGuildDemo.Data;
using IdleGuildDemo.Runtime;
using IdleGuildDemo.Systems;
using UnityEngine;
using UnityEngine.UI;

namespace IdleGuildDemo.UI
{
    /// <summary>
    /// Thin panel for character stats, class choice, and talent spending.
    /// </summary>
    public sealed class CharacterProgressionPanel : MonoBehaviour
    {
        [SerializeField] private GameObject root;
        [SerializeField] private Button closeButton;
        [SerializeField] private Text summaryText;
        [SerializeField] private Text statsText;
        [SerializeField] private Text statusText;
        [SerializeField] private Button warriorButton;
        [SerializeField] private Button archerButton;
        [SerializeField] private Button mageButton;
        [SerializeField] private Button damageTalentButton;
        [SerializeField] private Button miningSpeedTalentButton;
        [SerializeField] private Button xpGainTalentButton;
        [SerializeField] private Button afkGainTalentButton;
        [SerializeField] private ToastView toastView;
        [SerializeField] private TownHUDView townHudView;
        [SerializeField] private QuestDefinition[] questDefinitions;

        private void OnEnable()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (closeButton != null)
            {
                closeButton.onClick.AddListener(Hide);
            }

            if (warriorButton != null)
            {
                warriorButton.onClick.AddListener(ChooseWarrior);
            }

            if (archerButton != null)
            {
                archerButton.onClick.AddListener(ChooseArcher);
            }

            if (mageButton != null)
            {
                mageButton.onClick.AddListener(ChooseMage);
            }

            if (damageTalentButton != null)
            {
                damageTalentButton.onClick.AddListener(SpendDamageTalent);
            }

            if (miningSpeedTalentButton != null)
            {
                miningSpeedTalentButton.onClick.AddListener(SpendMiningSpeedTalent);
            }

            if (xpGainTalentButton != null)
            {
                xpGainTalentButton.onClick.AddListener(SpendXpGainTalent);
            }

            if (afkGainTalentButton != null)
            {
                afkGainTalentButton.onClick.AddListener(SpendAfkGainTalent);
            }
        }

        private void OnDisable()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (closeButton != null)
            {
                closeButton.onClick.RemoveListener(Hide);
            }

            if (warriorButton != null)
            {
                warriorButton.onClick.RemoveListener(ChooseWarrior);
            }

            if (archerButton != null)
            {
                archerButton.onClick.RemoveListener(ChooseArcher);
            }

            if (mageButton != null)
            {
                mageButton.onClick.RemoveListener(ChooseMage);
            }

            if (damageTalentButton != null)
            {
                damageTalentButton.onClick.RemoveListener(SpendDamageTalent);
            }

            if (miningSpeedTalentButton != null)
            {
                miningSpeedTalentButton.onClick.RemoveListener(SpendMiningSpeedTalent);
            }

            if (xpGainTalentButton != null)
            {
                xpGainTalentButton.onClick.RemoveListener(SpendXpGainTalent);
            }

            if (afkGainTalentButton != null)
            {
                afkGainTalentButton.onClick.RemoveListener(SpendAfkGainTalent);
            }
        }

        public void Show()
        {
            SetVisible(true);
            Refresh();
        }

        public void Hide()
        {
            SetVisible(false);
        }

        public void Refresh()
        {
            if (!TryGetActiveCharacter(out ServiceRegistry services, out CharacterState character))
            {
                SetStatus("Runtime is not ready.");
                return;
            }

            CharacterStats stats = services.StatsSystem.CalculateStats(character);
            SetText(summaryText, FormatSummary(services, character));
            SetText(statsText, FormatStats(stats));
            RefreshButtons(services, character);
        }

        public void ChooseWarrior()
        {
            ChooseClass(GameConstants.WarriorClassId);
        }

        public void ChooseArcher()
        {
            ChooseClass(GameConstants.ArcherClassId);
        }

        public void ChooseMage()
        {
            ChooseClass(GameConstants.MageClassId);
        }

        public void SpendDamageTalent()
        {
            SpendTalent(GameConstants.DamageTalentId);
        }

        public void SpendMiningSpeedTalent()
        {
            SpendTalent(GameConstants.MiningSpeedTalentId);
        }

        public void SpendXpGainTalent()
        {
            SpendTalent(GameConstants.XpGainTalentId);
        }

        public void SpendAfkGainTalent()
        {
            SpendTalent(GameConstants.AfkGainTalentId);
        }

        private void ChooseClass(string classId)
        {
            if (!TryGetActiveCharacter(out ServiceRegistry services, out CharacterState character))
            {
                SetStatus("Runtime is not ready.");
                return;
            }

            ClassSelectionResult result = services.ProgressionSystem.ChooseClass(character, classId);
            if (!result.success)
            {
                SetStatus(result.failureReason);
                Refresh();
                return;
            }

            QuestUpdateResult questResult = services.QuestSystem.ReportClassChosen(classId, questDefinitions);
            services.SaveSystem.Save(services.SaveData);

            string message = $"Class selected: {FormatClass(classId)}.";
            if (questResult.completed)
            {
                message += " Quest complete.";
            }

            SetStatus(message);
            Refresh();
            townHudView?.Refresh();
        }

        private void SpendTalent(string talentId)
        {
            if (!TryGetActiveCharacter(out ServiceRegistry services, out CharacterState character))
            {
                SetStatus("Runtime is not ready.");
                return;
            }

            TalentSpendResult result = services.ProgressionSystem.SpendTalentPoint(character, talentId);
            if (!result.success)
            {
                SetStatus(result.failureReason);
                Refresh();
                return;
            }

            services.SaveSystem.Save(services.SaveData);
            SetStatus($"{FormatTalent(talentId)} upgraded to rank {result.newRank}.");
            Refresh();
            townHudView?.Refresh();
        }

        private void RefreshButtons(ServiceRegistry services, CharacterState character)
        {
            bool canChooseClass = services.ProgressionSystem.CanChooseClass(character);
            SetInteractable(warriorButton, canChooseClass);
            SetInteractable(archerButton, canChooseClass);
            SetInteractable(mageButton, canChooseClass);

            bool canSpendTalent = character.unspentTalentPoints > 0;
            SetTalentButton(damageTalentButton, GameConstants.DamageTalentId, services, character, canSpendTalent);
            SetTalentButton(miningSpeedTalentButton, GameConstants.MiningSpeedTalentId, services, character, canSpendTalent);
            SetTalentButton(xpGainTalentButton, GameConstants.XpGainTalentId, services, character, canSpendTalent);
            SetTalentButton(afkGainTalentButton, GameConstants.AfkGainTalentId, services, character, canSpendTalent);
        }

        private static void SetTalentButton(
            Button button,
            string talentId,
            ServiceRegistry services,
            CharacterState character,
            bool canSpendTalent)
        {
            if (button == null)
            {
                return;
            }

            button.interactable = canSpendTalent;
            Text label = button.GetComponentInChildren<Text>();
            if (label != null)
            {
                int rank = services.ProgressionSystem.GetTalentRank(character, talentId);
                label.text = $"{FormatTalent(talentId)} + ({rank})";
            }
        }

        private static string FormatSummary(ServiceRegistry services, CharacterState character)
        {
            int xpRequired = services.ProgressionSystem.GetXpRequiredForLevel(character.level);
            return $"{character.displayName}\nLevel {character.level}  XP {character.currentXp}/{xpRequired}\nClass: {FormatClass(character.selectedClassId)}\nUnspent Talent Points: {character.unspentTalentPoints}";
        }

        private static string FormatStats(CharacterStats stats)
        {
            return $"Damage: {stats.damage}\nMining Speed: {stats.miningSpeedMultiplier:0.##}x\nXP Gain: {stats.xpGainMultiplier:0.##}x\nDrop Rate: {stats.dropRateMultiplier:0.##}x\nAFK Gain: {stats.afkGainMultiplier:0.##}x";
        }

        private static string FormatClass(string classId)
        {
            switch (classId)
            {
                case GameConstants.WarriorClassId:
                    return "Warrior";
                case GameConstants.ArcherClassId:
                    return "Archer";
                case GameConstants.MageClassId:
                    return "Mage";
                default:
                    return "None";
            }
        }

        private static string FormatTalent(string talentId)
        {
            switch (talentId)
            {
                case GameConstants.DamageTalentId:
                    return "Damage";
                case GameConstants.MiningSpeedTalentId:
                    return "Mining Speed";
                case GameConstants.XpGainTalentId:
                    return "XP Gain";
                case GameConstants.AfkGainTalentId:
                    return "AFK Gain";
                default:
                    return "Talent";
            }
        }

        private void SetVisible(bool visible)
        {
            if (root != null)
            {
                root.SetActive(visible);
            }
            else
            {
                gameObject.SetActive(visible);
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

        private static void SetInteractable(Button button, bool interactable)
        {
            if (button != null)
            {
                button.interactable = interactable;
            }
        }

        private static void SetText(Text text, string value)
        {
            if (text != null)
            {
                text.text = value ?? string.Empty;
            }
        }

        private static bool TryGetActiveCharacter(out ServiceRegistry services, out CharacterState character)
        {
            GameBootstrap.EnsureInitialized();
            services = ServiceRegistry.Instance;
            character = null;

            if (!services.IsInitialized)
            {
                Debug.LogError("ServiceRegistry is not initialized. Add GameBootstrap to the scene.");
                return false;
            }

            character = services.CharacterRosterSystem.GetActiveCharacter();
            return character != null;
        }
    }
}
