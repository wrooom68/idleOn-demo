using UnityEngine;
using UnityEngine.UI;
using IdleGuildDemo.Core;
using IdleGuildDemo.Runtime;
using IdleGuildDemo.Systems;

namespace IdleGuildDemo.UI
{
  public class ClassChoicePanel : UIPanel
  {
    [SerializeField] private Button warriorButton;
    [SerializeField] private Button archerButton;
    [SerializeField] private Button mageButton;
    [SerializeField] private Text promptText;
    [SerializeField] private Text currentClassText;
    [SerializeField] private Text statusText;

    public Button WarriorButton => warriorButton;
    public Button ArcherButton => archerButton;
    public Button MageButton => mageButton;

    public event System.Action<ClassSelectionResult> ClassSelectionCompleted;

    private CharacterState _character;
    private ClassSelectionSystem _classSelectionSystem;

    private void Awake()
    {
      BindButton(warriorButton, GameConstants.WarriorClassId);
      BindButton(archerButton, GameConstants.ArcherClassId);
      BindButton(mageButton, GameConstants.MageClassId);
    }

    public void Bind(CharacterState character, ClassSelectionSystem classSelectionSystem)
    {
      _character = character;
      _classSelectionSystem = classSelectionSystem ?? new ClassSelectionSystem();
      Refresh();
    }

    public void Refresh()
    {
      ClassSelectionSystem classSystem = _classSelectionSystem ?? new ClassSelectionSystem();
      string currentClass = classSystem.GetClassDisplayName(classSystem.GetCurrentClassId(_character));
      bool canChoose = classSystem.CanChooseClass(_character);

      if (currentClassText != null)
      {
        currentClassText.text = $"Current Class: {currentClass}";
      }

      SetClassButtonsInteractable(canChoose);

      if (_character == null)
      {
        SetPrompt("No character selected.");
        SetStatus(string.Empty);
      }
      else if (canChoose)
      {
        SetPrompt("Choose a class.");
        SetStatus("Warrior: damage | Archer: drops | Mage: AFK gains");
      }
      else if (classSystem.HasChosenSpecializedClass(_character))
      {
        SetPrompt($"{currentClass} selected.");
        SetStatus("Class choice is locked in.");
      }
      else
      {
        SetPrompt($"Class unlocks at level {GameConstants.ClassUnlockLevel}.");
        SetStatus($"Current level: {_character.level}");
      }
    }

    public void SetPrompt(string prompt)
    {
      if (promptText != null)
      {
        promptText.text = prompt;
      }
    }

    public void SetStatus(string status)
    {
      if (statusText != null)
      {
        statusText.text = status;
      }
    }

    public ClassSelectionResult ChooseWarrior()
    {
      return ChooseClass(GameConstants.WarriorClassId);
    }

    public ClassSelectionResult ChooseArcher()
    {
      return ChooseClass(GameConstants.ArcherClassId);
    }

    public ClassSelectionResult ChooseMage()
    {
      return ChooseClass(GameConstants.MageClassId);
    }

    public ClassSelectionResult ChooseClass(string classId)
    {
      ClassSelectionSystem classSystem = _classSelectionSystem ?? new ClassSelectionSystem();
      ClassSelectionResult result = classSystem.ChooseClass(_character, classId);
      Refresh();

      if (result.success)
      {
        SetStatus($"{result.selectedClassDisplayName} selected.");
      }
      else
      {
        SetStatus(result.failureReason);
      }

      ClassSelectionCompleted?.Invoke(result);
      return result;
    }

    private void BindButton(Button button, string classId)
    {
      if (button == null)
      {
        return;
      }

      button.onClick.AddListener(() => ChooseClass(classId));
    }

    private void SetClassButtonsInteractable(bool interactable)
    {
      if (warriorButton != null)
      {
        warriorButton.interactable = interactable;
      }

      if (archerButton != null)
      {
        archerButton.interactable = interactable;
      }

      if (mageButton != null)
      {
        mageButton.interactable = interactable;
      }
    }
  }
}
