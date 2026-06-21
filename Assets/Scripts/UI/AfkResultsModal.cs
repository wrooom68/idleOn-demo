using System.Text;
using IdleGuildDemo.Core;
using IdleGuildDemo.Runtime;
using IdleGuildDemo.Systems;
using UnityEngine;
using UnityEngine.UI;

namespace IdleGuildDemo.UI
{
    public class AfkResultsModal : MonoBehaviour
    {
        [SerializeField] private GameObject root;
        [SerializeField] private Text titleText;
        [SerializeField] private Text durationText;
        [SerializeField] private Text resultsText;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button simulateTwoHoursButton;

        public Button CloseButton => closeButton;
        public Button SimulateTwoHoursButton => simulateTwoHoursButton;

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
        }

        public void Show()
        {
            SetVisible(true);
        }

        public void Hide()
        {
            SetVisible(false);
        }

        public void SetResults(string results)
        {
            SetText(titleText, "AFK Results");
            SetText(durationText, string.Empty);
            SetText(resultsText, results);
        }

        public void SetResults(AfkRewardSummary summary)
        {
            SetText(titleText, "AFK Results");

            if (summary == null)
            {
                SetText(durationText, "No AFK rewards were calculated.");
                SetText(resultsText, string.Empty);
                return;
            }

            SetText(durationText, FormatDuration(summary));

            StringBuilder builder = new StringBuilder();
            if (summary.characterRewards == null || summary.characterRewards.Count == 0)
            {
                builder.AppendLine("No characters reported rewards.");
            }
            else
            {
                foreach (CharacterAfkRewardSummary characterReward in summary.characterRewards)
                {
                    AppendCharacterReward(builder, characterReward);
                }
            }

            if (!summary.hasAnyRewards)
            {
                builder.AppendLine("No rewards yet. Assign a character to combat or mining first.");
            }

            SetText(resultsText, builder.ToString().TrimEnd());
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

        private static string FormatDuration(AfkRewardSummary summary)
        {
            string capped = $"{summary.cappedElapsedMinutes:0.#} minutes simulated";
            return summary.wasCapped ? $"{capped} (8 hour cap applied)" : capped;
        }

        private static void AppendCharacterReward(StringBuilder builder, CharacterAfkRewardSummary reward)
        {
            if (reward == null)
            {
                return;
            }

            builder.AppendLine($"{reward.characterName} - {FormatTask(reward.taskType, reward.targetId)}");
            builder.AppendLine($"XP +{reward.xpGained}  Coins +{reward.coinsGained}");

            if (reward.itemsGained != null && reward.itemsGained.Count > 0)
            {
                foreach (InventoryStack item in reward.itemsGained)
                {
                    if (item != null && item.quantity > 0)
                    {
                        builder.AppendLine($"{FormatItemName(item.itemId)} +{item.quantity}");
                    }
                }
            }

            if (!reward.hadRewards)
            {
                builder.AppendLine("No rewards for this task.");
            }

            builder.AppendLine();
        }

        private static string FormatTask(string taskType, string targetId)
        {
            if (taskType == GameConstants.TaskCombat && targetId == GameConstants.EnemySlimeId)
            {
                return "Fought Slimes";
            }

            if (taskType == GameConstants.TaskMining && targetId == GameConstants.ZoneMineCopperId)
            {
                return "Mined Copper";
            }

            return "Idle";
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

        private static void SetText(Text text, string value)
        {
            if (text != null)
            {
                text.text = value ?? string.Empty;
            }
        }
    }
}
