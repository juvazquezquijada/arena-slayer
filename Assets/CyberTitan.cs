using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyberTitan : MonoBehaviour
{
    public GameObject rocketPrefab;
    public Transform player;
    public float moveSpeed = 6f;
    public float rocketSpeed = 50f;
    private float lastRocketTime = 1f;
    public float fireRate = 1f;
    public Transform launcher;
    public bool isDead = false;
    public AudioClip deathSound;
    public AudioClip hurtSound;
    private AudioSource audioSource;
    private bool playerDead = false;
    public ParticleSystem explosionParticle;
    public int health;
    private PlayerController playerHealth;
    private bool hasPlayedDeathSound = false;
    public AudioClip shootSound;

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
        // Die if bosses health is 0
        if (health <= 0)
        {
            Die();
            if (!hasPlayedDeathSound) 
            {
                audioSource.PlayOneShot(deathSound);
                hasPlayedDeathSound = true;
                
            }
        }
      if (isDead) return; // Don't do anything if the boss is dead
        if (playerDead) 
        {
            // Freeze the bosses movement
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            return;
        }
        if (playerHealth.isDead)
        {
            playerDead = true;
            return;
        }  
    
        // Move towards the player
        transform.LookAt(player.transform.position);
        transform.position += transform.forward * moveSpeed * Time.deltaTime;

        // Shoot fireballs at the player
        if (Time.time - lastRocketTime > fireRate)
        {
            lastRocketTime = Time.time;
            GameObject rocket = Instantiate(rocketPrefab, launcher.position, launcher.rotation);
            rocket.transform.rotation = Quaternion.identity;
            rocket.GetComponent<Rigidbody>().velocity = (player.position - transform.position).normalized * rocketSpeed;
            audioSource.PlayOneShot(shootSound);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDead) return; // Don't do anything if the enemy is dead
        if (playerDead) return; //Don't do anything if the player is dead     

        if (other.gameObject.CompareTag("Bullet"))
        {
            health -= 1;
            if (health < 0) health = 0;
            Destroy(other.gameObject);
            audioSource.PlayOneShot(hurtSound);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
        }
    } 
    public void Die()
    {
        GetComponent<BoxCollider>().enabled = false;
        Destroy(gameObject, 5f);
        audioSource.PlayOneShot(deathSound);
    }

    
}
