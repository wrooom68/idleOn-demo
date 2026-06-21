using UnityEngine;

namespace IdleGuildDemo.UI
{
  /// Wires navigation-bar button clicks to UIRootController at runtime.
  public class UIWiring : MonoBehaviour
  {
    [SerializeField] private UIRootController root;
    [SerializeField] private NavigationBarView navigationBar;
    [SerializeField] private TownHUDView townHudView;
    [SerializeField] private AfkResultsModal afkResultsModal;

    private void OnEnable()
    {
      if (!Application.isPlaying)
      {
        return;
      }

      if (root != null && navigationBar != null)
      {
        navigationBar.Bind(root, townHudView);
        navigationBar.SetActiveView("Town");
      }

      if (afkResultsModal != null && afkResultsModal.CloseButton != null)
      {
        afkResultsModal.CloseButton.onClick.AddListener(afkResultsModal.Hide);
      }
    }

    private void OnDisable()
    {
      if (!Application.isPlaying)
      {
        return;
      }

      navigationBar?.Unbind();

      if (afkResultsModal != null && afkResultsModal.CloseButton != null)
      {
        afkResultsModal.CloseButton.onClick.RemoveListener(afkResultsModal.Hide);
      }
    }
  }
}
