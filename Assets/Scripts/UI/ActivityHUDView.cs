using UnityEngine;
using UnityEngine.UI;

namespace IdleGuildDemo.UI
{
  public class ActivityHUDView : MonoBehaviour
  {
    [SerializeField] private GameObject miningGroup;
    [SerializeField] private Image miningProgressFill;
    [SerializeField] private Text miningLabelText;

    [SerializeField] private GameObject combatGroup;
    [SerializeField] private Image enemyHpFill;
    [SerializeField] private Text enemyNameText;

    public void ShowMining(bool visible)
    {
      if (miningGroup != null)
      {
        miningGroup.SetActive(visible);
      }
    }

    public void SetMiningProgress(float normalized, string label)
    {
      if (miningProgressFill != null)
      {
        miningProgressFill.fillAmount = Mathf.Clamp01(normalized);
      }

      if (miningLabelText != null)
      {
        miningLabelText.text = label;
      }
    }

    public void ShowCombat(bool visible)
    {
      if (combatGroup != null)
      {
        combatGroup.SetActive(visible);
      }
    }

    public void SetEnemyHealth(float normalized, string enemyName)
    {
      if (enemyHpFill != null)
      {
        enemyHpFill.fillAmount = Mathf.Clamp01(normalized);
      }

      if (enemyNameText != null)
      {
        enemyNameText.text = enemyName;
      }
    }
  }
}
