using UnityEngine;
using System.Collections;
using IdleGuildDemo.UI;

namespace IdleGuildDemo.Runtime
{
    public sealed class MiningNodeController : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private int maxHits = 3;
        [SerializeField] private Sprite oreDropSprite;
        [SerializeField] private Vector2 oreDropOffset = new Vector2(0.45f, -0.28f);

        private const int MaxOreDrops = 5;
        private const float ReactivationSeconds = 60f;
        private int currentHits;
        private int oreDropsSpawned;
        private SpriteRenderer spriteRenderer;
        private Collider2D col;
        private Color originalColor;
        private Vector3 originalScale;
        private bool isFullyMined;

        // Health/Durability UI
        private UnityEngine.UI.Image fillImage;
        private GameObject healthBarCanvasGo;
        private UnityEngine.UI.Text progressText;

        public bool IsMined => isFullyMined;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            col = GetComponent<Collider2D>();
            originalScale = transform.localScale;
            if (spriteRenderer != null)
            {
                originalColor = spriteRenderer.color;
            }
            currentHits = maxHits;
        }

        private void Start()
        {
            CreateHealthBar();
        }

        public void OnMineByPlayer(MiningHUDView miningHUDView)
        {
            if (isFullyMined || miningHUDView == null) return;

            currentHits--;
            float cycleProgress = 1f - ((float)currentHits / maxHits);
            float totalProgress = GetTotalProgress(cycleProgress);
            miningHUDView.ShowMiningStrokeProgress(totalProgress);
            SetNodeProgress(totalProgress);

            StartCoroutine(HitJuiceRoutine());

            if (currentHits <= 0)
            {
                SpawnCopperOrePickup();
            }
        }

        private IEnumerator HitJuiceRoutine()
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = new Color(1f, 0.85f, 0.4f, 1f); // Golden/Copper flash
            }

            yield return new WaitForSeconds(0.12f);

            if (spriteRenderer != null)
            {
                spriteRenderer.color = originalColor;
            }
        }

        private void SpawnCopperOrePickup()
        {
            oreDropsSpawned = Mathf.Clamp(oreDropsSpawned + 1, 0, MaxOreDrops);
            if (oreDropsSpawned >= MaxOreDrops)
            {
                isFullyMined = true;
                SetProgressBarVisible(false);
                SetSpriteAlpha(0.5f);
                if (col != null)
                {
                    col.enabled = false;
                }

                StartCoroutine(ReactivateAfterDelay());
            }
            else
            {
                SetNodeProgress(GetTotalProgress(1f));
            }

            GameObject oreDrop = new GameObject("CopperOre_Drop");
            oreDrop.transform.position = GetOreDropStartPosition();
            oreDrop.transform.localScale = new Vector3(0.45f, 0.45f, 1f);

            SpriteRenderer oreRenderer = oreDrop.AddComponent<SpriteRenderer>();
            oreRenderer.sprite = oreDropSprite != null
                ? oreDropSprite
                : spriteRenderer != null ? spriteRenderer.sprite : null;
            oreRenderer.color = Color.white;
            oreRenderer.sortingOrder = spriteRenderer != null ? spriteRenderer.sortingOrder + 2 : 2;

            Rigidbody2D rb = oreDrop.AddComponent<Rigidbody2D>();
            rb.gravityScale = 1f;
            rb.freezeRotation = true;
            rb.linearDamping = 1f;

            CircleCollider2D pickupCollider = oreDrop.AddComponent<CircleCollider2D>();
            pickupCollider.radius = 0.45f;

            CopperOrePickup pickup = oreDrop.AddComponent<CopperOrePickup>();
            pickup.Initialize(GameObject.FindFirstObjectByType<MiningHUDView>(), this);

            if (!isFullyMined)
            {
                currentHits = maxHits;
                SetNodeProgress(GetTotalProgress(0f));
            }
        }

        public void ResetAfterOreCollected()
        {
            if (isFullyMined)
            {
                currentHits = 0;
                SetNodeProgress(1f);
                return;
            }

            SetNodeProgress(GetTotalProgress(0f));
        }

        private void CreateHealthBar()
        {
            healthBarCanvasGo = new GameObject("ProgressBar_MiningProgress");
            healthBarCanvasGo.transform.SetParent(this.transform);
            healthBarCanvasGo.transform.localPosition = new Vector3(0, 0.7f, 0);
            healthBarCanvasGo.transform.localScale = new Vector3(0.015f, 0.015f, 1f);

            Canvas canvas = healthBarCanvasGo.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            
            var scaler = healthBarCanvasGo.AddComponent<UnityEngine.UI.CanvasScaler>();
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
            fillImage.color = new Color(0.85f, 0.45f, 0.25f, 1f); // Copper orange color
            fillImage.type = UnityEngine.UI.Image.Type.Filled;
            fillImage.fillMethod = UnityEngine.UI.Image.FillMethod.Horizontal;
            fillImage.fillOrigin = 0;
            fillImage.fillAmount = 0f;

            GameObject labelGo = new GameObject("Label_Percentage");
            labelGo.transform.SetParent(bgGo.transform, false);
            var labelRect = labelGo.AddComponent<RectTransform>();
            labelRect.anchorMin = Vector2.zero;
            labelRect.anchorMax = Vector2.one;
            labelRect.sizeDelta = Vector2.zero;
            progressText = labelGo.AddComponent<UnityEngine.UI.Text>();
            progressText.alignment = TextAnchor.MiddleCenter;
            progressText.fontSize = 4;
            progressText.fontStyle = FontStyle.Bold;
            progressText.color = Color.white;
            progressText.raycastTarget = false;
            SetNodeProgress(0f);
        }

        private void SetNodeProgress(float normalizedProgress)
        {
            float clampedProgress = Mathf.Clamp01(normalizedProgress);
            if (fillImage != null)
            {
                fillImage.fillAmount = clampedProgress;
            }

            if (progressText != null)
            {
                progressText.text = FormatProgressStatus(clampedProgress);
            }
        }

        private float GetTotalProgress(float currentCycleProgress)
        {
            return Mathf.Clamp01((oreDropsSpawned + Mathf.Clamp01(currentCycleProgress)) / MaxOreDrops);
        }

        private Vector3 GetOreDropStartPosition()
        {
            int dropIndex = Mathf.Max(oreDropsSpawned - 1, 0);
            float spreadX = ((dropIndex % MaxOreDrops) - 2) * 0.28f;
            return transform.position + (Vector3)oreDropOffset + new Vector3(spreadX, 0.35f, 0f);
        }

        private static string FormatProgressStatus(float normalizedProgress)
        {
            return $"{Mathf.Clamp01(normalizedProgress) * 100f:0}%";
        }

        private void SetSpriteAlpha(float alpha)
        {
            if (spriteRenderer == null) return;

            Color color = originalColor;
            color.a = alpha;
            spriteRenderer.color = color;
        }

        private void SetProgressBarVisible(bool visible)
        {
            if (healthBarCanvasGo != null)
            {
                healthBarCanvasGo.SetActive(visible);
            }
        }

        private IEnumerator ReactivateAfterDelay()
        {
            yield return new WaitForSeconds(ReactivationSeconds);

            isFullyMined = false;
            oreDropsSpawned = 0;
            currentHits = maxHits;
            SetSpriteAlpha(1f);
            if (col != null)
            {
                col.enabled = true;
            }

            SetProgressBarVisible(true);
            SetNodeProgress(0f);
        }
    }
}
