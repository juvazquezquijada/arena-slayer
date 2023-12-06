using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth: MonoBehaviour
{
    public int health;
    private AudioSource audioSource;
    public AudioClip hurtSound;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public void TakeDamage(int damage)
    {
        health -= damage;
        audioSource.PlayOneShot(hurtSound);
    }
}
