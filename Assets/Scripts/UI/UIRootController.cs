using UnityEngine;

namespace IdleGuild.UI
{
    /// Routes navigation-bar clicks to panels. Does not own gameplay rules.
    public class UIRootController : MonoBehaviour
    {
        [SerializeField] private UIPanel inventoryPanel;
        [SerializeField] private UIPanel craftingPanel;
        [SerializeField] private UIPanel characterPanel;
        [SerializeField] private UIPanel talentPanel;
        [SerializeField] private UIPanel classChoicePanel;
        [SerializeField] private AfkResultsModal afkResultsModal;

        private UIPanel _openPanel;

        public void ShowInventory() => TogglePanel(inventoryPanel);
        public void ShowCrafting() => TogglePanel(craftingPanel);
        public void ShowCharacters() => TogglePanel(characterPanel);
        public void ShowTalents() => TogglePanel(talentPanel);
        public void ShowClassChoice() => ShowExclusive(classChoicePanel);

        public void CloseAllPanels()
        {
            inventoryPanel?.Hide();
            craftingPanel?.Hide();
            characterPanel?.Hide();
            talentPanel?.Hide();
            classChoicePanel?.Hide();
            _openPanel = null;
        }

        public void ShowAfkResults() => afkResultsModal?.Show();

        private void TogglePanel(UIPanel panel)
        {
            if (panel == null)
            {
                return;
            }

            if (_openPanel == panel && panel.IsVisible)
            {
                panel.Hide();
                _openPanel = null;
                return;
            }

            ShowExclusive(panel);
        }

        private void ShowExclusive(UIPanel panel)
        {
            CloseAllPanels();
            panel?.Show();
            _openPanel = panel;
        }
    }
}
