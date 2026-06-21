using UnityEngine;
using UnityEngine.UI;

namespace IdleGuildDemo.UI
{
  public class TalentNodeView : MonoBehaviour
  {
    [SerializeField] private Text talentNameText;
    [SerializeField] private Text descriptionText;
    [SerializeField] private Text rankText;
    [SerializeField] private Button upgradeButton;

    public Button UpgradeButton => upgradeButton;
    public string TalentId { get; private set; }

    public void SetTalentId(string talentId)
    {
      TalentId = talentId ?? string.Empty;
    }

    public void SetTalent(string talentName, string description, int rank, int maxRank, bool canUpgrade)
    {
      if (talentNameText != null)
      {
        talentNameText.text = talentName;
      }

      if (descriptionText != null)
      {
        descriptionText.text = description;
      }

      if (rankText != null)
      {
        rankText.text = $"{rank}/{maxRank}";
      }

      if (upgradeButton != null)
      {
        upgradeButton.interactable = canUpgrade;
      }
    }

    public void BindUpgrade(System.Action<string> handler)
    {
      if (upgradeButton == null)
      {
        return;
      }

      upgradeButton.onClick.RemoveAllListeners();
      upgradeButton.onClick.AddListener(() => handler?.Invoke(TalentId));
    }
  }
}
