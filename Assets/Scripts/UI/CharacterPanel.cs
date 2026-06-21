using System.Collections.Generic;
using IdleGuildDemo.Core;
using IdleGuildDemo.Runtime;
using IdleGuildDemo.Systems;
using UnityEngine;
using UnityEngine.UI;

namespace IdleGuildDemo.UI
{
  public class CharacterPanel : UIPanel
  {
    [SerializeField] private Transform cardContainer;
    [SerializeField] private CharacterCardView cardPrefab;
    [SerializeField] private Text rosterSummaryText;

    public Transform CardContainer => cardContainer;
    public CharacterCardView CardPrefab => cardPrefab;

    private readonly List<CharacterCardView> _cards = new List<CharacterCardView>();
    private readonly ClassSelectionSystem _classSelectionSystem = new ClassSelectionSystem();

    public void RefreshFromServices(ServiceRegistry services)
    {
      if (services == null || !services.IsInitialized)
      {
        ClearCards();
        SetRosterSummary("Roster unavailable.");
        return;
      }

      RefreshRoster(
        services.CharacterRosterSystem.GetUnlockedCharacters(),
        services.ProgressionSystem);
    }

    public void RefreshRoster(
      IReadOnlyList<CharacterState> characters,
      ProgressionSystem progressionSystem)
    {
      int count = characters != null ? characters.Count : 0;
      EnsureCardCount(count);

      for (int i = 0; i < _cards.Count; i++)
      {
        CharacterCardView card = _cards[i];
        if (card == null)
        {
          continue;
        }

        if (characters == null || i >= characters.Count)
        {
          card.Clear();
          continue;
        }

        CharacterState character = characters[i];
        character?.Normalize();
        if (character == null)
        {
          card.Clear();
          continue;
        }

        int targetXp = progressionSystem != null
          ? progressionSystem.GetXpRequiredForLevel(character.level)
          : 1;

        card.SetCharacter(
          null,
          string.IsNullOrEmpty(character.displayName) ? character.characterId : character.displayName,
          character.level,
          character.currentXp,
          targetXp,
          FormatTask(character.currentTask),
          _classSelectionSystem.GetClassDisplayName(_classSelectionSystem.GetCurrentClassId(character)),
          character.unspentTalentPoints);
      }

      SetRosterSummary($"{count} character{(count == 1 ? string.Empty : "s")} unlocked");
    }

    public void SetRosterSummary(string summary)
    {
      if (rosterSummaryText != null)
      {
        rosterSummaryText.text = summary;
      }
    }

    public void ClearCards()
    {
      for (int i = 0; i < _cards.Count; i++)
      {
        if (_cards[i] != null)
        {
          _cards[i].Clear();
        }
      }
    }

    private void EnsureCardCount(int count)
    {
      if (cardContainer == null || cardPrefab == null)
      {
        return;
      }

      while (_cards.Count < count)
      {
        _cards.Add(Instantiate(cardPrefab, cardContainer));
      }
    }

    private static string FormatTask(TaskState task)
    {
      if (task == null || string.IsNullOrEmpty(task.taskType) || task.taskType == GameConstants.TaskIdle)
      {
        return "Idle";
      }

      if (task.taskType == GameConstants.TaskCombat && task.targetId == GameConstants.EnemySlimeId)
      {
        return "Fighting Slimes";
      }

      if (task.taskType == GameConstants.TaskMining && task.targetId == GameConstants.ZoneMineCopperId)
      {
        return "Mining Copper";
      }

      return task.taskType;
    }
  }
}
