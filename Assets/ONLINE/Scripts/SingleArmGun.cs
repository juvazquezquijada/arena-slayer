using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SingleArmGun : Gun
{
    [SerializeField] Camera cam;
    [SerializeField] public Animator animator;
    [SerializeField] string shootAnimationName;
    [SerializeField] public float fireRate;
    [SerializeField] AudioClip shootSound;
    [SerializeField] AudioSource gunSound;
    public int maxAmmo, lowAmmoThreshold;
    public int currentAmmo;
    [SerializeField] Image ammoBar;
    [SerializeField] TextMeshProUGUI ammoText, lowAmmoText, outOfAmmoText, reloadingText;
    [SerializeField] float reloadTime;
    [SerializeField] AudioClip reloadSound;
    [SerializeField] string reloadAnimationName;
    [SerializeField] float shootDelay = 0.3f; // Delay before shooting (adjust this delay as needed)
    private bool isReloading = false; // Reset the reloading flag
    private bool isUsingWeapon = false;

    public GameObject leftArm, rightArm, otherLeftArm, otherRightArm;

    [SerializeField] ParticleSystem muzzleFlashParticleSystem;
    public float maxRaycastDistance = 100f; // Replace with your desired max distance

    PhotonView PV;

    private float lastFireTime; // Time when the gun was last fired

    void Start()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoUI();
    }

    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    void Update()
    {
        currentAmmo = Mathf.Clamp(currentAmmo, 0, maxAmmo);

        if (Time.time - lastFireTime >= fireRate)
        {
            isUsingWeapon = false;
        }
    }
    public bool IsReloadingOrShooting()
    {
        return isReloading || isUsingWeapon;
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

            StartCoroutine(ShootWithDelay());
            
        }

        
    }

    public void UpdateAmmoUIOnSwitch()
    {
        UpdateAmmoUI();
    }

    IEnumerator ShootWithDelay()
    {
        PV.RPC("RPC_PlayShootEffects", RpcTarget.All);
        // Wait for the shoot delay
        yield return new WaitForSeconds(shootDelay);

        // Perform the shooting logic
        Shoot();
    }

    void Shoot()
    {
        currentAmmo--;
        UpdateAmmoUI();

        isUsingWeapon = true; // Mark that the weapon is in use for shooting

        //shoot ray
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 3f));
        ray.origin = cam.transform.position;

        if (Physics.Raycast(ray, out RaycastHit hit, maxRaycastDistance))
        {
            // Check if the hit point is beyond a minimum distance
            float minRaycastDistance = 0.5f; // Set your desired minimum distance here
            float distanceToHit = Vector3.Distance(ray.origin, hit.point);

            if (distanceToHit >= minRaycastDistance)
            {
                Debug.Log("We hit " + hit.collider.gameObject.name);
                hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)itemInfo).damage);
                PV.RPC("RPC_Shoot", RpcTarget.All, hit.point, hit.normal);
            }
            else
            {
                // Handle the case where the hit is too close (e.g., play a different sound, effect, or do nothing)
            }
        }

        PlayMuzzleFlash();
    }

    public override void Reload()
    {
        if(isReloading || currentAmmo == maxAmmo || Time.time - lastFireTime < fireRate|| isUsingWeapon)
        {
            // Cannot reload while already reloading or if ammo is already full
            return;
        }

        isReloading = true;

        // Play reload sound
        gunSound.PlayOneShot(reloadSound);

        // Wait for the reload time and then refill the ammo
        StartCoroutine(RefillAmmo());

        PV.RPC("RPC_PlayReloadEffects", RpcTarget.All);

        reloadingText.gameObject.SetActive(true);
    }

    IEnumerator RefillAmmo()
    {
        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        if (!ammoBar == null)
        {
            ammoBar.fillAmount = 1f;
        }
        isReloading = false; // Reset the reloading flag
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

    [PunRPC]
    void RPC_Shoot(Vector3 hitPosition, Vector3 hitNormal)
    {
        Collider[] colliders = Physics.OverlapSphere(hitPosition, 0.3f);
        if (colliders.Length != 0)
        {
            GameObject bulletImpactObj = Instantiate(bulletImpactPrefab, hitPosition + hitNormal * 0.001f, Quaternion.LookRotation(hitNormal, Vector3.up) * bulletImpactPrefab.transform.rotation);
            Destroy(bulletImpactObj, 10f);
            bulletImpactObj.transform.SetParent(colliders[0].transform);
        }
    }

    [PunRPC]
    void RPC_PlayShootEffects()
    {
        animator.SetTrigger(shootAnimationName);
        gunSound.PlayOneShot(shootSound);
    }

    [PunRPC]
    void RPC_PlayReloadEffects()
    {
        animator.SetTrigger(reloadAnimationName);
        gunSound.PlayOneShot(reloadSound);
    }
    void PlayMuzzleFlash()
    {
        // Play the muzzle flash locally
        muzzleFlashParticleSystem.Play();

        // Call an RPC to play the muzzle flash on all clients
        PV.RPC("RPC_PlayMuzzleFlash", RpcTarget.All);
    }

    public void ShowAltArms()
    {
        PV.RPC("RPC_ShowAltArms", RpcTarget.All);
    }

    [PunRPC]
    void RPC_ShowAltArms()
    {
        leftArm.gameObject.SetActive(true);
        rightArm.gameObject.SetActive(true);
        otherLeftArm.gameObject.SetActive(false);
        otherRightArm.gameObject.SetActive(false);
    }

    [PunRPC]
    void RPC_PlayMuzzleFlash()
    {
        // Play the muzzle flash for all clients
        muzzleFlashParticleSystem.Play();

        // Stop the muzzle flash after a short duration
        StartCoroutine(StopMuzzleFlash());
    }
    IEnumerator StopMuzzleFlash()
    {
        // Wait for a short duration (e.g., 0.1 seconds) and then stop the muzzle flash locally
        yield return new WaitForSeconds(0.1f);

        muzzleFlashParticleSystem.Stop();

        // Call an RPC to stop the muzzle flash on all clients
        PV.RPC("RPC_StopMuzzleFlash", RpcTarget.All);
    }

    [PunRPC]
    void RPC_StopMuzzleFlash()
    {
        // Stop the muzzle flash for all clients
        muzzleFlashParticleSystem.Stop();
    }

}

