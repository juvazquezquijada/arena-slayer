using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public static PlayerSpawner Instance;

    SpawnPoint[] spawnpoints;

    void Awake() 
    {
        Instance = this;
        spawnpoints = GetComponentsInChildren<SpawnPoint>();
    }

    public Transform GetSpawnPoint() 
    {
        return spawnpoints[Random.Range(0, spawnpoints.Length)].transform;
    }
}
