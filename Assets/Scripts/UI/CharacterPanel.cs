using UnityEngine;
using UnityEngine.UI;

namespace IdleGuild.UI
{
  public class CharacterPanel : UIPanel
  {
    [SerializeField] private Transform cardContainer;
    [SerializeField] private CharacterCardView cardPrefab;
    [SerializeField] private Text rosterSummaryText;

    public Transform CardContainer => cardContainer;
    public CharacterCardView CardPrefab => cardPrefab;

    public void SetRosterSummary(string summary)
    {
      if (rosterSummaryText != null)
      {
        rosterSummaryText.text = summary;
      }
    }
  }
}
