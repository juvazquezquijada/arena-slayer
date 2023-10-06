using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSound : MonoBehaviour
{
	public AudioSource audioSource;
	public AudioClip stomp;

	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Floor"))
		{
			audioSource.PlayOneShot(stomp);
		}
	}
}
