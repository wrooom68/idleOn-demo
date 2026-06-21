using UnityEngine;
using UnityEngine.UI;

namespace IdleGuildDemo.UI
{
    /// <summary>
    /// Thin placeholder view for automatic combat HUD state.
    /// </summary>
    public sealed class CombatHUDView : MonoBehaviour
    {
        [SerializeField] private Button backToTownButton;
        [SerializeField] private Image enemyHpFill;

        // TODO: Render combat progress, enemy HP, quest state, and loot log entries.
        // TODO: Keep combat rules in CombatSystem.
    }
}
