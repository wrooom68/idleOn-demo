using UnityEngine;
using UnityEngine.UI;

namespace IdleGuildDemo.UI
{
    /// <summary>
    /// Thin placeholder view for reusable UI progress bars.
    /// </summary>
    public sealed class ProgressBarView : MonoBehaviour
    {
        [SerializeField] private Image fillImage;
        [SerializeField] private Text labelText;

        public void SetNormalizedValue(float value)
        {
            if (fillImage != null)
            {
                fillImage.fillAmount = Mathf.Clamp01(value);
            }
        }

        public void SetLabel(string label)
        {
            if (labelText != null)
            {
                labelText.text = label;
            }
        }
    }
}
