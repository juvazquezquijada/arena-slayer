using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.AI;
public class AncientDragonBoss : MonoBehaviour
{
    public Transform player;
    private float currentCooldown;

    public int health;
    public int maxHealth = 3500;
    public Image bossHealthbar;
    public GameObject  fireball, singleFireball;
    public AudioSource audioSource;
    public AudioClip fireBreathSound, deathSound;
    public Animator anim;
    public GameObject winText, music;
    public float fireBreathCooldown = 1f;

    private bool isDead = false;
    private bool playerDead = false;
    private bool isAttacking = false;
    private float fireBarrageAnimationDuration = 2.5f;
    private float timeBetweenFireballs = 1f;
    private float fireballDuration = 2f;
    private float fireballSpeed = 35f;
    private float walkCooldown = 3f;
    private float meleeRange = 20f;

    private float chargeSpeed = 15;
    private float chargeDuration = 7f;
    public float diveDuration = 6.5f;
    public float diveSpeed = 10f;
    public float diveRange = 10f;

    public NavMeshAgent navMeshAgent;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        health = maxHealth;
        bossHealthbar.fillAmount = (float)health / maxHealth;
        
    }

    private void Update()
    {
        if (isDead)
            return;

        if (playerDead)
            return;

        

        if (!isAttacking)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            WalkTowardsPlayer();
            if (currentCooldown <= 0f)
            {
                int randomAttack = Random.Range(0, 7);
                switch (randomAttack)
                {
                    case 0:
                        StartCoroutine(FireBreath());
                        currentCooldown = fireBreathCooldown;
                        break;
                    case 1:
                        StartCoroutine(FireBreath2());
                        currentCooldown = fireBreathCooldown;
                        break;
                    case 2:
                        StartCoroutine(FireBreath3());
                        currentCooldown = fireBreathCooldown;
                        break;
                    case 3:
                        StartCoroutine(FireAOE());
                        currentCooldown = fireBreathCooldown;
                        break;
                    case 4:
                        FireBarrage();
                        currentCooldown = fireBreathCooldown;
                        break;
                    case 5:
                        ChargeAttack();
                        currentCooldown = fireBreathCooldown;
                        break;
                    case 6:
                        PerformDiveAttack();
                        currentCooldown = fireBreathCooldown;
                        break;

                }
			}
            else
            {
                currentCooldown -= Time.deltaTime;
            }
        }
    }

    private void MeleeAttack()
    {
        anim.SetTrigger("MeleeAttack");
        Debug.Log("Melee");
    }

    private IEnumerator FireBreath()
    {
        isAttacking = true;
        // Stop the NavMeshAgent instantly
        navMeshAgent.enabled = false;
        // Trigger the fire breath animation
        anim.SetTrigger("FireBreath");
        Debug.Log("Fire");

        // Adjust the duration based on your animation's length or desired timing
        yield return new WaitForSeconds(8);

        // Set isAttacking to false after the attack duration
        isAttacking = false;
    }

    private IEnumerator FireBreath2()
    {
        isAttacking = true;
        // Stop the NavMeshAgent instantly
        navMeshAgent.enabled = false;
        // Trigger the fire breath animation
        anim.SetTrigger("FireBreath2");
        Debug.Log("Fire2");

        // Adjust the duration based on your animation's length or desired timing
        yield return new WaitForSeconds(8);

        // Set isAttacking to false after the attack duration
        isAttacking = false;
    }

    private IEnumerator FireBreath3()
    {
        isAttacking = true;
        // Stop the NavMeshAgent instantly
        navMeshAgent.enabled = false;
        // Trigger the fire breath animation
        anim.SetTrigger("FireBreath3");
        Debug.Log("Fire3");

        // Adjust the duration based on your animation's length or desired timing
        yield return new WaitForSeconds(8);

        // Set isAttacking to false after the attack duration
        isAttacking = false;
    }

    private IEnumerator FireAOE()
    {
        isAttacking = true;
        // Stop the NavMeshAgent instantly
        navMeshAgent.speed = 0f;
        navMeshAgent.enabled = false;
        // Trigger the fire breath animation
        anim.SetTrigger("FireAOE");
        Debug.Log("FireAOE");

        // Wait for 3 seconds before starting to spawn fireballs
        yield return new WaitForSeconds(5f);

        // Spawn the fireball AoE 5 times with a delay of 1.5 seconds between each spawn
        for (int i = 0; i < 5; i++)
        {
            StartCoroutine(SpawnFireballsWithDelay());

            // Wait for 1.5 seconds before spawning the next fireball AoE
            yield return new WaitForSeconds(1.5f);
        }

        // Set isAttacking to false after the attack duration
        isAttacking = false;
    }


    private IEnumerator SpawnFireballsWithDelay()
    {

        yield return new WaitForSeconds(0);
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
        GameObject fireball = Instantiate(singleFireball, position, Quaternion.identity);
        fireball.transform.rotation = Quaternion.identity;
        fireball.GetComponent<Rigidbody>().velocity = velocity;
    }

    public void WalkTowardsPlayer()
    {
        if (!isDead && !isAttacking)
        {
            navMeshAgent.enabled = true;
            navMeshAgent.speed = 3f;
            navMeshAgent.SetDestination(player.position);
            anim.SetTrigger("Walk");
            Debug.Log("WalkingTowardsPlayer");
        }
    }

    // Attack 4: Fire Barrage
    private void FireBarrage()
    {
        if (!isAttacking)
        {
            StartCoroutine(PerformFireBarrage());
        }
    }

    private IEnumerator PerformFireBarrage()
    {
        // Stop the NavMeshAgent instantly
        navMeshAgent.enabled = false;
        Debug.Log("FireBaragge");

        // Play the animation for launching missiles
        anim.SetTrigger("FireBarrage");
        isAttacking = true;
        // Wait for the animation to complete (you may need to adjust the time according to your animation)
        yield return new WaitForSeconds(fireBarrageAnimationDuration);

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

        isAttacking = false;
    }

    private void ChargeAttack()
    {
        navMeshAgent.enabled = false;
        isAttacking = true;
        // Trigger charge animation
        anim.SetTrigger("Charge");

        // Implement your charge attack logic here
        Debug.Log("Charge attack!");
        Invoke(nameof(StopCharge), chargeDuration);
    }

    void StopCharge()
    {
        isAttacking = false;
    }

    private void PerformDiveAttack()
    {
        isAttacking = true;
        Debug.Log("Dive");
        // Check if the player is within a certain range to initiate the dive attack
        

            anim.SetTrigger("Dive");

            // You may want to implement a timer to control the duration of the dive attack
            StartCoroutine(EndDiveAttack());
    }


    private IEnumerator EndDiveAttack()
    {
        yield return new WaitForSeconds(diveDuration);


        isAttacking = false;
    }

    // Function to handle boss taking damage
    public void TakeDamage(int damage)
    {
        health -= damage;
        bossHealthbar.fillAmount = (float)health / maxHealth;

        if (health <= 0)
        {
            Die();
        }
    }

    // Function to handle boss death
    void Die()
    {
        anim.SetTrigger("Die");
        music.gameObject.SetActive(false);
        winText.gameObject.SetActive(true);
        navMeshAgent.enabled = false;
        audioSource.PlayOneShot(deathSound);
        Destroy(gameObject, 4f);
        PlayerController1.Instance.UpdateScore(500);
        isDead = true;
    }
}
