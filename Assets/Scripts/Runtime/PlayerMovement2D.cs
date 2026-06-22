using UnityEngine;
using UnityEngine.SceneManagement;
using IdleGuildDemo.UI;

namespace IdleGuildDemo.Runtime
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public sealed class PlayerMovement2D : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float jumpForce = 15f;
        [SerializeField] private CombatHUDView combatHUDView;
        [SerializeField] private MiningHUDView miningHUDView;

        private Rigidbody2D rb;
        private Collider2D col;
        private Animator animator;
        private SpriteRenderer spriteRenderer;

        private bool isGrounded;
        
        // Attack/Mining state variables
        private float attackTimer = 0f;
        private float attackDuration = 0.5f; // half a second animation swing
        private string activeAttackAnimation = "Pawn_Attack";

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            col = GetComponent<Collider2D>();
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();

            if (combatHUDView == null)
            {
                combatHUDView = GameObject.FindFirstObjectByType<CombatHUDView>();
            }

            if (miningHUDView == null)
            {
                miningHUDView = GameObject.FindFirstObjectByType<MiningHUDView>();
            }
        }

        private void Update()
        {
            // Ground check
            isGrounded = CheckGrounded();

            // Horizontal input
            float input = Input.GetAxisRaw("Horizontal");
            rb.linearVelocity = new Vector2(input * moveSpeed, rb.linearVelocity.y);

            // Sprite flipping
            if (input < -0.1f)
            {
                spriteRenderer.flipX = true;
            }
            else if (input > 0.1f)
            {
                spriteRenderer.flipX = false;
            }

            // Jump input
            if (isGrounded && Input.GetKeyDown(KeyCode.Space))
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }

            // Attack/Mine input trigger
            if (attackTimer > 0f)
            {
                attackTimer -= Time.deltaTime;
            }
            else
            {
                // Trigger attack on left click, left Ctrl, F or Enter key
                if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Return))
                {
                    attackTimer = attackDuration;
                    
                    // Determine animation type based on scene
                    string sceneName = SceneManager.GetActiveScene().name;
                    if (sceneName == "MineZone")
                    {
                        activeAttackAnimation = "Pawn_Mine";

                        if (miningHUDView == null)
                        {
                            miningHUDView = GameObject.FindFirstObjectByType<MiningHUDView>();
                        }
                        if (miningHUDView != null)
                        {
                            var nodes = GameObject.FindObjectsByType<MiningNodeController>(FindObjectsSortMode.None);
                            MiningNodeController nearestNode = null;
                            float nearestDistance = float.MaxValue;
                            foreach (var n in nodes)
                            {
                                if (n.IsMined) continue;
                                float dist = Vector2.Distance(transform.position, n.transform.position);
                                if (dist < nearestDistance)
                                {
                                    nearestDistance = dist;
                                    nearestNode = n;
                                }
                            }

                            if (nearestNode != null && nearestDistance <= 2.2f)
                            {
                                nearestNode.OnMineByPlayer(miningHUDView);
                            }
                        }
                    }
                    else
                    {
                        activeAttackAnimation = "Pawn_Attack";

                        if (sceneName == "CombatZone")
                        {
                            if (combatHUDView == null)
                            {
                                combatHUDView = GameObject.FindFirstObjectByType<CombatHUDView>();
                            }
                            if (combatHUDView != null)
                            {
                                var slimes = GameObject.FindObjectsByType<SlimeFightController>(FindObjectsSortMode.None);
                                SlimeFightController nearestSlime = null;
                                float nearestDistance = float.MaxValue;
                                foreach (var s in slimes)
                                {
                                    if (s.IsDying) continue;
                                    float dist = Vector2.Distance(transform.position, s.transform.position);
                                    if (dist < nearestDistance)
                                    {
                                        nearestDistance = dist;
                                        nearestSlime = s;
                                    }
                                }

                                if (nearestSlime != null && nearestDistance <= 2.2f)
                                {
                                    nearestSlime.OnHitByPlayer(combatHUDView);
                                }
                            }
                        }
                    }
                }
            }

            // Animation priority system
            if (animator != null)
            {
                if (attackTimer > 0f)
                {
                    animator.Play(activeAttackAnimation);
                }
                else if (!isGrounded)
                {
                    animator.Play("Pawn_Jump");
                }
                else if (Mathf.Abs(rb.linearVelocity.x) > 0.1f)
                {
                    animator.Play("Run");
                }
                else
                {
                    animator.Play("Idle");
                }
            }
        }

        private bool CheckGrounded()
        {
            // Fire a short raycast downwards from the bottom edge of our collider
            Bounds bounds = col.bounds;
            Vector2 origin = new Vector2(bounds.center.x, bounds.min.y - 0.05f);
            RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 0.1f);
            
            // If we hit any collider that is not ourselves, we are grounded!
            if (hit.collider != null && hit.collider != col)
            {
                // Verify we are not hitting trigger zones or the player's own parts
                if (!hit.collider.isTrigger)
                {
                    return true;
                }
            }
            return false;
        }
    }
}