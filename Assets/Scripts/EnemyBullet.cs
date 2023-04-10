using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    
    
    private void OnCollisionEnter(Collision other)
    {
         
        if (other.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
            GetComponent<BoxCollider>().enabled = false;
        }
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}
