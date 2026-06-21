using UnityEngine;
using UnityEngine.UI;
using IdleGuildDemo.Systems;

namespace IdleGuildDemo.UI
{
  public class LootLogView : MonoBehaviour
  {
    [SerializeField] private Text logText;
    [SerializeField] private int maxLines = 6;

    private readonly System.Collections.Generic.List<string> _lines = new();

    public void AddEntry(string entry)
    {
      _lines.Insert(0, entry);
      while (_lines.Count > maxLines)
      {
        _lines.RemoveAt(_lines.Count - 1);
      }

      if (logText != null)
      {
        logText.text = string.Join("\n", _lines);
      }
    }

    public void AddCombatResult(CombatTickResult result)
    {
      if (result == null)
      {
        return;
      }

      if (!string.IsNullOrEmpty(result.failureReason))
      {
        AddEntry(result.failureReason);
        return;
      }

      if (result.enemyDefeated)
      {
        AddEntry(FormatRewardLine("Slime defeated", result.xpGained, result.coinsGained, result.itemDroppedId, result.itemDroppedQuantity));
      }
      else if (result.attacked)
      {
        AddEntry($"Hit for {result.damageDealt} damage.");
      }
    }

    public void AddGatheringResult(GatheringTickResult result)
    {
      if (result == null)
      {
        return;
      }

      if (!string.IsNullOrEmpty(result.failureReason))
      {
        AddEntry(result.failureReason);
        return;
      }

      if (result.completed)
      {
        AddEntry(FormatRewardLine("Gathered", result.xpGained, 0, result.itemGainedId, result.itemGainedQuantity));
      }
    }

    public void AddCraftingResult(CraftingResult result)
    {
      if (result == null)
      {
        return;
      }

      if (result.success)
      {
        AddEntry($"Crafted {FormatItem(result.outputItemId, result.outputQuantity)}.");
      }
      else
      {
        AddEntry(string.IsNullOrEmpty(result.failureReason) ? "Crafting failed." : result.failureReason);
      }
    }

    public void AddQuestClaimResult(QuestClaimResult result)
    {
      if (result == null)
      {
        return;
      }

      if (result.success)
      {
        AddEntry(FormatRewardLine("Quest reward", result.xpReward, result.coinsReward, result.rewardItemId, result.rewardItemQuantity));
      }
      else
      {
        AddEntry(string.IsNullOrEmpty(result.failureReason) ? "Quest reward unavailable." : result.failureReason);
      }
    }

    public void AddAfkRewards(AfkRewardSummary summary)
    {
      if (summary == null || !summary.hasAnyRewards)
      {
        AddEntry("No AFK rewards yet.");
        return;
      }

      foreach (CharacterAfkRewardSummary characterReward in summary.characterRewards)
      {
        if (characterReward == null)
        {
          continue;
        }

        if (!characterReward.hadRewards)
        {
          continue;
        }

        string label = string.IsNullOrEmpty(characterReward.characterName)
          ? "AFK rewards"
          : $"{characterReward.characterName} AFK";

        AddEntry(FormatRewardLine(label, characterReward.xpGained, characterReward.coinsGained, string.Empty, 0));

        foreach (IdleGuildDemo.Runtime.InventoryStack item in characterReward.itemsGained)
        {
          if (item != null)
          {
            AddEntry(FormatRewardLine(label, 0, 0, item.itemId, item.quantity));
          }
        }
      }
    }

    public void AddCoinChange(int coins, string reason)
    {
      if (coins == 0)
      {
        return;
      }

      AddEntry($"{reason}: {FormatSigned(coins)} coins.");
    }

    public void AddItemChange(string itemId, int quantity, string reason)
    {
      if (string.IsNullOrEmpty(itemId) || quantity == 0)
      {
        return;
      }

      AddEntry($"{reason}: {FormatItem(itemId, quantity)}.");
    }

    public void Clear()
    {
      _lines.Clear();
      if (logText != null)
      {
        logText.text = string.Empty;
      }
    }

    private static string FormatRewardLine(string label, int xp, int coins, string itemId, int itemQuantity)
    {
      System.Collections.Generic.List<string> parts = new();
      if (xp > 0)
      {
        parts.Add($"+{xp} XP");
      }

      if (coins > 0)
      {
        parts.Add($"+{coins} coins");
      }

      if (!string.IsNullOrEmpty(itemId) && itemQuantity > 0)
      {
        parts.Add(FormatItem(itemId, itemQuantity));
      }

      string prefix = string.IsNullOrEmpty(label) ? "Reward" : label;
      return parts.Count > 0 ? $"{prefix}: {string.Join(", ", parts)}." : $"{prefix}.";
    }

    private static string FormatItem(string itemId, int quantity)
    {
      string name = FormatItemName(itemId);
      if (quantity == 1)
      {
        return $"+{name}";
      }

      if (quantity == -1)
      {
        return $"-{name}";
      }

      return quantity > 0 ? $"+{quantity} {name}" : $"{quantity} {name}";
    }

    private static string FormatItemName(string itemId)
    {
      if (string.IsNullOrEmpty(itemId))
      {
        return "item";
      }

      string spaced = itemId.Replace("item.", string.Empty).Replace("_", " ");
      return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(spaced);
    }

    private static string FormatSigned(int amount)
    {
      return amount > 0 ? $"+{amount}" : amount.ToString();
    }
  }
}
