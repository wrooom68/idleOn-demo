using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IdleGuildDemo.Core;
using IdleGuildDemo.Runtime;
using IdleGuildDemo.Systems;

namespace IdleGuildDemo.UI
{
  public class TalentPanel : UIPanel
  {
    [SerializeField] private Transform talentContainer;
    [SerializeField] private TalentNodeView talentNodePrefab;
    [SerializeField] private Text availablePointsText;
    [SerializeField] private Text statusText;

    public Transform TalentContainer => talentContainer;
    public TalentNodeView TalentNodePrefab => talentNodePrefab;

    public event System.Action<TalentSpendResult> TalentSpendCompleted;

    private static readonly string[] TalentIds =
    {
      GameConstants.DamageTalentId,
      GameConstants.MiningSpeedTalentId,
      GameConstants.XpGainTalentId,
      GameConstants.AfkGainTalentId
    };

    private readonly List<TalentNodeView> _spawnedNodes = new List<TalentNodeView>();
    private CharacterState _character;
    private TalentSystem _talentSystem;

    public void Bind(CharacterState character, TalentSystem talentSystem)
    {
      _character = character;
      _talentSystem = talentSystem ?? new TalentSystem();
      Refresh();
    }

    public void Refresh()
    {
      TalentSystem talentSystem = _talentSystem ?? new TalentSystem();
      int points = _character != null ? _character.unspentTalentPoints : 0;
      SetAvailablePoints(points);
      RebuildTalentNodes(talentSystem);

      if (_character == null)
      {
        SetStatus("No character selected.");
      }
      else if (points > 0)
      {
        SetStatus("Spend talent points to improve this character.");
      }
      else
      {
        SetStatus("Gain levels to earn talent points.");
      }
    }

    public void SetAvailablePoints(int points)
    {
      if (availablePointsText != null)
      {
        availablePointsText.text = $"Talent Points: {points}";
      }
    }

    public void SetStatus(string status)
    {
      if (statusText != null)
      {
        statusText.text = status;
      }
    }

    public TalentSpendResult SpendDamageTalent()
    {
      return SpendTalentPoint(GameConstants.DamageTalentId);
    }

    public TalentSpendResult SpendMiningSpeedTalent()
    {
      return SpendTalentPoint(GameConstants.MiningSpeedTalentId);
    }

    public TalentSpendResult SpendXpGainTalent()
    {
      return SpendTalentPoint(GameConstants.XpGainTalentId);
    }

    public TalentSpendResult SpendAfkGainTalent()
    {
      return SpendTalentPoint(GameConstants.AfkGainTalentId);
    }

    public TalentSpendResult SpendTalentPoint(string talentId)
    {
      TalentSystem talentSystem = _talentSystem ?? new TalentSystem();
      TalentSpendResult result = talentSystem.SpendTalentPoint(_character, talentId);
      Refresh();

      if (result.success)
      {
        SetStatus($"{talentSystem.GetTalentDisplayName(talentId)} increased to rank {result.newRank}.");
      }
      else
      {
        SetStatus(result.failureReason);
      }

      TalentSpendCompleted?.Invoke(result);
      return result;
    }

    private void RebuildTalentNodes(TalentSystem talentSystem)
    {
      for (int i = _spawnedNodes.Count - 1; i >= 0; i--)
      {
        if (_spawnedNodes[i] != null)
        {
          Destroy(_spawnedNodes[i].gameObject);
        }
      }

      _spawnedNodes.Clear();

      if (talentContainer == null || talentNodePrefab == null)
      {
        return;
      }

      foreach (string talentId in TalentIds)
      {
        TalentNodeView node = Instantiate(talentNodePrefab, talentContainer);
        node.SetTalentId(talentId);
        node.SetTalent(
          talentSystem.GetTalentDisplayName(talentId),
          GetTalentDescription(talentId),
          talentSystem.GetTalentRank(_character, talentId),
          TalentSystem.MaxTalentRank,
          talentSystem.CanSpendTalentPoint(_character, talentId));
        node.BindUpgrade(talentId =>
        {
          SpendTalentPoint(talentId);
        });
        _spawnedNodes.Add(node);
      }
    }

    private static string GetTalentDescription(string talentId)
    {
      switch (talentId)
      {
        case GameConstants.DamageTalentId:
          return $"+{TalentSystem.DamagePerRank} damage per rank.";
        case GameConstants.MiningSpeedTalentId:
          return $"+{TalentSystem.MiningSpeedPerRank:P0} mining speed per rank.";
        case GameConstants.XpGainTalentId:
          return $"+{TalentSystem.XpGainPerRank:P0} XP gain per rank.";
        case GameConstants.AfkGainTalentId:
          return $"+{TalentSystem.AfkGainPerRank:P0} AFK gain per rank.";
        default:
          return string.Empty;
      }
    }
  }
}
