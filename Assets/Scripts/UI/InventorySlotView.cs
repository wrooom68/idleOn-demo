using UnityEngine;
using UnityEngine.UI;

namespace IdleGuildDemo.UI
{
  public class InventorySlotView : MonoBehaviour
  {
    [SerializeField] private Image iconImage;
    [SerializeField] private Text quantityText;
    [SerializeField] private Image backgroundImage;

    public void SetEmpty()
    {
      if (iconImage != null)
      {
        iconImage.enabled = false;
      }

      if (quantityText != null)
      {
        quantityText.text = string.Empty;
      }
    }

    public void SetItem(Sprite icon, int quantity, string displayName)
    {
      if (iconImage != null)
      {
        iconImage.enabled = icon != null;
        iconImage.sprite = icon;
      }

      if (quantityText != null)
      {
        quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty;
      }

      name = displayName;
    }
  }
}
