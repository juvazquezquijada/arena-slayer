using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int damage = 10;
    
    private void OnCollisionEnter(Collision other)
    {
         if (other.gameObject.CompareTag("Player"))
        {
            // Deal damage to player
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            player.TakeDamage(damage);

            // Destroy bullet
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
