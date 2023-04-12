using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip shootSound;

    void Start()
    {
         audioSource = GetComponent<AudioSource>();
         audioSource.PlayOneShot(shootSound);
         Destroy(gameObject, 5f);
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
