using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Demon : MonoBehaviour
{
    public GameObject fireballPrefab;
    public float fireballSpeed = 10f;
    public float fireballInterval = 2f;
    public float knockbackForce = 10f;
    public Transform launcher;
    private Transform player;
    private float lastFireballTime = 1f;
    public bool isDead = false;
    public float destroyTime = 20f;
    public AudioClip deathSound;
    private AudioSource audioSource;
    public AudioClip hurtSound;
    private bool playerDead = false;
    private bool hasDied = false;
    public ParticleSystem explosionParticle;
    public int health = 12;

    private PlayerController1 playerHealth;

    private NavMeshAgent navMeshAgent; // Reference to the NavMeshAgent component

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        audioSource = GetComponent<AudioSource>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController1>();
        navMeshAgent = GetComponent<NavMeshAgent>(); // Get a reference to the NavMeshAgent component
        navMeshAgent.stoppingDistance = 2f; // Set the stopping distance for the agent
        
    }

    private void Update()
    {
        if (isDead) return;
        if (playerDead)
        {
            navMeshAgent.velocity = Vector3.zero; // Freeze the agent's movement
            return;
        }
        if (playerHealth.isDead)
        {
            playerDead = true;
            return;
        }

        // Move towards the player using NavMeshAgent
        navMeshAgent.SetDestination(player.position);

        // Shoot fireballs at the player
        if (Time.time - lastFireballTime > fireballInterval)
        {
            lastFireballTime = Time.time;
            GameObject fireball = Instantiate(fireballPrefab, launcher.position, launcher.rotation);
            fireball.transform.rotation = Quaternion.identity;
            fireball.GetComponent<Rigidbody>().velocity = (player.position - transform.position).normalized * fireballSpeed;
        }

        if (health <= 0)
        {
            Die();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDead) return;
        if (playerDead) return;
    }

    public void Die()
    {
        if (hasDied) return;
        hasDied = true;

        isDead = true;
        navMeshAgent.enabled = false;
        audioSource.PlayOneShot(deathSound);
        SpawnManager.Instance.EnemyDied();
        GetComponent<CapsuleCollider>().enabled = false;

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        Vector3 knockbackDirection = transform.up + -transform.forward * 0.5f;
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
        Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);

        Destroy(gameObject, destroyTime);

        CanvasManager.Instance.UpdateScore(10);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        audioSource.PlayOneShot(hurtSound);
    }


    public void PlayerDied()
    {
        playerDead = true;
    }
}
