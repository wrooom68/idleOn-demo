using UnityEngine;
using UnityEngine.UI;

namespace IdleGuildDemo.UI
{
  public class TalentPanel : UIPanel
  {
    [SerializeField] private Transform talentContainer;
    [SerializeField] private TalentNodeView talentNodePrefab;
    [SerializeField] private Text availablePointsText;

    public Transform TalentContainer => talentContainer;
    public TalentNodeView TalentNodePrefab => talentNodePrefab;

    public void SetAvailablePoints(int points)
    {
      if (availablePointsText != null)
      {
        availablePointsText.text = $"Talent Points: {points}";
      }
    }
  }
}
