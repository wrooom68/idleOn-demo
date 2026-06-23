using IdleGuildDemo.UI;
using UnityEngine;

namespace IdleGuildDemo.Runtime
{
    public sealed class CopperOrePickup : MonoBehaviour
    {
        private MiningHUDView miningHUDView;
        private MiningNodeController sourceNode;

        public void Initialize(MiningHUDView hudView)
        {
            miningHUDView = hudView;
        }

        public void Initialize(MiningHUDView hudView, MiningNodeController node)
        {
            miningHUDView = hudView;
            sourceNode = node;
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

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.GetComponent<PlayerMovement2D>() != null)
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
                sourceNode?.ResetAfterOreCollected();
                Destroy(gameObject);
            }
        }
    }
}
