using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaGun : MonoBehaviour
{
    //Properties of the Plasma gun
    public Transform gunBarrel;
    public GameObject bulletPrefab;
    public float bulletSpeed = 50f;
    public float fireRate = 0.1f;
    public float bulletLifetime = 2f;
    private float fireTimer = 0f;
    public Animator shotgunAnimator;
    private bool shotFired = false;
    public int currentAmmo = 40;
    public AudioClip ammoPickup;
    public AudioClip shootSound;
    public int maxAmmo = 75; 
    public bool isDead = false;
    private AudioSource audioSource;
    public int lowAmmoThreshhold = 25;
    // Start is called before the first frame update
    void Start()
    {
       CanvasManager.Instance.UpdateAmmo(currentAmmo); 
       audioSource = GetComponent<AudioSource>();
       currentAmmo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0f)
        {
            // Don't shoot the gun if the game is paused
            return;
        }

        //Shoot a bullet when the shoot button is pressed and ammo is more than 0 and the shooting cooldown is up 
        if ((Input.GetButton("Fire1") || Mathf.Abs(Input.GetAxis("JoystickAxis10")) > 0.5f) && fireTimer <= 0f && currentAmmo > 0 && !isDead)
        {
            Shoot();
        }
        
        
        // controls the ammo indicators
        if (currentAmmo < 1)
            {
                CanvasManager.Instance.OutOfAmmo();
            }
        else if (currentAmmo < lowAmmoThreshhold)
            {
                CanvasManager.Instance.LowAmmo();
            }
        else if (currentAmmo > lowAmmoThreshhold)
        {
            CanvasManager.Instance.HasAmmo();
        }

        // Decrease the fire timer if the player can't shoot yet
        if (fireTimer > 0f)
        {
            fireTimer -= Time.deltaTime;
        }
    }

    private void PlayShootAnimations()
    {
        Animator anim = GetComponent<Animator>();
        anim.SetTrigger("ShootPlasma");
        shotgunAnimator.SetTrigger("ShootPlasma");
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
    void UpdateAmmo()
    {
    CanvasManager.Instance.UpdateAmmo(currentAmmo);
    }
    
    private void OnTriggerEnter(Collider other)
        {
         if (other.CompareTag("Ammo"))
         {
        currentAmmo += 25;
        if (currentAmmo > maxAmmo) currentAmmo = maxAmmo;
        CanvasManager.Instance.UpdateAmmo(currentAmmo);
        Destroy(other.gameObject);
        audioSource.PlayOneShot(ammoPickup);
        } 
        
        else if (other.CompareTag("Bullet"))
        {
        currentAmmo = maxAmmo;
        UpdateAmmo();
        Destroy(other.gameObject);
        }
    }
    public int GetCurrentAmmo()
    {
        // Return the current ammo count for the plasma gun
        return currentAmmo;
    }

    public void Shoot()
    {   
        
        PlayShootAnimations();

        //Play shooting sound
        audioSource.PlayOneShot(shootSound);

        //Spawn bullet at gun barrel
        GameObject bullet = Instantiate(bulletPrefab, gunBarrel.position, Quaternion.identity);

        //Set the bullet's rotation to be the same as the player's rotation
        bullet.transform.rotation = transform.rotation;

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

}

