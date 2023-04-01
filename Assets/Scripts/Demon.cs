using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon : MonoBehaviour
{
    public GameObject fireballPrefab;
    public float moveSpeed = 3f;
    public float fireballSpeed = 10f;
    public float fireballInterval = 2f;
    public float knockbackForce = 10f;
    private Transform player;
    private float lastFireballTime = 0f;
    private bool isDead = false;
    public float destroyTime = 20f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return; // Don't do anything if the enemy is dead

        // Move towards the player
        transform.LookAt(player);
        transform.position += transform.forward * moveSpeed * Time.deltaTime;

        // Shoot fireballs at the player
        if (Time.time - lastFireballTime > fireballInterval)
        {
            lastFireballTime = Time.time;
            GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
            fireball.GetComponent<Rigidbody>().velocity = (player.position - transform.position).normalized * fireballSpeed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDead) return; // Don't do anything if the enemy is dead

        if (other.gameObject.CompareTag("Bullet"))
        {
            // Apply knockback force
            Vector3 knockbackDirection = (transform.position - other.transform.position).normalized;
            GetComponent<Rigidbody>().AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);

            // Die
            Die();
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Game over!");
        }
    }

    private void Die()
    {
    isDead = true;

    // Disable the enemy's collider and renderer
    GetComponent<BoxCollider>().enabled = false;
    
    // Apply a force to launch the enemy in the air
    Rigidbody rb = GetComponent<Rigidbody>();
    rb.isKinematic = false;
    Vector3 knockbackDirection = transform.up + transform.forward * 0.5f;
    rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);

    // Destroy the enemy after a delay
    Destroy(gameObject, destroyTime);
    }
}

