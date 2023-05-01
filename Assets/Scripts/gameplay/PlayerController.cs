using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Basic player movement
    public float moveSpeed = 5.0f;
    public float shiftSpeed = 8.0f;
    public float gravity = 9.81f;
    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;

    //Audio clips the player makes
    public AudioClip deathSound;
    public AudioClip healthPickup;
    private AudioSource audioSource;
    public AudioClip hurtSound;
    //Status of the player
    public int health = 100;   
    public int score = 1;
    public int points = 0;
    public bool isDead = false;
    private CanvasManager canvasManager;
    private Rigidbody rb;
    private bool hasPlayedDeathSound = false;
    

    void Start()
    {
        //Get AudioSource component
        
        //Get CharacterController component
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        canvasManager = CanvasManager.Instance;
        CanvasManager.Instance.UpdateHealth(health);
        CanvasManager.Instance.UpdateScore(score);
        Application.targetFrameRate = 60;
        audioSource = GetComponent<AudioSource>();
       
    }

    void Update()
    {
        //controls the players health
        if (health <= 0)
        {
            Die();
            SpawnManager.Instance.GameOver();
            CanvasManager.Instance.GameOver();
            if (!hasPlayedDeathSound) 
            {
                audioSource.PlayOneShot(deathSound);
                hasPlayedDeathSound = true;
                
            }

        }
        else if (health < 40)
        {
            CanvasManager.Instance.LowHealth();
        }
        else if (health > 40)
        {
            CanvasManager.Instance.HasHealth();
        }
        // check if the player is dead
        if (isDead == true) 
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
            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveDirection *= shiftSpeed;
            }
            else
            {
                moveDirection *= moveSpeed;
            }

            //Apply gravity
            moveDirection.y -= gravity * Time.deltaTime;

        //Move the character controller
        controller.Move(moveDirection * Time.deltaTime);
        }
        
    }

    
    
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
        }
    } 

    
    public void UpdateHealth (int health)
    {
        CanvasManager.Instance.UpdateHealth(health);
    }
    
    void Die()
    {
            isDead = true;
            Debug.Log("Player is Dead!");
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<CharacterController>().enabled = false;
            GetComponent<PlayerShooting>().isDead = true;
            rb.velocity = Vector3.zero; // stop player movement
            rb.angularVelocity = Vector3.zero;
            
            
    }


    private void OnTriggerEnter(Collider other)
{

    if (other.gameObject.CompareTag("Health"))
    {
        health += 25;
        if (health > 100) health = 100;
        CanvasManager.Instance.UpdateHealth(health);
        Destroy(other.gameObject);
        audioSource.PlayOneShot(healthPickup);
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
    public void Health()
    {
        
    }
}

        
    

        
   

