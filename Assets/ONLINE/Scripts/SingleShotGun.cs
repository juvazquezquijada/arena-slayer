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
	
	private float lastFireTime; // Time when the gun was last firedd

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
			
		}
	}

	[PunRPC]
	void RPC_Shoot()
	{
		
	}
}