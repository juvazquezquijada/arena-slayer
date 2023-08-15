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
    public int health = 6;
    private bool hasDied = false;
    public AudioClip hurtSound;
   

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        audioSource = GetComponent<AudioSource>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController1>();
        navMeshAgent = GetComponent<NavMeshAgent>();
       
        // Set the agent to be active and enable auto-braking
        navMeshAgent.enabled = true;
        navMeshAgent.autoBraking = true;
    }

    void Update()
    {
        if (isDead) return;
        if (playerDead)
        {
            navMeshAgent.velocity = Vector3.zero;
            return;
        }
        if (playerHealth.isDead)
        {
            playerDead = true;
            return;
        }

        // Move towards the player using NavMeshAgent
        navMeshAgent.SetDestination(player.position);

        if (health <= 0)
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
        if (hasDied) return;
        hasDied = true;

        isDead = true;

        audioSource.PlayOneShot(deathSound);

        GetComponent<CapsuleCollider>().enabled = false;

        SpawnManager.Instance.EnemyDied();
        navMeshAgent.enabled = false;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        Vector3 knockbackDirection = transform.up + transform.forward * 0.5f;
        rb.AddForce(knockbackDirection * -knockbackForce, ForceMode.Impulse);
        Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
        CanvasManager.Instance.UpdateScore(4);

        Destroy(gameObject, destroyTime);
    }

    public void TakeDamage()
    {
        health -= 6;
        audioSource.PlayOneShot(hurtSound);
    }

    public void TakeDamagePlasma()
    {
        if (health > 4)
        {
            audioSource.PlayOneShot(hurtSound);
        }
        health -= 4;
    }

    public void TakeDamageRocket()
    {
        health -= 6;
    }

    public void PlayerDied()
    {
        playerDead = true;
    }
}
