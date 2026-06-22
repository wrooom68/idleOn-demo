using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using IdleGuildDemo.UI;
using IdleGuildDemo.Systems;
using IdleGuildDemo.Data;

namespace IdleGuildDemo.Runtime
{
    public sealed class SlimeFightController : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private float patrolSpeed = 1.5f;
        [SerializeField] private float minX = -8f;
        [SerializeField] private float maxX = 8f;

        [Header("State")]
        private CombatEnemyState enemyState;
        private SpriteSheetAnimationProvider animProvider;
        private SpriteRenderer spriteRenderer;
        private Collider2D col;
        private Rigidbody2D rb;

        private bool movingRight = true;
        private bool isDying = false;
        private Color originalColor;

        // Sprite lists for states
        private List<Sprite> idleSprites = new List<Sprite>();
        private List<Sprite> moveSprites = new List<Sprite>();
        private List<Sprite> hitSprites = new List<Sprite>();
        private List<Sprite> dieSprites = new List<Sprite>();

        // Health UI
        private UnityEngine.UI.Image fillImage;
        private GameObject healthBarCanvasGo;

        public bool IsDying => isDying;
        public CombatEnemyState EnemyState => enemyState;

        private void Awake()
        {
            animProvider = GetComponent<SpriteSheetAnimationProvider>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            col = GetComponent<Collider2D>();
            rb = GetComponent<Rigidbody2D>();

            if (spriteRenderer != null)
            {
                originalColor = spriteRenderer.color;
            }

            SplitSprites();
        }

        private void Start()
        {
            var hudView = GameObject.FindFirstObjectByType<CombatHUDView>();
            int maxHp = 10; // Fallback
            if (hudView != null && hudView.SlimeDefinition != null)
            {
                maxHp = hudView.SlimeDefinition.MaxHp < 1 ? 1 : hudView.SlimeDefinition.MaxHp;
                
                enemyState = new CombatEnemyState
                {
                    enemyId = hudView.SlimeDefinition.Id ?? string.Empty,
                    maxHp = maxHp,
                    currentHp = maxHp
                };
            }
            else
            {
                enemyState = new CombatEnemyState
                {
                    enemyId = "Enemy_Slime",
                    maxHp = maxHp,
                    currentHp = maxHp
                };
            }

            CreateHealthBar();
            PlayMoveAnimation();
        }

        private void Update()
        {
            if (isDying) return;

            Patrol();
        }

        private void SplitSprites()
        {
            if (animProvider != null && animProvider.Sprites.Count >= 16)
            {
                var allSprites = animProvider.Sprites;
                // Since IReadOnlyList doesn't have GetRange, let's copy them
                for (int i = 0; i < 4; i++) idleSprites.Add(allSprites[i]);
                for (int i = 4; i < 8; i++) moveSprites.Add(allSprites[i]);
                for (int i = 8; i < 12; i++) hitSprites.Add(allSprites[i]);
                for (int i = 12; i < 16; i++) dieSprites.Add(allSprites[i]);
            }
        }

        private void Patrol()
        {
            float currentX = transform.position.x;

            if (movingRight)
            {
                transform.Translate(Vector3.right * patrolSpeed * Time.deltaTime);
                if (currentX >= maxX)
                {
                    movingRight = false;
                }
            }
            else
            {
                transform.Translate(Vector3.left * patrolSpeed * Time.deltaTime);
                if (currentX <= minX)
                {
                    movingRight = true;
                }
            }

            if (spriteRenderer != null)
            {
                spriteRenderer.flipX = !movingRight;
            }
        }

        public void OnHitByPlayer(CombatHUDView hudView)
        {
            if (isDying || enemyState == null || hudView == null) return;

            // Attack specific enemy state through HUD
            CombatTickResult result = hudView.AttackEnemyState(enemyState);
            if (result == null) return;

            // Update local health bar
            UpdateHealthBar();

            // Provide visual hit feedback
            StartCoroutine(HitFeedbackRoutine());

            if (enemyState.IsDefeated)
            {
                Die();
            }
        }

