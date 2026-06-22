using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IdleGuildDemo.Runtime
{
    /// <summary>
    /// Plays an inspector-assigned sprite frame list on a SpriteRenderer or UI Image.
    /// </summary>
    public sealed class SpriteSheetAnimationProvider : MonoBehaviour
    {
        [Header("Frames")]
        [SerializeField] private List<Sprite> sprites = new List<Sprite>();
        [SerializeField] private float framesPerSecond = 8f;
        [SerializeField] private bool playOnEnable = true;
        [SerializeField] private bool loop = true;

        [Header("Optional Targets")]
        [SerializeField] private SpriteRenderer targetRenderer;
        [SerializeField] private Image targetImage;

        private float elapsed;
        private int currentFrameIndex;
        private bool isPlaying;

        public IReadOnlyList<Sprite> Sprites => sprites;
        public int FrameCount => sprites.Count;
        public bool IsPlaying => isPlaying;

        private void Awake()
        {
            ResolveTargets();
        }

        private void OnEnable()
        {
            ResolveTargets();
            SetFrame(0);

            if (playOnEnable)
            {
                Play();
            }
        }

        private void Update()
        {
            if (!isPlaying || sprites.Count == 0 || framesPerSecond <= 0f)
            {
                return;
            }

            elapsed += Time.deltaTime;
            float secondsPerFrame = 1f / framesPerSecond;

            while (elapsed >= secondsPerFrame)
            {
                elapsed -= secondsPerFrame;
                AdvanceFrame();
            }
        }

        private void OnValidate()
        {
            framesPerSecond = Mathf.Max(0f, framesPerSecond);
            currentFrameIndex = Mathf.Clamp(currentFrameIndex, 0, Mathf.Max(0, sprites.Count - 1));
        }

        public Sprite[] GetSprites()
        {
            return sprites.ToArray();
        }

        public void SetSprites(IEnumerable<Sprite> animationSprites, bool resetToFirstFrame = true)
        {
            sprites.Clear();
            if (animationSprites != null)
            {
                sprites.AddRange(animationSprites);
            }

            currentFrameIndex = Mathf.Clamp(currentFrameIndex, 0, Mathf.Max(0, sprites.Count - 1));
            if (resetToFirstFrame)
            {
                SetFrame(0);
            }
        }

        public bool TryGetFrame(int index, out Sprite sprite)
        {
            if (index < 0 || index >= sprites.Count)
            {
                sprite = null;
                return false;
            }

            sprite = sprites[index];
            return true;
        }

        public void SetFrame(int index)
        {
            ResolveTargets();
            if (!TryGetFrame(index, out Sprite sprite))
            {
                return;
            }

            currentFrameIndex = index;
            if (targetRenderer != null)
            {
                targetRenderer.sprite = sprite;
            }

            if (targetImage != null)
            {
                targetImage.sprite = sprite;
            }
        }

        public void Play()
        {
            ResolveTargets();
            isPlaying = sprites.Count > 0;
        }

        public void Stop()
        {
            isPlaying = false;
            elapsed = 0f;
        }

        private void AdvanceFrame()
        {
            int nextFrame = currentFrameIndex + 1;
            if (nextFrame >= sprites.Count)
            {
                if (!loop)
                {
                    Stop();
                    return;
                }

                nextFrame = 0;
            }

            SetFrame(nextFrame);
        }

        private void ResolveTargets()
        {
            if (targetRenderer == null)
            {
                targetRenderer = GetComponent<SpriteRenderer>();
            }

            if (targetImage == null)
            {
                targetImage = GetComponent<Image>();
            }
        }
    }
}
