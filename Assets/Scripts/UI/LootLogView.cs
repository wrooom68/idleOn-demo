using UnityEngine;
using UnityEngine.UI;

namespace IdleGuild.UI
{
  public class LootLogView : MonoBehaviour
  {
    [SerializeField] private Text logText;
    [SerializeField] private int maxLines = 6;

    private readonly System.Collections.Generic.List<string> _lines = new();

    public void AddEntry(string entry)
    {
      _lines.Insert(0, entry);
      while (_lines.Count > maxLines)
      {
        _lines.RemoveAt(_lines.Count - 1);
      }

      if (logText != null)
      {
        logText.text = string.Join("\n", _lines);
      }
    }

    public void Clear()
    {
      _lines.Clear();
      if (logText != null)
      {
        logText.text = string.Empty;
      }
    }
  }
}
