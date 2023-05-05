using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
    public GameObject healthPickupPrefab;
    public GameObject ammoPickupPrefab;
    public float spawnRange = 20f;
    public float spawnInterval = 10f;
    public int maxItemsOnScreen = 3;
    private int currentItemsOnScreen = 0;
    private bool canSpawn = true;
    public float spawnRangeY = 2;

    private void Update()
    {
        if (canSpawn && currentItemsOnScreen < maxItemsOnScreen)
        {
            StartCoroutine(SpawnItem());
        }
    }

    private IEnumerator SpawnItem()
    {
        canSpawn = false;
        yield return new WaitForSeconds(spawnInterval);

        GameObject itemPrefab = Random.value < 0.5f ? healthPickupPrefab : ammoPickupPrefab;

        Vector3 spawnPosition = transform.position + Random.insideUnitSphere * spawnRange;
        spawnPosition.y = spawnRangeY;

        Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
        currentItemsOnScreen++;

        canSpawn = true;
    }

    public void RemoveItemFromScreen()
    {
        currentItemsOnScreen--;
    }
}
