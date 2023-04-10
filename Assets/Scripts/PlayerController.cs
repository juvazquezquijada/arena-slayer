using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Basic player movement
    public float moveSpeed = 5.0f;
    public float gravity = 9.81f;
    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;

    //Properties of the Pump Shotgun
    public Transform gunBarrel;
    public GameObject bulletPrefab;
    public float bulletSpeed = 50f;
    public float fireRate = 0.1f;
    public float bulletLifetime = 2f;
    private float fireTimer = 0f;
    public Animator shotgunAnimator;
    private bool shotFired = false;
    //Audio clips the player makes
    public AudioClip shootSound;
    public AudioClip deathSound;
    public AudioClip healthPickup;
    public AudioClip ammoPickup;
    private AudioSource audioSource;
    public AudioClip hurtSound;
    //Status of the player
    public int health = 100;
    public int maxAmmo = 25;
    private int currentAmmo = 0;
    public int score = 1;
    public int points = 0;
    public bool isDead = false;
    private CanvasManager canvasManager;
    private Rigidbody rb;
    private bool hasPlayedDeathSound = false;
    

    void Start()
    {
        //Get AudioSource component
        audioSource = GetComponent<AudioSource>();
        //Get CharacterController component
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        currentAmmo = maxAmmo;
        canvasManager = CanvasManager.Instance;
        CanvasManager.Instance.UpdateAmmo(currentAmmo);
        CanvasManager.Instance.UpdateHealth(health);
        CanvasManager.Instance.UpdateScore(score);
    }

    void Update()
    {
        if (health <= 0)
        {
            Die();
            SpawnManager.Instance.GameOver();
            if (!hasPlayedDeathSound) 
            {
                audioSource.PlayOneShot(deathSound);
                hasPlayedDeathSound = true;
            }

        }
        
        if (isDead == true) // check if the player is dead
        {
        moveDirection = Vector3.zero; // stop the movement
        return; // exit the method
        }
        else if (isDead == false)
        {
        //Get input axis
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        //Calculate the movement direction
        Vector3 forward = transform.forward * vertical;
        Vector3 right = transform.right * horizontal;
        moveDirection = forward + right;

        //Apply movement direction and speed
        moveDirection *= moveSpeed;

        //Apply gravity
        moveDirection.y -= gravity * Time.deltaTime;

        //Move the character controller
        controller.Move(moveDirection * Time.deltaTime);
        }
        //Shoot a bullet when the shoot button is pressed and ammo is more than 0 and the shooting cooldown is up 
        if (Input.GetButtonDown("Fire1") && fireTimer <= 0f && currentAmmo > 0 && isDead == false)
        {
            PlayShootAnimations();

            //Play shooting sound
            audioSource.PlayOneShot(shootSound);

            //Spawn bullet at gun barrel
            GameObject bullet = Instantiate(bulletPrefab, gunBarrel.position, gunBarrel.rotation);

            //Add velocity to the bullet
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.velocity = transform.forward * bulletSpeed;

            //Destroy bullet after 2 seconds
            Destroy(bullet, bulletLifetime);

            //Decrease the ammo stat
            currentAmmo--;

            //Update the ammo stat
            CanvasManager.Instance.UpdateAmmo(currentAmmo);
            
            //Reset the fire cooldown
            fireTimer = fireRate;
        }

        // Decrease the fire timer if the player can't shoot yet
        if (fireTimer > 0f)
        {
            fireTimer -= Time.deltaTime;
        }
    }

    public void SetShotFired(bool value)
    {
        shotFired = value;
    }

    public bool GetShotFired()
    {
        return shotFired;
    }

    public void ResetShotFired()
    {
        shotFired = false;
    }
    
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
        }
    } 

    private void PlayShootAnimations()
    {
        Animator anim = GetComponent<Animator>();
        anim.SetTrigger("Shoot");
        shotgunAnimator.SetTrigger("Shoot");
    }
    public void UpdateHealth (int health)
    {
        CanvasManager.Instance.UpdateHealth(health);
    }
    
    void UpdateAmmo()
    {
    CanvasManager.Instance.UpdateAmmo(currentAmmo);
    }

    void Die()
    {
            isDead = true;
            Debug.Log("Player is Dead!");
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<CharacterController>().enabled = false;
            rb.velocity = Vector3.zero; // stop player movement
            rb.angularVelocity = Vector3.zero;
            
    }


    private void OnTriggerEnter(Collider other)
{
   
    if (other.gameObject.CompareTag("Bullet"))
    {
        currentAmmo = maxAmmo;
        UpdateAmmo();
        Destroy(other.gameObject);
    }
    else if (other.gameObject.CompareTag("Health"))
    {
        health += 25;
        if (health > 100) health = 100;
        CanvasManager.Instance.UpdateHealth(health);
        Destroy(other.gameObject);
        audioSource.PlayOneShot(healthPickup);
    }
    else if (other.gameObject.CompareTag("Ammo"))
    {
        currentAmmo += 5;
        if (currentAmmo > 25) currentAmmo = 25;
        CanvasManager.Instance.UpdateAmmo(currentAmmo);
        Destroy(other.gameObject);
        audioSource.PlayOneShot(ammoPickup);
    }
    else if (other.gameObject.CompareTag("Fireball"))
    {
        health -= 5;
        if (health < 0) health = 0;
        CanvasManager.Instance.UpdateHealth(health);
        Destroy(other.gameObject);
        audioSource.PlayOneShot(hurtSound);
    }
    else if (other.gameObject.CompareTag("Demon"))
    {
        health -= 5;
        if (health < 0) health = 0;
        CanvasManager.Instance.UpdateHealth(health);
        audioSource.PlayOneShot(hurtSound);
    }
    else if (other.gameObject.CompareTag("Zombie"))
    {
        health -= 5;
        if (health < 0) health = 0;
        CanvasManager.Instance.UpdateHealth(health);
        audioSource.PlayOneShot(hurtSound);
    }
    else if (other.gameObject.CompareTag("Soldier"))
    {
        health -= 10;
        if (health < 0) health = 0;
        CanvasManager.Instance.UpdateHealth(health);
        audioSource.PlayOneShot(hurtSound);
    }
    else if (other.gameObject.CompareTag("EnemyProjectile"))
    {
        health -= 3;
        if (health < 0) health = 0;
        CanvasManager.Instance.UpdateHealth(health);
        audioSource.PlayOneShot(hurtSound);
    }
        
    }   
}

        
    

        
   

