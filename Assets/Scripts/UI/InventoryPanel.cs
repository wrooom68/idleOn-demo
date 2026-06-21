using UnityEngine;
using UnityEngine.UI;

namespace IdleGuildDemo.UI
{
  public class InventoryPanel : UIPanel
  {
    [SerializeField] private Transform slotContainer;
    [SerializeField] private InventorySlotView slotPrefab;
    [SerializeField] private Text detailText;

    public Transform SlotContainer => slotContainer;
    public InventorySlotView SlotPrefab => slotPrefab;

    public void SetDetail(string detail)
    {
      if (detailText != null)
      {
        detailText.text = detail;
      }
    }
  }
}
