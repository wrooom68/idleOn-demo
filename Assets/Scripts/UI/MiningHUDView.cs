using UnityEngine;
using UnityEngine.UI;

namespace IdleGuildDemo.UI
{
    /// <summary>
    /// Thin placeholder view for mining HUD state.
    /// </summary>
    public sealed class MiningHUDView : MonoBehaviour
    {
        [SerializeField] private Button backToTownButton;
        [SerializeField] private Image miningProgressFill;

        // TODO: Render gathering progress and recent rewards from systems.
        // TODO: Keep gathering rules in GatheringSystem.
    }
}
