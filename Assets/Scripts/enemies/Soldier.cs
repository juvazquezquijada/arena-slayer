using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float destroyTime = 5;
    public float knockbackForce;
    public int scoreValue = 1;
    private AudioSource audioSource;
    public AudioClip deathSound;
    public float moveSpeed = 1;
    private Transform player;
    public bool isDead = false;
    private bool playerDead= false;
    public Transform bulletSpawn;
    public float fireRate = 3;
    public float bulletSpeed = 30;
    public float lastBulletTime = 0;
    public ParticleSystem explosionParticle;
    public AudioClip gunSound;
    public AudioClip hurtSound;
    private bool hasDied = false;
    public int health = 20;
    private PlayerController playerHealth; // Reference to the player's health script
    private SpawnManager spawnManager;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        audioSource = GetComponent<AudioSource>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return; // Don't do anything if the enemy is dead
        if (playerDead) 
        {
            // Freeze the enemy's movement
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            return;
        }
        if (playerHealth.isDead)
        {
            playerDead = true;
            return;
        }
        // Move towards the player
        transform.LookAt(player);
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
        if (Time.time - lastBulletTime > fireRate)
        {
            lastBulletTime = Time.time;
           GameObject Bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
            Bullet.GetComponent<Rigidbody>().velocity = (player.position - transform.position).normalized * bulletSpeed;
            audioSource.PlayOneShot(gunSound);
        }

         if (health <=0)
        {
            Die();
        }
    }

    private void OnTriggerEnter(Collider other)
{
    if (isDead) return; // Don't do anything if the enemy is dead
    if (playerDead) return; //Don't do anything if the player is dead     

    
}

    public void Die()
    {
        if (hasDied) return;
         hasDied = true;

         isDead = true;

         // Play death sound
         audioSource.PlayOneShot(deathSound);

         // tell the spawnManager script to subtract the current enemies present value by 1
        SpawnManager.Instance.EnemyDied();

         // Disable the enemy's collider and renderer
         GetComponent<CapsuleCollider>().enabled = false;
    
         // Apply a force to launch the enemy in the air
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        Vector3 knockbackDirection = transform.up + transform.forward * 0.5f;
        rb.AddForce(knockbackDirection * -knockbackForce, ForceMode.Impulse);
        Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
        CanvasManager.Instance.UpdateScore(15);
          // Destroy the enemy after a delay
          Destroy(gameObject, destroyTime);
    }

    public void TakeDamage()
    {   
        health -= 10;
        audioSource.PlayOneShot(hurtSound);
    }
    public void TakeDamagePlasma()
    {
        if (health > 4) // Only play sound if the enemy is still alive
        {
            audioSource.PlayOneShot(hurtSound);
        }
            health-= 4;
    }

    public void PlayerDied()
    {
        playerDead = true;
    }
} 
    


