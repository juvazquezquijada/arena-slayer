using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    private bool playerDead = false;
    public Transform bulletSpawn;
    public float fireRate = 3;
    public float bulletSpeed = 30;
    public float lastBulletTime = 0;
    public ParticleSystem explosionParticle;
    public AudioClip gunSound;
    public AudioClip hurtSound;
    private bool hasDied = false;
    public int health = 20;

    private PlayerController1 playerHealth;
    private NavMeshAgent navMeshAgent;
    private SpawnManager spawnManager;
    

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

        if (Time.time - lastBulletTime > fireRate)
        {
            lastBulletTime = Time.time;
            GameObject Bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
            Bullet.GetComponent<Rigidbody>().velocity = (player.position - transform.position).normalized * bulletSpeed;
            audioSource.PlayOneShot(gunSound);
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
        navMeshAgent.enabled = false;
        SpawnManager.Instance.EnemyDied();

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        Vector3 knockbackDirection = transform.up + transform.forward * 0.5f;
        rb.AddForce(knockbackDirection * -knockbackForce, ForceMode.Impulse);
        Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
        PlayerController1.Instance.UpdateScore(15);

        Destroy(gameObject, destroyTime);
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
