using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonAI : MonoBehaviour
{
    public float moveSpeed = 3.0f;
    public float attackRange = 5.0f;
    public float fireballSpeed = 10.0f;
    public float fireballCooldown = 2.0f;
    public GameObject fireballPrefab;
    
    public GameObject player;
    private Rigidbody demonRigidbody;
    private float nextFireballTime = 0.0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        demonRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance <= attackRange)
        {
            // Stop moving and perform melee attack
            demonRigidbody.velocity = Vector3.zero;
            Debug.Log("Demon is doing melee attack");

            // Insert code for melee attack here
        }
        else
        {
            // Move towards player
            Move();
            
            // Check if we can perform a ranged attack
            if (Time.time > nextFireballTime)
            {
                nextFireballTime = Time.time + fireballCooldown;
                PerformRangedAttack();
            }
        }
    }

    void Move()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        demonRigidbody.velocity = direction * moveSpeed;
        transform.LookAt(player.transform.position);
        Debug.Log("Demon is moving towards player");
    }

    void PerformRangedAttack()
    {
        // Spawn a fireball prefab and set its velocity towards the player
        GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
        Vector3 direction = (player.transform.position - transform.position).normalized;
        fireball.GetComponent<Rigidbody>().velocity = direction * fireballSpeed;
        Debug.Log("Demon is throwing a fireball");
    }
}
