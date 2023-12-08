using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    
    private PlayerController1 playerHealth;
    
    private NavMeshAgent navMeshAgent;
    private bool hasDied = false;
    public AudioClip hurtSound;
    EnemyHealth enemy;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player1").transform;
        audioSource = GetComponent<AudioSource>();
        
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemy = GetComponent<EnemyHealth>();
        // Set the agent to be active and enable auto-braking
        navMeshAgent.enabled = true;
        navMeshAgent.autoBraking = true;
    }

    void Update()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player1").GetComponent<PlayerController1>();
        if (isDead || playerHealth.isDead)
        {
            navMeshAgent.velocity = Vector3.zero;
            return;
        }

        // Move towards the player using NavMeshAgent
        navMeshAgent.SetDestination(player.position);

        if (enemy.health <= 0)
        {
            Die();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDead) return;
        if (playerDead) return;

        if (other.gameObject.CompareTag("Bullet"))
        {
            Die();
            Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
        }
    }

    public void Die()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        Vector3 knockbackDirection = transform.up + transform.forward * 0.5f;
        rb.AddForce(knockbackDirection * -knockbackForce, ForceMode.Impulse);
        if (hasDied) return;
        hasDied = true;

        isDead = true;

        audioSource.PlayOneShot(deathSound);
        SpawnManager.Instance.EnemyDied();
        navMeshAgent.enabled = false;
        
        rb.isKinematic = false;
        
        Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
        PlayerController1.Instance.UpdateScore(4);

        Destroy(gameObject, destroyTime);
    }

    public void PlayerDied()
    {
        playerDead = true;
    }
}
