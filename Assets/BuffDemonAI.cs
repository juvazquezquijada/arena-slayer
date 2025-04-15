using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class BuffDemonAI : MonoBehaviour
{
    public Transform player;
    public Animator animator;
    public Animator hudAnim;
    public GameObject fireballPrefab;
    public GameObject winText;
    public GameObject music;
    public AudioSource audioSource;
    public AudioClip deathSound;
    public AudioClip[] hurtSounds;
    public Transform leftHand;  // Left hand transform of the demon
    public Transform rightHand; // Right hand transform of the demon
    public float fireballSpeed = 10f;
    public float meleeRange = 2f;
    public float walkSpeed = 3f;
    public float fireballCooldown = 3f;
    public float groundPoundCooldown = 5f;
    public float chargeCooldown = 8f;
    public float chargeSpeed = 10f;
    public float chargeDuration = 5f;
    public float chargeStopDistance = 1f;
    private float lastHurtSoundTime;
    public float hurtSoundCooldown = 0.5f; // Adjust the cooldown time as needed
    private EnemyHealth enemy;
    public int maxHealth = 300;
    public Image healthbar;
    public bool isDead = false;
    private bool playerDead = false;
    private bool canAttack = true;
    private bool isCharging = false;
    private bool isStaggered = false;
    private float staggerTime = 10f;
    private bool isInSecondPhase = false;
    private float currentCooldown;
    private Rigidbody rb;

    public PlayerController1 playerHealth;
    private NavMeshAgent navMeshAgent; // Reference to the NavMeshAgent component

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        enemy = GetComponent<EnemyHealth>();
        healthbar.fillAmount = enemy.health / maxHealth;

        navMeshAgent = GetComponent<NavMeshAgent>(); // Get a reference to the NavMeshAgent component
        navMeshAgent.stoppingDistance = 2f; // Set the stopping distance for the agent
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        currentCooldown = 5f;
    }
    private void Update()
    {
        if (isDead || playerHealth.isDead)
            return;
        
        UpdateHealthBar();

        // Check if the demon should enter the second phase
        if (enemy.health <= maxHealth / 2 && !isInSecondPhase)
        {
            // Enter the second phase
            isInSecondPhase = true;
        }

        if(isStaggered)
        {
            staggerTime -= Time.deltaTime;
            
            if(staggerTime <= 0f)
            {
            EndStagger();
            }
        }

        if (enemy.damageChain >= enemy.staggerValue)
        {
            Stagger();
        }

        if (canAttack && !playerDead)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            WalkTowardsPlayer();
            if (distanceToPlayer <= meleeRange)
            {
                MeleeAttack();
            }
            else if (currentCooldown <= 0f)
            {
                int randomAttack = Random.Range(0, 3);
                switch (randomAttack)
                {
                    case 0:
                        FireballAttack();
                        currentCooldown = fireballCooldown;
                        break;
                    case 1:
                        GroundPoundAttack();
                        currentCooldown = groundPoundCooldown;
                        break;
                    case 2:
                        ChargeAttack();
                        currentCooldown = chargeCooldown;
                        break;
                }
            }
            else
            {
                currentCooldown -= Time.deltaTime;
            }
        }

        
    }

    private void WalkTowardsPlayer()
    {
        if (isDead)
            return;
        // Move towards the player using NavMeshAgent
        navMeshAgent.SetDestination(player.position);
    }


    private void MeleeAttack()
    {
        // Trigger melee attack animation
        animator.SetTrigger("MeleeAttack");

        // Implement your melee attack logic here
        Debug.Log("Melee attack!");
    }

    private void Stagger()
    {
        animator.SetTrigger("Stagger");
        navMeshAgent.enabled = false;
        isStaggered = true;
        canAttack = false;
        hudAnim.SetTrigger("StaggerBar");
        enemy.damageChain = 0;
        
    }

    private void EndStagger()
    {
        animator.SetTrigger("StaggerEnd");
        isStaggered = false;
        navMeshAgent.enabled = true;
        canAttack = true;
        hudAnim.SetTrigger("Normal");
        
    }

    private void FireballAttack()
    {
        StartCoroutine(ThrowFireballs());
        animator.SetTrigger("FireballAttack");
        
    }
    private IEnumerator ThrowFireballs()
    {
        yield return new WaitForSeconds(1.225f);

        Debug.Log("Fireball attack!");

        // Calculate direction to the player from the demon
        Vector3 playerDirection = (player.position - transform.position).normalized;

        // Calculate fireball velocity to hit the player from the demon's hands
        Vector3 fireballVelocity = playerDirection * fireballSpeed;

        // Spawn fireballs from both hands
        SpawnFireball(leftHand.position, fireballVelocity);

        yield return new WaitForSeconds(0.225f);

        SpawnFireball(rightHand.position, fireballVelocity);

        StopCoroutine(ThrowFireballs());
    }
    private void GroundPoundAttack()
    {
        animator.SetTrigger("GroundPound");
        Debug.Log("Ground pound attack!");

        // Delay and spawn fireballs in a circular pattern
        StartCoroutine(SpawnFireballsWithDelay());
    }

    private IEnumerator SpawnFireballsWithDelay()
    {
        yield return new WaitForSeconds(2.33f);

        int numFireballs = 35;
        float angleIncrement = 360f / numFireballs;

        for (int i = 0; i < numFireballs; i++)
        {
            float angle = i * angleIncrement;
            Vector3 offset = Quaternion.Euler(0f, angle, 0f) * Vector3.forward * 2f;
            Vector3 spawnPosition = transform.position + offset;

            // Calculate a direction away from the demon
            Vector3 spawnDirection = (spawnPosition - transform.position).normalized;

            // Calculate the fireball velocity using the spawn direction
            Vector3 fireballVelocity = spawnDirection * fireballSpeed;

            // Spawn the fireball
            SpawnFireball(spawnPosition, fireballVelocity);
        }
    }


    private void SpawnFireball(Vector3 position, Vector3 velocity)
    {
        Debug.Log("Spawned fireball");
        GameObject fireball = Instantiate(fireballPrefab, position, Quaternion.identity);
        fireball.transform.rotation = Quaternion.identity;
        fireball.GetComponent<Rigidbody>().velocity = velocity;
    }





    private void ChargeAttack()
    {
        // Trigger charge animation
        animator.SetTrigger("Charge");

        // Implement your charge attack logic here
        Debug.Log("Charge attack!");
        navMeshAgent.speed = 15f;
        navMeshAgent.angularSpeed = 160f;
        Invoke(nameof(StopCharge), chargeDuration);
    }

    void UpdateHealthBar()
    {
        healthbar.fillAmount = (float)enemy.health / maxHealth;

        if (enemy.health <= 0)
        {
            Die();
        }
    }
    void PlayRandomHurtSound()
    {
        if (Time.time - lastHurtSoundTime > hurtSoundCooldown)
        {
            if (hurtSounds.Length > 0)
            {
                int randomIndex = Random.Range(0, hurtSounds.Length);
                audioSource.PlayOneShot(hurtSounds[randomIndex]);
                lastHurtSoundTime = Time.time;
            }
        }
    }


    void Die()
    {
        animator.SetTrigger("Die");
        music.gameObject.SetActive(false);
        winText.gameObject.SetActive(true);
        navMeshAgent.enabled = false;
        audioSource.PlayOneShot(deathSound);
        Destroy(gameObject, 4f);
        PlayerController1.Instance.UpdateScore(500);
        isDead = true;
    }

    public void PlayerDied()
    {
        playerDead = true;
    }


    void StopCharge()
    {
        navMeshAgent.speed = 3.5f;
        isCharging = false;
    }
}
