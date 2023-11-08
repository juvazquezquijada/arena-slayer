using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltFire : MonoBehaviour
{
    public SingleArmGun gun;
    public int burstCount = 6; // Number of shots in each burst
    public float timeBetweenShots = 0.1f; // Time between each shot in the burst
    public float burstCooldown = 1.0f; // Time between consecutive bursts
    public float burstAnimatorSpeed = 2.0f; // Animator speed during burst
    private int shotsFiredInBurst;
    private float timeSinceLastShot;
    private float timeSinceLastBurst;
    private bool isFiring;
    private float originalAnimatorSpeed;

    // Start is called before the first frame update
    void Start()
    {
        shotsFiredInBurst = 0;
        timeSinceLastShot = 0;
        timeSinceLastBurst = 0;
        isFiring = false;

        // Store the original animator speed
        originalAnimatorSpeed = gun.animator.speed;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastBurst += Time.deltaTime;

        if (Input.GetMouseButtonDown(1) && timeSinceLastBurst >= burstCooldown)
        {
            StartBurst();
        }

        if (isFiring)
        {
            FireShot();
        }
    }

    void StartBurst()
    {
        if (shotsFiredInBurst == 0)
        {
            isFiring = true;
            gun.fireRate = 0.1f; // Change the fire rate to 0.1 during the burst
            gun.animator.speed = burstAnimatorSpeed; // Set the Animator speed during burst
            timeSinceLastBurst = 0; // Reset the burst cooldown timer
        }
    }

    void FireShot()
    {
        if (shotsFiredInBurst < burstCount && timeSinceLastShot >= timeBetweenShots)
        {
            // Fire a shot
            gun.Use();
            shotsFiredInBurst++;
            timeSinceLastShot = 0;
        }
        else if (shotsFiredInBurst == burstCount)
        {
            // Burst fire completed, reset burst
            shotsFiredInBurst = 0;
            isFiring = false;
            gun.fireRate = 0.3f; // Restore the original fire rate
            gun.animator.speed = originalAnimatorSpeed; // Restore the original Animator speed
        }
    }

    void UpdateTimeSinceLastShot()
    {
        timeSinceLastShot += Time.deltaTime;
    }

    void FixedUpdate()
    {
        UpdateTimeSinceLastShot();
    }
}
