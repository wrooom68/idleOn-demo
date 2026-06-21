using UnityEngine;
using UnityEngine.UI;

namespace IdleGuild.UI
{
  public class MainMenuView : MonoBehaviour
  {
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Text evaluationNoteText;

    public Button NewGameButton => newGameButton;
    public Button ContinueButton => continueButton;

    public void SetContinueAvailable(bool available)
    {
      if (continueButton != null)
      {
        continueButton.interactable = available;
      }
    }

    public void SetEvaluationNote(string note)
    {
      if (evaluationNoteText != null)
      {
        evaluationNoteText.text = note;
      }
    }
  }
}
