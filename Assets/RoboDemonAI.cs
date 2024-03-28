using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class RoboDemonAI: MonoBehaviour
{
    public Transform player;
    public Animator animator;
    public GameObject bullet, missile, fireball;
    public GameObject winText;
    public GameObject music;
    public AudioSource audioSource;
    public AudioClip deathSound, gunSound, missileSound;
    public AudioClip[] hurtSounds;
    public Transform leftHand;  // Left hand transform of the demon
    public Transform rightHand; // Right hand transform of the demon
    public float bulletSpeed, missileSpeed, fireballSpeed;
    public float meleeRange = 2f;
    public float walkSpeed = 3f;
    public float shootCooldown = 5f;
    public float missileCooldown = 10f;
    public float chargeCooldown = 8f;
    public float chargeSpeed = 10f;
    public float chargeDuration, fireballDuration = 2f;
    public float chargeStopDistance = 1f;
    public float missileLaunchAnimationDuration = 5f;
    private bool isShooting = false; // Add this flag to prevent multiple shooting sequences
    private bool isRainingFireballs, isLaunchingMissiles = false; // Flag to control the rain of fireballs
    public Transform bulletAimPos;
    public Transform rocketAimPos;
    private float lastHurtSoundTime;
    public float hurtSoundCooldown = 0.5f; // Adjust the cooldown time as needed
    float timeBetweenShots = 0.15f;
    float timeBetweenFireballs = 1f;
    public int maxHealth = 1200;
    public Image healthbar;
    public bool isDead = false;
    private bool playerDead = false;
    private bool canAttack = true;
    private bool isCharging = false;
    private bool isInSecondPhase = false;
    private float currentCooldown;
    private Rigidbody rb;
    private EnemyHealth enemy;
    public PlayerController1 playerHealth;
    private NavMeshAgent navMeshAgent; // Reference to the NavMeshAgent component

    private void Awake()
    {
        enemy = GetComponent<EnemyHealth>();
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemy.health = maxHealth;
        healthbar.fillAmount = enemy.health / maxHealth;
        currentCooldown = 5f;
        navMeshAgent = GetComponent<NavMeshAgent>(); // Get a reference to the NavMeshAgent component
        navMeshAgent.stoppingDistance = 2f; // Set the stopping distance for the agent
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
            // You can adjust cooldowns here to make attacks more frequent
            shootCooldown /= 1.1f; // For example, halve the fireball cooldown
            missileCooldown /= 1.1f; // Halve the ground pound cooldown
            chargeCooldown /= 1.1f ; // Halve the charge cooldown
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
                int randomAttack = Random.Range(0, 4);
                switch (randomAttack)
                {
                    case 0:
                        ShootAttack();
                        currentCooldown = shootCooldown;
                        break;
                    case 1:
                        MissileAttack();
                        currentCooldown = missileCooldown;
                        break;
                    case 2:
                        ChargeAttack();
                        currentCooldown = chargeCooldown;
                        break;
                    case 3:
                        SpecialAttack();
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


    private void ShootAttack()
    {
        if (!isShooting)
        {
            StartCoroutine(AutoShootBullets());
        }
    }

    private IEnumerator AutoShootBullets()
    {
        isShooting = true;
        animator.SetTrigger("Shoot");

        for (int i = 0; i < 10; i++) // Shoot 10 bullets
        {
            // Calculate the direction to the player's last known position
            Vector3 bulletDirection = (bulletAimPos.position - rightHand.position).normalized;

            // Instantiate a bullet from the demon's right hand and shoot it towards the player's last known position
            GameObject newBullet = Instantiate(bullet, rightHand.position, Quaternion.identity);
            Rigidbody bulletRigidbody = newBullet.GetComponent<Rigidbody>();
            bulletRigidbody.velocity = bulletDirection * bulletSpeed;

            audioSource.PlayOneShot(gunSound);
            yield return new WaitForSeconds(timeBetweenShots); // Add a delay between shots
        }

        isShooting = false; // Reset the shooting flag after the sequence
    }



    private void MissileAttack()
    {
        // Randomly determine the number of missiles to shoot (between 1 and 3).
        int numberOfMissilesToShoot = Random.Range(1, 4);

        StartCoroutine(ShootMissilesSequentially(numberOfMissilesToShoot));
    }

    private IEnumerator ShootMissilesSequentially(int numberOfMissiles)
    {
        for (int i = 0; i < numberOfMissiles; i++)
        {
            animator.SetTrigger("ShootMissile");
            // Instantiate a missile from the left hand
            GameObject newMissile = Instantiate(missile, leftHand.position, Quaternion.identity);

            // Calculate the direction to the player's last known position
            Vector3 launchDirection = (rocketAimPos.position - leftHand.position).normalized;

            // Get the Rigidbody component of the missile
            Rigidbody missileRigidbody = newMissile.GetComponent<Rigidbody>();
            audioSource.PlayOneShot(missileSound);

            // Set the missile's velocity to move in the calculated launch direction
            missileRigidbody.velocity = launchDirection * missileSpeed;

            // Wait for a short delay between missile shots (you can adjust this delay)
            yield return new WaitForSeconds(2f); // Adjust the delay as needed
        }
    }





    private void SpecialAttack()
    {
        if (!isLaunchingMissiles)
        {
            StartCoroutine(LaunchMissiles());
        }
    }

    private IEnumerator LaunchMissiles()
    {
        isLaunchingMissiles = true;

        // Play the animation for launching missiles
        animator.SetTrigger("LaunchMissiles");

        // Wait for the animation to complete (you may need to adjust the time according to your animation)
        yield return new WaitForSeconds(missileLaunchAnimationDuration);

        // Spawn fireballs that rain down towards the player
        for (int i = 0; i < 5; i++)
        {
            Vector3 spawnPosition = player.position + new Vector3(Random.Range(-5f, 5f), 20f, Random.Range(-5f, 5f));
            GameObject newFireball = Instantiate(fireball, spawnPosition, Quaternion.identity);

            // Calculate direction towards the player
            Vector3 directionToPlayer = (player.position - spawnPosition).normalized;

            // Set initial velocity to move towards the player (you may need to adjust fireballSpeed)
            Rigidbody fireballRigidbody = newFireball.GetComponent<Rigidbody>();
            fireballRigidbody.velocity = directionToPlayer * fireballSpeed;

            // Destroy the fireball after a delay (you may need to adjust the delay)
            Destroy(newFireball, fireballDuration);

            // Wait for a short time before spawning the next fireball
            yield return new WaitForSeconds(timeBetweenFireballs);
        }

        isLaunchingMissiles = false;
    }

    private void ChargeAttack()
    {
        // Trigger charge animation
        animator.SetTrigger("Charge");

        // Implement your charge attack logic here
        Debug.Log("Charge attack!");
        navMeshAgent.speed = 10f;
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
        navMeshAgent.speed = 2f;
        isCharging = false;
    }
}
