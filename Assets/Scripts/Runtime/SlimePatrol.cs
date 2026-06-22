using UnityEngine;

namespace IdleGuildDemo.Runtime
{
    public sealed class SlimePatrol : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 1.5f;
        [SerializeField] private float minX = -8f;
        [SerializeField] private float maxX = 8f;
        
        private SpriteRenderer spriteRenderer;
        private bool movingRight = true;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            float currentX = transform.position.x;

            if (movingRight)
            {
                transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
                if (currentX >= maxX)
                {
                    movingRight = false;
                }
            }
            else
            {
                transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
                if (currentX <= minX)
                {
                    movingRight = true;
                }
            }

            // Sprite flipping to face the direction of movement
            if (spriteRenderer != null)
            {
                // If moving left, flipX = true (assuming sprite faces right by default)
                // If moving right, flipX = false
                spriteRenderer.flipX = !movingRight;
            }
        }
    }
}
