using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float knockbackForce = 10f;
    private Transform player;
    public float destroyTime = 20f;
    public AudioClip deathSound; 
    private AudioSource audioSource; 
    private bool playerDead = false;
    public int scoreValue = 1;
    public bool isDead = false;
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
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            return; 
        } 
        if (playerHealth.isDead)
        {
            playerDead = true;
            return;
        }
        //Don't do anything if the player is dead
        // Move towards the player
        transform.LookAt(player);
        transform.position += transform.forward * moveSpeed * Time.deltaTime;

        if (playerHealth.isDead)
        {
            playerDead = true;
            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDead) return; // Don't do anything if the enemy is dead
        if (playerDead) return; //Don't do anything if the player is dead     

        if (other.gameObject.CompareTag("Bullet"))
        {
            // Die
            Die();
            Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
        }      
    }

    public void Die()
    {
        isDead = true;

        // Play death sound
        audioSource.PlayOneShot(deathSound);

        // Disable the enemy's collider and renderer
        GetComponent<CapsuleCollider>().enabled = false;
        

        // Apply a force to launch the enemy in the air
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        Vector3 knockbackDirection = transform.up + transform.forward * 0.5f;
        rb.AddForce(knockbackDirection * -knockbackForce, ForceMode.Impulse);
        Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
        // Destroy the enemy after a delay
        Destroy(gameObject, destroyTime);
         
    }

    public void PlayerDied()
    {
        playerDead = true;
    }
}