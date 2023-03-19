using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float jumpForce = 5.0f;
    public float gravity = 9.81f;

    // These variables are for when the player shoots with Ctrl
    public Transform gunBarrel;  // The position where the bullets will be spawned
    public GameObject bulletPrefab;  // The bullet prefab to be spawned
    public float bulletSpeed = 50f;  // The speed of the bullet
    public float fireRate = 0.1f;  // The delay between shots
    public float bulletLifetime = 2f; // The amount of time the bullet will exist before being destroyed
    private float fireTimer = 0f;  // Timer to track when the player can shoot again
    public AudioClip shootSound; // The sound to play when shooting
    private AudioSource audioSource; // Reference to the AudioSource component
    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    public Animator shotgunAnimator; // Reference to the Animator component on the shotgun model
    private bool shotFired = false;
   
   
    void Start()
    {
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();
        // Get the CharacterController component
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Get the input axes
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Calculate the movement direction
        Vector3 forward = transform.forward * vertical;
        Vector3 right = transform.right * horizontal;
        moveDirection = forward + right;

        // Apply the movement direction and speed
        moveDirection *= moveSpeed;

        // Apply gravity
        moveDirection.y -= gravity * Time.deltaTime;

        // Check for jump input
        if (controller.isGrounded && Input.GetButtonDown("Jump"))
        {
            moveDirection.y = jumpForce;
        }

        // Move the character controller
        controller.Move(moveDirection * Time.deltaTime);
            
        // Check if the player is pressing the fire (Ctrl) button and if enough time has passed since the last shot
        if (Input.GetButtonDown("Fire1") && fireTimer <= 0f)
        {
            PlayShootAnimations();

            // Play the shooting sound
            audioSource.PlayOneShot(shootSound);

            // Spawn a bullet at the gun barrel position
            GameObject bullet = Instantiate(bulletPrefab, gunBarrel.position, gunBarrel.rotation);

            // Add velocity to the bullet
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.velocity = transform.forward * bulletSpeed;

            // Destroy the bullet after a certain amount of time
            Destroy(bullet, bulletLifetime);

            // Reset the fire timer
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

    private void PlayShootAnimations()
    {
        Animator anim = GetComponent<Animator>();
        anim.SetTrigger("Shoot");
        shotgunAnimator.SetTrigger("Shoot");
    }
}
