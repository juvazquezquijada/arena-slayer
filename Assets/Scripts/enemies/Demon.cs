using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Demon : MonoBehaviour
{
    public GameObject fireballPrefab;
    public float moveSpeed = 3f;
    public float fireballSpeed = 10f;
    public float fireballInterval = 2f;
    public float knockbackForce = 10f;
    public Transform launcher;
    private Transform player;
    private float lastFireballTime = 1f;
    public bool isDead = false;
    public float destroyTime = 20f;
    public AudioClip deathSound; // The death sound to play
    private AudioSource audioSource; // Reference to the AudioSource component
    public AudioClip hurtSound;
    private bool playerDead = false;
    public ParticleSystem explosionParticle;
    private PlayerController playerHealth; // Reference to the player's health script

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

        // Shoot fireballs at the player
        if (Time.time - lastFireballTime > fireballInterval)
        {
            lastFireballTime = Time.time;
            GameObject fireball = Instantiate(fireballPrefab, launcher.position, launcher.rotation);
            fireball.transform.rotation = Quaternion.identity;
            fireball.GetComponent<Rigidbody>().velocity = (player.position - transform.position).normalized * fireballSpeed;
        }
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (isDead) return; // Don't do anything if the enemy is dead
        if (playerDead) return; //Don't do anything if the player is dead     

        if (other.gameObject.CompareTag("Bullet"))
        {
            Die();
            GetComponent<CapsuleCollider>().enabled = false;
            
        }
    }

    public void Die()
    {
        
        isDead = true;

        // Play death sound
        audioSource.PlayOneShot(deathSound);

        // Disable the enemy's collider and renderer
        

        // Apply a force to launch the enemy in the air
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        Vector3 knockbackDirection = transform.up + -transform.forward * 0.5f; // adjust knockback direction
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
         Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);

        // Destroy the enemy after a delay
        Destroy(gameObject, destroyTime);
    }

    public void PlayerDied()
    {
        playerDead = true;
    }
}
