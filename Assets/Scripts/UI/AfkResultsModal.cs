using UnityEngine;
using UnityEngine.UI;

namespace IdleGuildDemo.UI
{
  public class AfkResultsModal : MonoBehaviour
  {
    [SerializeField] private GameObject root;
    [SerializeField] private Text resultsText;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button simulateTwoHoursButton;

    public Button CloseButton => closeButton;
    public Button SimulateTwoHoursButton => simulateTwoHoursButton;

    public void Show()
    {
      if (root != null)
      {
        root.SetActive(true);
      }
      else
      {
        gameObject.SetActive(true);
      }
    }

    public void Hide()
    {
      if (root != null)
      {
        root.SetActive(false);
      }
      else
      {
        gameObject.SetActive(false);
      }
    }

    public void SetResults(string results)
    {
      if (resultsText != null)
      {
        resultsText.text = results;
      }
    }
  }
}
