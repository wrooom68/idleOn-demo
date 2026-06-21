using UnityEngine;
using UnityEngine.UI;

namespace IdleGuildDemo.UI
{
  public class ClassChoicePanel : UIPanel
  {
    [SerializeField] private Button warriorButton;
    [SerializeField] private Button archerButton;
    [SerializeField] private Button mageButton;
    [SerializeField] private Text promptText;

    public Button WarriorButton => warriorButton;
    public Button ArcherButton => archerButton;
    public Button MageButton => mageButton;

    public void SetPrompt(string prompt)
    {
      if (promptText != null)
      {
        promptText.text = prompt;
      }
    }
  }
}
