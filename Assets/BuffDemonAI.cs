using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class BuffDemonAI : MonoBehaviour
{
    public Transform player;
    public Animator animator;
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
    public float chargeDuration = 2f;
    public float chargeStopDistance = 1f;
    private float lastHurtSoundTime;
    public float hurtSoundCooldown = 0.5f; // Adjust the cooldown time as needed
    public int health;
    public int maxHealth = 300;
    public Image healthbar;
    public bool isDead = false;
    private bool playerDead = false;
    private bool canAttack = true;
    private bool isCharging = false;
    private bool isInSecondPhase = false;
    private float currentCooldown;
    private Rigidbody rb;

    private PlayerController1 playerHealth;
    private NavMeshAgent navMeshAgent; // Reference to the NavMeshAgent component

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController1>();
        health = maxHealth;
        healthbar.fillAmount = health / maxHealth;

        navMeshAgent = GetComponent<NavMeshAgent>(); // Get a reference to the NavMeshAgent component
        navMeshAgent.stoppingDistance = 2f; // Set the stopping distance for the agent
    }
    private void Update()
    {
        if (isDead)
            return;

        if (playerHealth.isDead)
        {
            playerDead = true;
            return;
        }

        // Check if the demon should enter the second phase
        if (health <= maxHealth / 2 && !isInSecondPhase)
        {
            // Enter the second phase
            isInSecondPhase = true;
            // You can adjust cooldowns here to make attacks more frequent
            fireballCooldown /= 2; // For example, halve the fireball cooldown
            groundPoundCooldown /= 2; // Halve the ground pound cooldown
            chargeCooldown /= 2; // Halve the charge cooldown
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

    private void FireballAttack()
    {
        animator.SetTrigger("FireballAttack");
        Debug.Log("Fireball attack!");

        // Calculate direction to the player from the demon
        Vector3 playerDirection = (player.position - transform.position).normalized;

        // Calculate fireball velocity to hit the player from the demon's hands
        Vector3 fireballVelocity = playerDirection * fireballSpeed;

        // Spawn fireballs from both hands
        SpawnFireball(leftHand.position, fireballVelocity);
        SpawnFireball(rightHand.position, fireballVelocity);
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
        yield return new WaitForSeconds(1.7f);

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
        rb.velocity = (player.position - transform.position).normalized * chargeSpeed;
        Invoke(nameof(StopCharge), chargeDuration);
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Demon took damage" + damage);
        health -= damage;
        healthbar.fillAmount = (float)health / maxHealth;

        if (health <= 0)
        {
            Die();
        }
        else
        {
            PlayRandomHurtSound();
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
        StartCoroutine(SpawnFireballsWithDelay());
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
        rb.velocity = Vector3.zero;
        isCharging = false;
    }
}
