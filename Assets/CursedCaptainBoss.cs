using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursedCaptainBoss : MonoBehaviour
{
    private enum BossState
    {
        Idle,
        Shooting,
        Grabbing,
        Summoning,
        AOE,
    }

    private BossState currentState;
    private Transform player;
    private float currentCooldown;
    private float curseDuration = 20f;
    private float curseCooldown = 1f;
    private float grabCooldown = 5f;
    private float summonCooldown = 10f;
    private float aoeCooldown = 15f;
    private float timeBetweenShots = 0.3f;
    private float bulletSpeed = 40f;
    public float curseballSpeed = 10f;
    public GameObject cursedBullet;
    public Transform gunBarrel;
    public GameObject grabAttackHitbox;
    public GameObject soldierPrefab;
    public Transform[] soldierSpawnPoints;
    public GameObject aoeAttackPrefab;
    public Transform aoeAttackSpawnPoint;

    bool isShooting = false;
    public AudioSource audioSource;
    public AudioClip gunSound;
    public Animator anim;
    

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentState = BossState.Idle;
    }

    private void Update()
    {
        switch (currentState)
        {
            case BossState.Idle:
                // Implement idle behavior or transition to other states based on conditions
                break;

            case BossState.Shooting:
                if (currentCooldown <= 0f)
                {
                    ShootCurseBullet();
                    currentCooldown = curseCooldown;
                }
                else
                {
                    currentCooldown -= Time.deltaTime;
                }
                break;

            case BossState.Grabbing:
                if (currentCooldown <= 0f)
                {
                    GrabAttack();
                    currentCooldown = grabCooldown;
                }
                else
                {
                    currentCooldown -= Time.deltaTime;
                }
                break;

            case BossState.Summoning:
                if (currentCooldown <= 0f)
                {
                    SummonSoldiers();
                    currentCooldown = summonCooldown;
                }
                else
                {
                    currentCooldown -= Time.deltaTime;
                }
                break;

            case BossState.AOE:
                if (currentCooldown <= 0f)
                {
                    AOEAttack();
                    currentCooldown = aoeCooldown;
                }
                else
                {
                    currentCooldown -= Time.deltaTime;
                }
                break;
        }
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
        anim.SetTrigger("Shoot");
        for (int i = 0; i < 5; i++) // Shoot 10 bullets
        {
            // Instantiate a bullet from the demon's right hand and shoot it towards the player
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
        // Implement logic for the grab attack
        // Check if the grab is successful, drain the Slayer's health, and regenerate boss health
        // Implement break free mechanic
    }

    private void SummonSoldiers()
    {
        // Implement logic to summon soldier enemies at random spawn points
        // Instantiate soldiers from soldierPrefab and set their behavior
    }

    private IEnumerator AOEAttack()
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
}

