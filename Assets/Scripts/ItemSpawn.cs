using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
    public GameObject healthPrefab;
    public GameObject ammoPrefab;
    public int spawnRange = 20;
    public float spawnInterval = 5f;
    public int maxItems = 3;

    private void Start()
    {
        StartCoroutine(SpawnItems());
    }

    private IEnumerator SpawnItems()
    {
        
        while (true)
        {
            // Determine random positions for spawn
            float randomX = Random.Range(-spawnRange, spawnRange);
            float randomZ = Random.Range(-spawnRange, spawnRange);
            Vector3 spawnPosition = new Vector3(randomX, 0.5f, randomZ);

            // Instantiate a random item prefab
            int randomItem = Random.Range(0, 2); // 0 for health, 1 for ammo
            if (randomItem == 0)
            {
                Instantiate(healthPrefab, spawnPosition, Quaternion.identity);
            }
            else
            {
                Instantiate(ammoPrefab, spawnPosition, Quaternion.identity);
            }

            // Wait for specified interval before spawning another item
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
