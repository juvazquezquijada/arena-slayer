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
    private int currentAmmo = 75;
    public AudioClip ammoPickup;
    public AudioClip shootSound;
    public int maxAmmo = 75; 
    public bool isDead = false;
    private AudioSource audioSource;
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
        if (Input.GetButton("Fire1") && fireTimer <= 0f && currentAmmo > 0 && isDead == false)
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
        // controls the ammo indicators
        if (currentAmmo < 1)
            {
                CanvasManager.Instance.OutOfAmmo();
            }
        else if (currentAmmo < 5)
            {
                CanvasManager.Instance.LowAmmo();
            }
        else if (currentAmmo > 5)
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
         if (other.gameObject.CompareTag("Ammo"))
         {
        currentAmmo += 7;
        if (currentAmmo > 75) currentAmmo = 75;
        CanvasManager.Instance.UpdateAmmo(currentAmmo);
        Destroy(other.gameObject);
        audioSource.PlayOneShot(ammoPickup);
        } 
        
        else if (other.gameObject.CompareTag("Bullet"))
        {
        currentAmmo = maxAmmo;
        UpdateAmmo();
        Destroy(other.gameObject);
        }
    }
}

