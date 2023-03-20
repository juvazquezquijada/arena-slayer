using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombieman: MonoBehaviour
{
    public float speed = 2.0f; // Speed of the zombie
    public int health = 50; // Health of the zombie
    public float attackRange = 2.0f; // Range of zombie's attack
    public int damage = 10; // Damage of zombie's attack
    public float attackCooldown = 2.0f; // Cooldown between zombie's attacks
    public float shotCooldown = 2.0f; // Cooldown between zombie's shots
    public Transform target; // Player character's transform
    public GameObject bulletPrefab; // Prefab of the zombie's bullet
    public Transform firePoint; // Transform representing where the zombie's bullet is fired from
    public float bulletSpeed = 10.0f; // Speed of the zombie's bullet
    public AudioClip gunshotSound; // Plays a sound when the zombie shoots

    private bool canAttack = true; // Whether the zombie can currently attack or not
    private bool canShoot = true; // Whether the zombie can currently shoot or not
    private bool isDead = false; // Whether the zombie is currently dead or not
  
    private AudioSource audioSource; // Reference to the zombies audio source

    // Start is called before the first frame update
    void Start()
    {
  
        audioSource = GetComponent<AudioSource>(); // Reference to the zombies audio source
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) // Do nothing if the zombie is dead
        {
            return;
        }

        if (target == null) // If the target is null, find the player character
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // Move the zombie towards the player
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // Rotate the zombie to face the player
        transform.LookAt(target);

        // If the zombie is within attack range and can attack, attack the player
        if (Vector3.Distance(transform.position, target.position) <= attackRange && canAttack)
        {
            StartCoroutine(Attack());
        }

        // If the zombie can shoot and is facing the player, shoot at the player
        if (canShoot && Vector3.Angle(transform.forward, target.position - transform.position) < 10.0f)
        {
            StartCoroutine(Shoot());
        }
    }

    // Coroutine for the zombie's attack
    IEnumerator Attack()
    {
        canAttack = false; // Set canAttack to false so the zombie can't attack while the coroutine is running
        target.GetComponent<PlayerHealth>().TakeDamage(damage); // Deal damage to the player
        yield return new WaitForSeconds(attackCooldown); // Wait for the attack cooldown
        canAttack = true; // Set canAttack back to true so the zombie can attack again
    }

    // Coroutine for the zombie's shot
    IEnumerator Shoot()
    {
        canShoot = false; // Set canShoot to false so the zombie can't shoot while the coroutine is running
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity); // Instantiate the zombie's bullet
        bullet.GetComponent<Rigidbody>().velocity = (target.position - firePoint.position).normalized * bulletSpeed; // Shoot the bullet towards the player
        yield return new WaitForSeconds(shotCooldown); // Wait for the shot cooldown
        canShoot = true; // Set canShoot back to true so the zombie can shoot again
        // Play a gunshot sound when the zombie shoots
        AudioSource.PlayClipAtPoint(gunshotSound, firePoint.position);

}
    // Function for taking damage
    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount; // Decrease the zombie's health by the damage amount

    if (health <= 0 && !isDead) // If the zombie's health is 0 or less, kill the zombie
    {
        Die();
    }
}

    // Function for killing the zombie
    void Die()
    {
        isDead = true; // Set isDead to true
        
        Destroy(gameObject, 3.0f); // Destroy the zombie after 2 seconds
    }
}


