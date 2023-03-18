using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public Transform gunBarrel;  // The position where the bullets will be spawned
    public GameObject bulletPrefab;  // The bullet prefab to be spawned
    public float bulletSpeed = 50f;  // The speed of the bullet
    public float fireRate = 0.1f;  // The delay between shots

    private float fireTimer = 0f;  // Timer to track when the player can shoot again

    void Update()
    {
        // Check if the player is pressing the fire button and if enough time has passed since the last shot
        if (Input.GetButtonDown("Fire1") && fireTimer <= 0f)
        {
            // Spawn a bullet at the gun barrel position
            GameObject bullet = Instantiate(bulletPrefab, gunBarrel.position, gunBarrel.rotation);

            // Add velocity to the bullet
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.velocity = gunBarrel.forward * bulletSpeed;

            // Reset the fire timer
            fireTimer = fireRate;
        }

        // Decrease the fire timer if the player can't shoot yet
        if (fireTimer > 0f)
        {
            fireTimer -= Time.deltaTime;
        }
    }
}
