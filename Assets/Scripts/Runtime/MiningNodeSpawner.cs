using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace IdleGuildDemo.Runtime
{
    public sealed class MiningNodeSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject nodePrototype;
        [SerializeField] private float spawnInterval = 4f;
        [SerializeField] private int maxNodes = 5;
        [SerializeField] private float minSpawnX = -6f;
        [SerializeField] private float maxSpawnX = 6f;
        [SerializeField] private float spawnY = -3.25f;

        private List<GameObject> activeNodes = new List<GameObject>();

        private void Start()
        {
            if (nodePrototype == null)
            {
                nodePrototype = GameObject.Find("CopperNode_World");
            }

            if (nodePrototype != null)
            {
                nodePrototype.SetActive(false);
                
                // Spawn first 3 nodes immediately
                SpawnNode();
                SpawnNode();
                SpawnNode();
            }

            StartCoroutine(SpawnRoutine());
        }

        private IEnumerator SpawnRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(spawnInterval);
                CleanActiveList();
                if (activeNodes.Count < maxNodes)
                {
                    SpawnNode();
                }
            }
        }

        private void SpawnNode()
        {
            if (nodePrototype == null) return;

            GameObject newNode = Instantiate(nodePrototype, nodePrototype.transform.parent);
            newNode.name = "CopperNode_Spawned_" + System.Guid.NewGuid().ToString().Substring(0, 4);
            
            float randomX = Random.Range(minSpawnX, maxSpawnX);
            newNode.transform.position = new Vector3(randomX, spawnY, 0f);
            
            newNode.SetActive(true);
            activeNodes.Add(newNode);
        }

        private void CleanActiveList()
        {
            activeNodes.RemoveAll(item => item == null);
        }

        public void RemoveNode(GameObject node)
        {
            activeNodes.Remove(node);
        }
    }
}
