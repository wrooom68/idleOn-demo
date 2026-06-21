using UnityEngine;

namespace IdleGuildDemo.Core
{
    /// <summary>
    /// Placeholder MonoBehaviour for future startup wiring and system composition.
    /// </summary>
    public sealed class GameBootstrap : MonoBehaviour
    {
        [SerializeField] private bool bootstrapOnStart = true;

        private void Start()
        {
            if (!bootstrapOnStart)
            {
                return;
            }

            // TODO: Create and register gameplay systems after the architecture is ready.
        }
    }
}
