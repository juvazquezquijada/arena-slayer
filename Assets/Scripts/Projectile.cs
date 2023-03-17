using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage = 10.0f;

    private void OnCollisionEnter(Collision collision)
    {
        // If the projectile collides with an object that has a health component, apply damage to it
        Health health = collision.collider.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(damage);
        }

        // Destroy the projectile
        Destroy(gameObject);
    }
}

