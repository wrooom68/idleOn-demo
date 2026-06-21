using UnityEngine;
using UnityEngine.UI;

namespace IdleGuildDemo.UI
{
  public class CharacterCardView : MonoBehaviour
  {
    [SerializeField] private Image portraitImage;
    [SerializeField] private Text nameText;
    [SerializeField] private Text levelText;
    [SerializeField] private Text xpText;
    [SerializeField] private Image xpFill;
    [SerializeField] private Text taskText;
    [SerializeField] private Text classText;
    [SerializeField] private Text talentPointsText;

    public void Clear()
    {
      SetCharacter(null, string.Empty, 1, 0, 1, string.Empty, string.Empty, 0);
    }

    public void SetCharacter(
      Sprite portrait,
      string characterName,
      int level,
      int currentXp,
      int targetXp,
      string currentTask,
      string className,
      int talentPoints)
    {
      if (portraitImage != null)
      {
        portraitImage.sprite = portrait;
        portraitImage.enabled = portrait != null;
      }

      if (nameText != null)
      {
        nameText.text = characterName;
      }

      if (levelText != null)
      {
        levelText.text = $"Lv {level}";
      }

      if (xpText != null)
      {
        xpText.text = $"{currentXp}/{targetXp} XP";
      }

      if (xpFill != null && targetXp > 0)
      {
        xpFill.fillAmount = Mathf.Clamp01((float)currentXp / targetXp);
      }

      if (taskText != null)
      {
        taskText.text = currentTask;
      }

      if (classText != null)
      {
        classText.text = className;
      }

      if (talentPointsText != null)
      {
        talentPointsText.text = $"Talents: {talentPoints}";
      }
    }
  }
}
