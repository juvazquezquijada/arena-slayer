using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Fireball : MonoBehaviour
{
  
    public float lifeTime = 5f;

   


    void Start()
    {
        Destroy(gameObject, lifeTime);    
    }

    private void OnCollisionEnter(Collision other)
    {       
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Game over!");
        }
    }
}
  
