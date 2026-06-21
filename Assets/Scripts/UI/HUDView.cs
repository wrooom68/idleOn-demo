using UnityEngine;
using UnityEngine.UI;
using IdleGuildDemo.Core;
using IdleGuildDemo.Runtime;
using IdleGuildDemo.Systems;

namespace IdleGuildDemo.UI
{
  public class HUDView : MonoBehaviour
  {
    [SerializeField] private Text coinsText;
    [SerializeField] private Text zoneNameText;
    [SerializeField] private Text inventorySummaryText;
    [SerializeField] private Text activeCharacterText;
    [SerializeField] private Text xpText;
    [SerializeField] private ProgressBarView xpBar;
    [SerializeField] private QuestTrackerView questTrackerView;

    public void RefreshFromServices(ServiceRegistry services, string zoneName = null)
    {
      if (services == null || !services.IsInitialized)
      {
        SetActiveCharacter(string.Empty);
        RefreshEconomy(null);
        questTrackerView?.Clear();
        return;
      }

      if (!string.IsNullOrEmpty(zoneName))
      {
        SetZoneName(zoneName);
      }

      CharacterState character = services.CharacterRosterSystem.GetActiveCharacter();
      ProgressionSystem progressionSystem = services.ProgressionSystem;
      RefreshCharacter(character, progressionSystem);
      RefreshEconomy(services.PlayerProfile);
    }

    public void RefreshQuest(
      QuestSystem questSystem,
      System.Collections.Generic.IReadOnlyList<IdleGuildDemo.Data.QuestDefinition> questDefinitions,
      PlayerProfile profile)
    {
      questTrackerView?.RefreshFromQuestSystem(questSystem, questDefinitions, profile);
    }

    public void RefreshCharacter(CharacterState character, ProgressionSystem progressionSystem)
    {
      if (character == null)
      {
        SetActiveCharacter(string.Empty);
        SetXp(0, 1);
        return;
      }

      character.Normalize();
      SetActiveCharacter($"{character.displayName}  Lv {character.level}");
      int requiredXp = progressionSystem != null ? progressionSystem.GetXpRequiredForLevel(character.level) : 1;
      SetXp(character.currentXp, requiredXp);
    }

    public void RefreshEconomy(PlayerProfile profile)
    {
      if (profile == null)
      {
        SetCoins(0);
        SetInventorySummary(string.Empty);
        return;
      }

      profile.Normalize();
      SetCoins(profile.coins);
      SetInventorySummary($"{profile.inventory.stacks.Count} item stacks");
    }

    public void SetActiveCharacter(string label)
    {
      if (activeCharacterText != null)
      {
        activeCharacterText.text = label ?? string.Empty;
      }
    }

    public void SetXp(int currentXp, int requiredXp)
    {
      int safeRequired = requiredXp < 1 ? 1 : requiredXp;
      int safeCurrent = Mathf.Clamp(currentXp, 0, safeRequired);
      if (xpText != null)
      {
        xpText.text = $"XP: {safeCurrent}/{safeRequired}";
      }

      if (xpBar != null)
      {
        xpBar.SetNormalizedValue((float)safeCurrent / safeRequired);
        xpBar.SetLabel($"{safeCurrent}/{safeRequired} XP");
      }
    }

    public void SetCoins(int coins)
    {
      if (coinsText != null)
      {
        coinsText.text = $"Coins: {coins}";
      }
    }

    public void SetZoneName(string zoneName)
    {
      if (zoneNameText != null)
      {
        zoneNameText.text = zoneName;
      }
    }

    public void SetInventorySummary(string summary)
    {
      if (inventorySummaryText != null)
      {
        inventorySummaryText.text = summary;
      }
    }
  }
}