        private IEnumerator HitFeedbackRoutine()
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = Color.red;
            }

            if (animProvider != null && hitSprites.Count > 0)
            {
                animProvider.SetSprites(hitSprites, true);
            }

            yield return new WaitForSeconds(0.2f);

            if (!isDying)
            {
                if (spriteRenderer != null)
                {
                    spriteRenderer.color = originalColor;
                }
                PlayMoveAnimation();
            }
        }

        private void PlayMoveAnimation()
        {
            if (animProvider != null && moveSprites.Count > 0)
            {
                animProvider.SetSprites(moveSprites, true);
                SetLoop(true);
            }
        }

        private void Die()
        {
            isDying = true;

            if (col != null) col.enabled = false;
            if (rb != null) rb.simulated = false;

            if (healthBarCanvasGo != null)
            {
                Destroy(healthBarCanvasGo);
            }

            if (spriteRenderer != null)
            {
                spriteRenderer.color = originalColor;
            }

            if (animProvider != null && dieSprites.Count > 0)
            {
                animProvider.SetSprites(dieSprites, true);
                SetLoop(false);
                SetSpeed(8f);
            }

            var spawner = GameObject.FindFirstObjectByType<SlimeSpawner>();
            if (spawner != null)
            {
                spawner.RemoveSlime(this.gameObject);
            }

            Destroy(this.gameObject, 0.6f);
        }

        private void SetLoop(bool loop)
        {
            if (animProvider == null) return;
            var field = typeof(SpriteSheetAnimationProvider).GetField("loop", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field != null)
            {
                field.SetValue(animProvider, loop);
            }
        }

        private void SetSpeed(float fps)
        {
            if (animProvider == null) return;
            var field = typeof(SpriteSheetAnimationProvider).GetField("framesPerSecond", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field != null)
            {
                field.SetValue(animProvider, fps);
            }
        }

        private void CreateHealthBar()
        {
            healthBarCanvasGo = new GameObject("HealthBarCanvas");
            healthBarCanvasGo.transform.SetParent(this.transform);
            healthBarCanvasGo.transform.localPosition = new Vector3(0, 0.7f, 0);
            healthBarCanvasGo.transform.localScale = new Vector3(0.015f, 0.015f, 1f);

            Canvas canvas = healthBarCanvasGo.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            
            CanvasScaler scaler = healthBarCanvasGo.AddComponent<CanvasScaler>();
            scaler.dynamicPixelsPerUnit = 10;

            GameObject bgGo = new GameObject("Background");
            bgGo.transform.SetParent(healthBarCanvasGo.transform, false);
            var bgRect = bgGo.AddComponent<RectTransform>();
            bgRect.sizeDelta = new Vector2(40, 6);
            var bgImage = bgGo.AddComponent<UnityEngine.UI.Image>();
            bgImage.color = Color.black;

            GameObject fillGo = new GameObject("Fill");
            fillGo.transform.SetParent(bgGo.transform, false);
            var fillRect = fillGo.AddComponent<RectTransform>();
            fillRect.anchorMin = Vector2.zero;
            fillRect.anchorMax = new Vector2(1f, 1f);
            fillRect.sizeDelta = Vector2.zero;
            fillImage = fillGo.AddComponent<UnityEngine.UI.Image>();
            fillImage.color = Color.green;
            fillImage.type = UnityEngine.UI.Image.Type.Filled;
            fillImage.fillMethod = UnityEngine.UI.Image.FillMethod.Horizontal;
            fillImage.fillOrigin = 0;
            fillImage.fillAmount = 1f;
        }

        private void UpdateHealthBar()
        {
            if (fillImage != null && enemyState != null)
            {
                float pct = (float)enemyState.currentHp / enemyState.maxHp;
                fillImage.fillAmount = Mathf.Clamp01(pct);
                fillImage.color = Color.Lerp(Color.red, Color.green, pct);
            }
        }
    }
}