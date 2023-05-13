using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public float minSpawnInterval = 1.0f;
    public float maxSpawnInterval = 5.0f;
    public int maxEnemies = 10;
    private int currentEnemies = 0;
    public Transform[] spawnPositions;
    private bool gameOver = false;
    private PlayerController player;
    public static SpawnManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        StartCoroutine(SpawnEnemies());
    }

    void Update()
    {
        if(currentEnemies < 0)
        {
            currentEnemies = 0;
        }
    }
    // stop spawning enemies when the game is over
    public void GameOver()
    {
    gameOver = true;
    }

    public void EnemyDied()
    {
        currentEnemies--;
    }

    IEnumerator SpawnEnemies()
    {
        while (!gameOver && !player.isDead)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));

        if (currentEnemies < maxEnemies)
        {
            Debug.Log("Enemies are now spawning");
            currentEnemies++;
            int enemyIndex = Random.Range(0, enemyPrefabs.Length);
            int spawnIndex = Random.Range(0, spawnPositions.Length);
            Instantiate(enemyPrefabs[enemyIndex], spawnPositions[spawnIndex].position, Quaternion.identity);
        }

        if (currentEnemies > maxEnemies)
        {
            Debug.Log("Enemies have stopped spawning");
        }
    }
}

}
