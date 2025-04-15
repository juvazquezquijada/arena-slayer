using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth: MonoBehaviour
{
    public int health;
    public float damageChain;
    public int staggerValue;
    private AudioSource audioSource;
    public AudioClip hurtSound;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(damageChain < 0)
        {
            damageChain = 0;
        }
        else
        {
            damageChain -= 5 * Time.deltaTime;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        damageChain += damage;
        audioSource.PlayOneShot(hurtSound);
    }
}
