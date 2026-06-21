using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace IdleGuild.UI
{
  public class ToastView : MonoBehaviour
  {
    [SerializeField] private Text toastText;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float displayDuration = 2.5f;

    private Coroutine _hideRoutine;

    public void Show(string message)
    {
      if (toastText != null)
      {
        toastText.text = message;
      }

      if (canvasGroup != null)
      {
        canvasGroup.alpha = 1f;
      }

      if (_hideRoutine != null)
      {
        StopCoroutine(_hideRoutine);
      }

      _hideRoutine = StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
      yield return new WaitForSeconds(displayDuration);
      if (canvasGroup != null)
      {
        canvasGroup.alpha = 0f;
      }

      _hideRoutine = null;
    }
  }
}
