using UnityEngine;
using UnityEngine.UI;

namespace IdleGuild.UI
{
  public class TalentNodeView : MonoBehaviour
  {
    [SerializeField] private Text talentNameText;
    [SerializeField] private Text descriptionText;
    [SerializeField] private Text rankText;
    [SerializeField] private Button upgradeButton;

    public Button UpgradeButton => upgradeButton;

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
  }
}
