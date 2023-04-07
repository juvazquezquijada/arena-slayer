using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float destroyTime = 5;
    public float knockbackForce;
    public int scoreValue = 1;
    public int health = 1;
    private AudioSource audioSource;
    public AudioClip deathSound;
    public float moveSpeed = 1;
    private Transform player;
    public bool isDead = false;
    private bool playerDead= false;
    public Transform bulletSpawn;
    public float fireRate = 3;
    public float bulletSpeed = 30;
    public float lastBulletTime = 0;
    public AudioClip gunSound;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return; // Don't do anything if the enemy is dead
        if (playerDead) return; //Don't do anything if the player is dead
        // Move towards the player
        transform.LookAt(player);
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
        if (Time.time - lastBulletTime > fireRate)
        {
            lastBulletTime = Time.time;
           GameObject Bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
            Bullet.GetComponent<Rigidbody>().velocity = (player.position - transform.position).normalized * bulletSpeed;
            audioSource.PlayOneShot(gunSound);
        }
    }

    private void OnTriggerEnter(Collider other)
{
    if (isDead) return; // Don't do anything if the enemy is dead
    if (playerDead) return; //Don't do anything if the player is dead     

    if (other.gameObject.CompareTag("Bullet"))
    {
        // Die
        Die();
        CanvasManager.Instance.UpdateScore(1);
    }
}

    public void Die()
    {
         isDead = true;

         // Play death sound
         audioSource.PlayOneShot(deathSound);

         // Disable the enemy's collider and renderer
         GetComponent<CapsuleCollider>().enabled = false;
    
         // Apply a force to launch the enemy in the air
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        Vector3 knockbackDirection = transform.up + transform.forward * 0.5f;
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);

          // Destroy the enemy after a delay
          Destroy(gameObject, destroyTime);
    }
} 
    


