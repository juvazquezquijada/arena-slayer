using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 2f);
    }
    
    private void OnCollisionEnter(Collision other)
    {
         
        if (other.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
            
        }
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
