using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Plasma: MonoBehaviour
{
    

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            // Deal damage to enemy
            EnemyHealth enemy = other.gameObject.GetComponent<EnemyHealth>();
            GetComponent<SphereCollider>().enabled = false;
            enemy.TakeDamage(3);
        }
        else if (other.gameObject.CompareTag("Wall") || (other.gameObject.CompareTag("Floor")))
        {
            // Destroy bullet
            Destroy(gameObject);
        }
    }
}
