using UnityEngine;
using UnityEngine.UI;

namespace IdleGuildDemo.UI
{
  public class QuestTrackerView : MonoBehaviour
  {
    [SerializeField] private Text questTitleText;
    [SerializeField] private Text questObjectiveText;
    [SerializeField] private Image progressFill;
    [SerializeField] private Text progressText;

    public void SetQuest(string title, string objective, int current, int target)
    {
      if (questTitleText != null)
      {
        questTitleText.text = title;
      }

      if (questObjectiveText != null)
      {
        questObjectiveText.text = objective;
      }

      if (progressText != null)
      {
        progressText.text = $"{current}/{target}";
      }

      if (progressFill != null && target > 0)
      {
        progressFill.fillAmount = Mathf.Clamp01((float)current / target);
      }
    }

    public void Clear()
    {
      if (questTitleText != null)
      {
        questTitleText.text = "No active quest";
      }

      if (questObjectiveText != null)
      {
        questObjectiveText.text = string.Empty;
      }

      if (progressText != null)
      {
        progressText.text = string.Empty;
      }

      if (progressFill != null)
      {
        progressFill.fillAmount = 0f;
      }
    }
  }
}
