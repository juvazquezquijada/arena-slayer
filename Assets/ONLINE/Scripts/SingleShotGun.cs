using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShotGun : Gun
{
	[SerializeField] Camera cam;
	[SerializeField] Animator animator;
	[SerializeField] string shootAnimationName;
	[SerializeField] float fireRate;
	[SerializeField] AudioClip shootSound;
	[SerializeField] AudioSource gunSound;
	PhotonView PV;
	
	private float lastFireTime; // Time when the gun was last firedd


	void Awake()
	{
		PV = GetComponent<PhotonView>();
	}

	public override void Use()
	{
		if (Time.time - lastFireTime >= fireRate) // Check if enough time has passed since the last shot
		{
			lastFireTime = Time.time; // Update the last fire time

			Shoot(); // Perform the shooting logic
		}
	}

	void Shoot()
	{
		animator.SetTrigger(shootAnimationName);
		gunSound.PlayOneShot(shootSound);
		Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
		ray.origin = cam.transform.position;
		if (Physics.Raycast(ray, out RaycastHit hit))
		{
			Debug.Log("We hit" + hit.collider.gameObject.name);
			hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)itemInfo).damage);
			PV.RPC("RPC_Shoot", RpcTarget.All, hit.point, hit.normal);
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
}