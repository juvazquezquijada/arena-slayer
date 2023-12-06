using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    public int damage = 6;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            // Deal damage to enemy
            EnemyHealth enemy = other.gameObject.GetComponent<EnemyHealth>();
            GetComponent<SphereCollider>().enabled = false;
            enemy.TakeDamage(damage);
        }

        else if (other.gameObject.CompareTag("Wall") || (other.gameObject.CompareTag("Floor")))
        {
            // Destroy bullet
            Destroy(gameObject);
        }
    }
}
