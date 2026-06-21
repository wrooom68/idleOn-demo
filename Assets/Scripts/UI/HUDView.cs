using UnityEngine;
using UnityEngine.UI;
using IdleGuildDemo.Runtime;

namespace IdleGuildDemo.UI
{
  public class HUDView : MonoBehaviour
  {
    [SerializeField] private Text coinsText;
    [SerializeField] private Text zoneNameText;
    [SerializeField] private Text inventorySummaryText;

    public void RefreshEconomy(PlayerProfile profile)
    {
      if (profile == null)
      {
        SetCoins(0);
        SetInventorySummary(string.Empty);
        return;
      }

      profile.Normalize();
      SetCoins(profile.coins);
      SetInventorySummary($"{profile.inventory.stacks.Count} item stacks");
    }

    public void SetCoins(int coins)
    {
      if (coinsText != null)
      {
        coinsText.text = $"Coins: {coins}";
      }
    }

    public void SetZoneName(string zoneName)
    {
      if (zoneNameText != null)
      {
        zoneNameText.text = zoneName;
      }
    }

    public void SetInventorySummary(string summary)
    {
      if (inventorySummaryText != null)
      {
        inventorySummaryText.text = summary;
      }
    }
  }
}
