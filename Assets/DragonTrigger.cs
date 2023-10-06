using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonTrigger : MonoBehaviour
{
	public AncientDragonBoss boss;

	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("PowerfulBullet"))
		{
			boss.TakeDamage(35);
		}
		else if (other.gameObject.CompareTag("Plasma"))
		{
			boss.TakeDamage(3);
		}
	}
}
