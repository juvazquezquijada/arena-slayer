using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoldier : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float moveSpeed = 5f; // The speed at which the enemy moves
    public GameObject bulletPrefab; // The prefab for the bullet
    public Transform firePoint; // The point from which bullets are fired
    public float fireRate = 1f; // The rate at which the enemy fires bullets
    public AudioClip shootSound; // The sound to play when the enemy shoots
    public float bulletLifetime = 2f; // The amount of time the bullet will exist before being destroyed
    public float bulletSpeed = 30f;
    private bool isKnockedDown = false;
    private bool isDisabled = false;
    private float nextFireTime; // The next time the enemy can fire
    private AudioSource audioSource; // Reference to the AudioSource component

    void Start()
    {
        // Get a reference to the AudioSource component
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
     void Update()
{
    // Calculate the direction to the player
    Vector3 direction = player.position - transform.position;
    direction.Normalize();

    // Rotate the enemy to face the player
    transform.rotation = Quaternion.LookRotation(direction);

    // Move the enemy towards the player
    transform.position += direction * moveSpeed * Time.deltaTime;

    // Check if the enemy can fire
    if (Time.time >= nextFireTime)
    {
        // Fire a bullet at the player
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        bulletRigidbody.velocity = (player.position - firePoint.position).normalized * bulletSpeed;

        nextFireTime = Time.time + 1f / fireRate;

        // Play the shoot sound
        audioSource.PlayOneShot(shootSound);

        // Destroy the bullet after 3 seconds
        Destroy(bullet, 3f);
    }
}

    void OnCollisionEnter(Collision collision)
    {
    if (collision.gameObject.CompareTag("Bullet"))
    {
        // Disable the AI script and enable the ragdoll
        isDisabled = true;
        GetComponent<EnemySoldier>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = false;
        
        // Calculate the direction and force of the impact
        Vector3 direction = collision.contacts[0].point - transform.position;
        direction = -direction.normalized;
        float force = 10f;

        // Apply the force to the rigidbody
        GetComponent<Rigidbody>().AddForce(direction * force, ForceMode.Impulse);

        // Destroy the enemy after a delay
        StartCoroutine(Dissolve());

        // If the bullet hits player
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Bullet hit the player!");
        }
    }
}

    public void Death()
    {
        StartCoroutine(Dissolve());
    }
    IEnumerator Dissolve()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}

