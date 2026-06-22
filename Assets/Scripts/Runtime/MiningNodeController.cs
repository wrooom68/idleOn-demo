using UnityEngine;
using System.Collections;
using IdleGuildDemo.UI;

namespace IdleGuildDemo.Runtime
{
    public sealed class MiningNodeController : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private int maxHits = 3;
        [SerializeField] private Vector2 oreDropOffset = new Vector2(0.75f, -0.15f);

        private int currentHits;
        private SpriteRenderer spriteRenderer;
        private Collider2D col;
        private Color originalColor;
        private Vector3 originalScale;
        private bool isMined = false;

        // Health/Durability UI
        private UnityEngine.UI.Image fillImage;
        private GameObject healthBarCanvasGo;
        private TextMesh statusText;

        public bool IsMined => isMined;

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
            if (isMined || miningHUDView == null) return;

            currentHits--;
            UpdateHealthBar();
            miningHUDView.ShowMiningStrokeProgress(1f - ((float)currentHits / maxHits));
            SetNodeStatus(currentHits > 0 ? "Mining copper..." : "Copper ore ready");

            // Visual juice: scale pop & white flash
            StartCoroutine(HitJuiceRoutine());

            if (currentHits <= 0)
            {
                Crumble();
            }
        }

        private IEnumerator HitJuiceRoutine()
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = new Color(1f, 0.85f, 0.4f, 1f); // Golden/Copper flash
            }
            transform.localScale = originalScale * 1.2f;

            yield return new WaitForSeconds(0.12f);

            transform.localScale = originalScale;
            if (spriteRenderer != null)
            {
                spriteRenderer.color = originalColor;
            }
        }

        private void Crumble()
        {
            isMined = true;
            if (col != null) col.enabled = false;
            if (healthBarCanvasGo != null) Destroy(healthBarCanvasGo);

            StartCoroutine(CrumbleRoutine());
        }

        private IEnumerator CrumbleRoutine()
        {
            SpawnCopperOrePickup();

            // Spawn some physical-like debris by scaling down rapidly with rotation
            float elapsed = 0f;
            float duration = 0.3f;
            Vector3 startScale = transform.localScale;
            Quaternion startRot = transform.rotation;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float pct = elapsed / duration;
                transform.localScale = Vector3.Lerp(startScale, Vector3.zero, pct);
                transform.rotation = startRot * Quaternion.Euler(0, 0, pct * 180f);
                yield return null;
            }

            Destroy(this.gameObject);
        }

        private void SpawnCopperOrePickup()
        {
            GameObject oreDrop = new GameObject("CopperOre_Drop");
            oreDrop.transform.position = transform.position + (Vector3)oreDropOffset;
            oreDrop.transform.localScale = new Vector3(0.55f, 0.55f, 1f);

            SpriteRenderer oreRenderer = oreDrop.AddComponent<SpriteRenderer>();
            oreRenderer.sprite = spriteRenderer != null ? spriteRenderer.sprite : null;
            oreRenderer.color = new Color(0.95f, 0.55f, 0.22f, 1f);
            oreRenderer.sortingOrder = spriteRenderer != null ? spriteRenderer.sortingOrder + 1 : 1;

            CircleCollider2D pickupCollider = oreDrop.AddComponent<CircleCollider2D>();
            pickupCollider.isTrigger = true;
            pickupCollider.radius = 0.45f;

            CopperOrePickup pickup = oreDrop.AddComponent<CopperOrePickup>();
            pickup.Initialize(GameObject.FindFirstObjectByType<MiningHUDView>());
        }

        private void CreateHealthBar()
        {
            healthBarCanvasGo = new GameObject("HealthBarCanvas");
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
            fillImage.fillAmount = 1f;

            GameObject statusGo = new GameObject("MiningStatus");
            statusGo.transform.SetParent(this.transform);
            statusGo.transform.localPosition = new Vector3(0, 1.05f, 0);
            statusGo.transform.localScale = Vector3.one * 0.08f;
            statusText = statusGo.AddComponent<TextMesh>();
            statusText.anchor = TextAnchor.MiddleCenter;
            statusText.alignment = TextAlignment.Center;
            statusText.fontSize = 24;
            statusText.color = Color.white;
            statusText.text = "Copper Node";
        }

        private void UpdateHealthBar()
        {
            if (fillImage != null)
            {
                float pct = (float)currentHits / maxHits;
                fillImage.fillAmount = Mathf.Clamp01(pct);
            }
        }

        private void SetNodeStatus(string message)
        {
            if (statusText != null)
            {
                statusText.text = message;
            }
        }
    }
}
