using UnityEngine;
using UnityEngine.UI;

namespace IdleGuild.UI
{
  public class HUDView : MonoBehaviour
  {
    [SerializeField] private Text coinsText;
    [SerializeField] private Text zoneNameText;

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
  }
}
