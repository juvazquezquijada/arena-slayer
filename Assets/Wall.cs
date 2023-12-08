using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
         
        if (other.gameObject.CompareTag("EnemyProjectile") || other.gameObject.CompareTag("Fireball"))
        {
            Destroy(other.gameObject);
            
        }
    }
}
