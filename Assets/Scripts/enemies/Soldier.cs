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
    EnemyHealth enemy;
    public ParticleSystem muzzleFlashParticleSystem;
    private PlayerController1 playerHealth;
    private Animator anim;
    public NavMeshAgent navMeshAgent;
    private SpawnManager spawnManager;
    

    void Awake()
    {
        SetTarget();
        player = GameObject.FindGameObjectWithTag("Player1").transform;
        audioSource = GetComponent<AudioSource>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        // Set the agent to be active and enable auto-braking
        navMeshAgent.enabled = true;
        navMeshAgent.autoBraking = true;
        enemy = GetComponent<EnemyHealth>();
        anim = GetComponent<Animator>();
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

        if (Time.time - lastBulletTime > fireRate)
        {
            lastBulletTime = Time.time;
            GameObject Bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
            Bullet.GetComponent<Rigidbody>().velocity = (player.position - transform.position).normalized * bulletSpeed;
            audioSource.PlayOneShot(gunSound);
            muzzleFlashParticleSystem.Play();
            anim.SetTrigger("Shoot");
        }

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
        if (hasDied) return;
        hasDied = true;

        isDead = true;
        anim.SetTrigger("Die");
        audioSource.PlayOneShot(deathSound);
        PlayerController1.Instance.UpdateScore(15);
        navMeshAgent.enabled = false;
        SpawnManager.Instance.EnemyDied();

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        Vector3 knockbackDirection = transform.up + transform.forward * 1f;
        rb.AddForce(knockbackDirection * -knockbackForce, ForceMode.Impulse);
        Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
        

        Destroy(gameObject, destroyTime);
    }

    public void PlayerDied()
    {
        playerDead = true;
    }

    public void SetTarget() 
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController1>();
    }
}
