using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public float minSpawnInterval = 1.0f;
    public float maxSpawnInterval = 5.0f;
    public int maxEnemies = 10;
    public Transform[] spawnPositions;

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));

            if (GameObject.FindGameObjectsWithTag("Enemy").Length < maxEnemies)
            {
                int enemyIndex = Random.Range(0, enemyPrefabs.Length);
                int spawnIndex = Random.Range(0, spawnPositions.Length);
                Instantiate(enemyPrefabs[enemyIndex], spawnPositions[spawnIndex].position, Quaternion.identity);
            }
        }
    }
}


