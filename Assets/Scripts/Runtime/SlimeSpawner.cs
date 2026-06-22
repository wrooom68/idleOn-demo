using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace IdleGuildDemo.Runtime
{
    public sealed class SlimeSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject slimePrototype;
        [SerializeField] private float spawnInterval = 3f; // Increase count slowly
        [SerializeField] private int maxSlimes = 8;
        [SerializeField] private float minSpawnX = -6f;
        [SerializeField] private float maxSpawnX = 6f;
        [SerializeField] private float spawnY = -3.25f;

        private List<GameObject> activeSlimes = new List<GameObject>();

        private void Start()
        {
            if (slimePrototype == null)
            {
                slimePrototype = GameObject.Find("Slime_World");
            }

            if (slimePrototype != null)
            {
                // Deactivate prototype so we can use it purely as a blueprint
                slimePrototype.SetActive(false);
                
                // Spawn first 2 slimes immediately
                SpawnSlime();
                SpawnSlime();
            }

            StartCoroutine(SpawnRoutine());
        }

        private IEnumerator SpawnRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(spawnInterval);
                CleanActiveList();
                if (activeSlimes.Count < maxSlimes)
                {
                    SpawnSlime();
                }
            }
        }

        private void SpawnSlime()
        {
            if (slimePrototype == null) return;

            GameObject newSlime = Instantiate(slimePrototype, slimePrototype.transform.parent);
            newSlime.name = "Slime_Spawned_" + System.Guid.NewGuid().ToString().Substring(0, 4);
            
            // Set random position on the platform
            float randomX = Random.Range(minSpawnX, maxSpawnX);
            newSlime.transform.position = new Vector3(randomX, spawnY, 0f);
            
            newSlime.SetActive(true);
            activeSlimes.Add(newSlime);
        }

        private void CleanActiveList()
        {
            activeSlimes.RemoveAll(item => item == null);
        }

        public void RemoveSlime(GameObject slime)
        {
            activeSlimes.Remove(slime);
        }
    }
}