using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SingleShotGun : Gun
{
    [SerializeField] Camera cam;
    [SerializeField] Animator animator;
    [SerializeField] string shootAnimationName;
    [SerializeField] float fireRate;
    [SerializeField] AudioClip shootSound;
    [SerializeField] AudioSource gunSound;
    public int maxAmmo, lowAmmoThreshold;
    public int currentAmmo;
    [SerializeField] Image ammoBar;
    [SerializeField] TextMeshProUGUI ammoText, lowAmmoText, outOfAmmoText;
    [SerializeField] float reloadTime;
    [SerializeField] AudioClip reloadSound;
    [SerializeField] string reloadAnimationName;
    private bool isReloading = false;

    PhotonView PV;

    private float lastFireTime; // Time when the gun was last fired



    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    void Update()
    {
        currentAmmo = Mathf.Clamp(currentAmmo, -1, maxAmmo);

        if (Input.GetKeyDown(KeyCode.R) && !isReloading && currentAmmo < maxAmmo)
        {
            Reload();
        }

    }

    public override void Use()
    {
        if (isReloading)
        {
            return;
        }

        if (currentAmmo > -1 && Time.time - lastFireTime >= fireRate) // Check if enough time has passed since the last shot and if there is ammo available
        {
            lastFireTime = Time.time; // Update the last fire time

            Shoot(); // Perform the shooting logic
        }
    }

    void Shoot()
    {
        if (currentAmmo <= -1)
        {
            // Player is out of ammo
            return;
        }

        UpdateAmmoUI();
        currentAmmo--;
        ammoBar.fillAmount = (float)currentAmmo / maxAmmo;

        // play shoot effects
        animator.SetTrigger(shootAnimationName);
        PV.RPC("RPC_PlayShootEffects", RpcTarget.All);

        //shoot ray
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = cam.transform.position;
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log("We hit" + hit.collider.gameObject.name);
            hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)itemInfo).damage);
            PV.RPC("RPC_Shoot", RpcTarget.All, hit.point, hit.normal);
        }
    }

    void Reload()
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

        PV.RPC("RPC_PlayReloadEffects", RpcTarget.All);
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
        ammoText.text = currentAmmo.ToString();

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
}

