using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class CursedCaptainBoss : MonoBehaviour
{
   
    private Transform player;
    //cooldowns
    private float currentCooldown;
    private float curseCooldown = 1f;
    private float grabCooldown = 5f;
    private float tripCooldown = 5f;
    private float summonCooldown = 5f;
    private float aoeCooldown = 15f;
    private float timeBetweenShots = 0.3f;
    private float bulletSpeed = 40f;
    public float curseballSpeed = 10f;
    public float meleeRange = 2f;
    private float grabDuration = 2f; // Adjust the duration of the grab attack as needed
    private float grabTimer = 0f; // Timer to track how long the grab has been active
    public int maxSoldiersToSummon = 3; // Adjust the maximum number of soldiers to summon
    private int soldiersSummoned = 0; // Keep track of the number of soldiers summoned

    //prefabs
    public GameObject cursedBullet;
    public Transform gunBarrel;
    public GameObject grabAttackHitbox;
    public GameObject soldierPrefab;
    public Transform[] soldierSpawnPoints;
    public GameObject aoeAttackPrefab;
    public Transform aoeAttackSpawnPoint;
    public GameObject captainHand;
    // health
    public int health;
    public int maxHealth = 300;
    public Image healthbar;

    // bools
    public bool isDead = false;
    private bool playerDead = false;
    private bool canAttack = true;
    private bool isCharging = false;
    private bool isInSecondPhase = false;
    private bool isGrabbing = false; // Add a flag to control the grab attack
    private bool isShooting = false;
    private bool isSummoning = false; // Add a flag to control the summoning attack
    private bool isTripping = false;
    // audio/visual
    public AudioSource audioSource;
    public AudioClip gunSound;
    public AudioClip summonSound;
    public Animator anim;
    public GameObject winText;
    public AudioClip deathSound;
    public GameObject music;

    //misc
    private Rigidbody rb;
    private PlayerController1 playerHealth;
    private NavMeshAgent navMeshAgent; // Reference to the NavMeshAgent component
    public Collider grabHitbox; // Reference to the grab hitbox collider


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
                int randomAttack = Random.Range(0, 5);
                switch (randomAttack)
                {
                    case 0:
                        ShootCurseBullet();
                        currentCooldown = curseCooldown;
                        break;
                    case 1:
                        GrabAttack();
                        currentCooldown = grabCooldown;
                        break;
                    case 2:
                        SummonSoldiers();
                        currentCooldown = summonCooldown;
                        break;
                    case 3:
                        AOEAttack();
                        currentCooldown = aoeCooldown;
                        break;
                    case 4:
                        Trip();
                        currentCooldown = tripCooldown;
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
    public void TakeDamage(int damage)
    {
        Debug.Log("Captain took damage" + damage);
        health -= damage;
        healthbar.fillAmount = (float)health / maxHealth;

        if (health <= 0)
        {
            Die();
        }
    }
    private void MeleeAttack()
    {
        // Trigger melee attack animation
        //anim.SetTrigger("MeleeAttack");

        // Implement your melee attack logic here
        Debug.Log("Melee attack!");
    }


    private void ShootCurseBullet()
    {
        if (!isShooting)
        {
            StartCoroutine(AutoShootBullets());
        }
    }

    private IEnumerator AutoShootBullets()
    {
        isShooting = true;
        
        for (int i = 0; i < 5; i++) // Shoot 10 bullets
        {
            // Instantiate a bullet from the demon's right hand and shoot it towards the player
            Debug.Log("shooting");
            GameObject newBullet = Instantiate(cursedBullet, gunBarrel.position, Quaternion.identity);
            Rigidbody bulletRigidbody = newBullet.GetComponent<Rigidbody>();
            bulletRigidbody.velocity = (player.position - gunBarrel.position).normalized * bulletSpeed;
            audioSource.PlayOneShot(gunSound);
            yield return new WaitForSeconds(timeBetweenShots); // Add a delay between shots
        }

        isShooting = false; // Reset the shooting flag after the sequence
    }

    private void GrabAttack()
    {
        // Play the grab animation
        //anim.SetTrigger("Grab");
        if (!isGrabbing)
        {
            // Start the grab attack
            isGrabbing = true;
            grabHitbox.enabled = true; // Enable the grab hitbox
            grabTimer = 0f;
        }
    }
    public void MovePlayerToCaptainHand()
    {
        // Move the player to the position of the captain's hand
        player.transform.position =  captainHand.transform.position;
    }

    public IEnumerator PerformGrab()
    {
        while (grabTimer < grabDuration)
        {
            Debug.Log("is grabbing");
            // Calculate damage per second
            int damagePerSecond = 5;

            // Apply damage to the player
            playerHealth.TakeDamage(damagePerSecond);

            grabTimer += Time.deltaTime;
            yield return null;
        }

        // End the grab attack
        isGrabbing = false;
    }


    private void SummonSoldiers()
    {
        if (!isSummoning && soldiersSummoned < maxSoldiersToSummon)
        {
            // Start the summoning attack
            isSummoning = true;

            StartCoroutine(PerformSummon());
        }
    }

    private IEnumerator PerformSummon()
    {

        Debug.Log("summoning");
        //anim.SetTrigger("Summon");
        audioSource.PlayOneShot(summonSound);

        // Randomly select a spawn point for the soldier
        Transform spawnPoint = soldierSpawnPoints[Random.Range(0, soldierSpawnPoints.Length)];

        // Instantiate a soldier at the chosen spawn point
        GameObject newSoldier = Instantiate(soldierPrefab, spawnPoint.position, Quaternion.identity);

        // You can set behaviors or scripts for the soldier here
        newSoldier.GetComponent<Soldier>().SetTarget();

        soldiersSummoned++;

        // Wait for a short delay before allowing the next summon
        yield return new WaitForSeconds(2f);

        // End the summoning attack
        isSummoning = false;
    }

    private IEnumerator AOEAttack()
    {
        Debug.Log("aoe attack");
       // anim.SetTrigger("AOEAttack");
        yield return new WaitForSeconds(2f);

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
            Vector3 fireballVelocity = spawnDirection * curseballSpeed;

            // Spawn the fireball
            SpawnFireball(spawnPosition, fireballVelocity);
        }
    }

    private void SpawnFireball(Vector3 position, Vector3 velocity)
    {
        GameObject curseball = Instantiate(aoeAttackPrefab, position, Quaternion.identity);
        curseball.transform.rotation = Quaternion.identity;
        curseball.GetComponent<Rigidbody>().velocity = velocity;
    }

    private void Trip()
    {
        //anim.SetTrigger("Trip");

        Debug.Log("tripped");
    }

    void Die()
    {
        //anim.SetTrigger("Die");
        music.gameObject.SetActive(false);
        winText.gameObject.SetActive(true);
        navMeshAgent.enabled = false;
        audioSource.PlayOneShot(deathSound);
        Destroy(gameObject, 4f);
        PlayerController1.Instance.UpdateScore(500);
        isDead = true;
        Debug.Log("died");
    }
}

