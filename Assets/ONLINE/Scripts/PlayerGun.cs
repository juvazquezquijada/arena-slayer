using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerGun : Gun
{
    public Camera cam;
    public Animator animator;
    public string shootAnimationName, reloadAnimationName;
    public AudioClip shootSound, reloadSound;
    public AudioSource gunSound;
    public int maxAmmo, lowAmmoThreshold;
    public int currentAmmo;
    public Image ammoBar;
    public TextMeshProUGUI ammoText, lowAmmoText, outOfAmmoText, reloadingText;
    public float reloadTime, fireRate;
    public Transform gunBarrel;
    public GameObject bulletPrefab;
    public float bulletLifetime;
    public float bulletSpeed;
    public int ammoValue;
    public ParticleSystem muzzleFlashParticleSystem;
    public bool isReloading = false;
    private float lastFireTime; // Time when the gun was last fired

    void Start()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoUI();
    }

    void Update()
    {
        currentAmmo = Mathf.Clamp(currentAmmo, 0, maxAmmo);
    }
    
    public override void Use()
    {
        if (isReloading)
        {
            return;
        }

        if (currentAmmo > 0 && Time.time - lastFireTime >= fireRate) // Check if enough time has passed since the last shot and if there is ammo available
        {
            lastFireTime = Time.time; // Update the last fire time

            Shoot(); // Perform the shooting logic
        }

        
    }

    public void UpdateAmmoUIOnSwitch()
    {
        UpdateAmmoUI();
    }

    void Shoot()
    {
        currentAmmo--;

        UpdateAmmoUI();
        
        // play shoot effects
        animator.SetTrigger(shootAnimationName);
        gunSound.PlayOneShot(shootSound);

        //Spawn bullet at gun barrel
        GameObject bullet = Instantiate(bulletPrefab, gunBarrel.position, Quaternion.identity);

        //Set the bullet's rotation to be the same as the player's rotation
        bullet.transform.rotation = transform.rotation;

        //Add velocity to the bullet
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = transform.forward * bulletSpeed;

        //Destroy bullet after 2 seconds
        Destroy(bullet, bulletLifetime);
        PlayMuzzleFlash();
    }

    public override void Reload()
    {
        if (isReloading || currentAmmo == maxAmmo)
        {
            // Cannot reload while already reloading or if ammo is already full
            return;
        }

        isReloading = true;

        // Play reload sound
        gunSound.PlayOneShot(reloadSound);

        // Wait for the reload time and then refill the ammo
        StartCoroutine(RefillAmmo());

        reloadingText.gameObject.SetActive(true);

        animator.SetTrigger(reloadAnimationName);
        gunSound.PlayOneShot(reloadSound);
    }

    IEnumerator RefillAmmo()
    {
        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        ammoBar.fillAmount = 1f;
        isReloading = false;
        UpdateAmmoUI();
    }




    void UpdateAmmoUI()
    {
        if (ammoText == null || ammoBar == null || lowAmmoText == null || outOfAmmoText == null)
        {
            // One of the UI elements is null, so return to avoid the error
            return;
        }

        ammoText.text = currentAmmo.ToString();
        ammoBar.fillAmount = (float)currentAmmo / maxAmmo;

        // Update UI indicators based on ammo count
        if (currentAmmo <= lowAmmoThreshold && currentAmmo > 0)
        {
            lowAmmoText.gameObject.SetActive(true);
        }
        else
        {
            lowAmmoText.gameObject.SetActive(false);
        }

        if (currentAmmo == 0 && !isReloading)
        {
            outOfAmmoText.gameObject.SetActive(true);
        }
        else
        {
            outOfAmmoText.gameObject.SetActive(false);
        }

        if (!isReloading)
        {
            reloadingText.gameObject.SetActive(false);
        }
    }

    void PlayMuzzleFlash()
    {
        // Play the muzzle flash locally
        muzzleFlashParticleSystem.Play();
    }

    IEnumerator StopMuzzleFlash()
    {
        // Wait for a short duration (e.g., 0.1 seconds) and then stop the muzzle flash locally
        yield return new WaitForSeconds(0.1f);

        muzzleFlashParticleSystem.Stop();
    }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ammo"))
            {
                currentAmmo += ammoValue;
                if (currentAmmo > maxAmmo) currentAmmo = maxAmmo;
                UpdateAmmoUI();
                Destroy(other.gameObject);
            }
        }
}

