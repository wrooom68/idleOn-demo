using UnityEngine;
using UnityEngine.SceneManagement;

namespace IdleGuildDemo.Runtime
{
    [RequireComponent(typeof(Collider2D))]
    public sealed class WorldTriggerPortal : MonoBehaviour
    {
        [SerializeField] private string sceneToLoad;

        public string SceneToLoad
        {
            get => sceneToLoad;
            set => sceneToLoad = value;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<PlayerMovement2D>() != null)
            {
                if (!string.IsNullOrEmpty(sceneToLoad))
                {
                    SceneManager.LoadScene(sceneToLoad);
                }
            }
        }
    }
}