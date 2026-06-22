using IdleGuildDemo.UI;
using UnityEngine;

namespace IdleGuildDemo.Runtime
{
    public sealed class CopperOrePickup : MonoBehaviour
    {
        private MiningHUDView miningHUDView;

        public void Initialize(MiningHUDView hudView)
        {
            miningHUDView = hudView;
        }

        private void Awake()
        {
            if (miningHUDView == null)
            {
                miningHUDView = GameObject.FindFirstObjectByType<MiningHUDView>();
            }
        }

        private void OnMouseDown()
        {
            Collect();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<PlayerMovement2D>() != null)
            {
                Collect();
            }
        }

        private void Collect()
        {
            if (miningHUDView == null)
            {
                miningHUDView = GameObject.FindFirstObjectByType<MiningHUDView>();
            }

            if (miningHUDView != null && miningHUDView.CollectDroppedCopperOre())
            {
                Destroy(gameObject);
            }
        }
    }
}
