using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    
    

    void Start()
    {
         
    }

   private void OnCollisionEnter(Collision other)
   {
       if (other.gameObject.CompareTag("Wall"))
       {
            Destroy(gameObject);
       }
       else if(other.gameObject.CompareTag("Player"))
       {
           Destroy(gameObject);
            GetComponent<BoxCollider>().enabled = false;
       }
   }
    
    


}
