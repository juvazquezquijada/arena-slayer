using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRocket : MonoBehaviour

{
    public AudioSource audioSource;
    public AudioClip explodeSound;
    public GameObject capsule;
    public float destroyTime = 3f;
    
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject,10f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject, destroyTime);
            audioSource.PlayOneShot(explodeSound);
            capsule.gameObject.SetActive(false);
            GetComponent<BoxCollider>().enabled = false;
        }
        else if (other.gameObject.CompareTag("Demon"))
        {
            
            // Deal damage to enemy
            Demon enemy = other.gameObject.GetComponent<Demon>();
            GetComponent<BoxCollider>().enabled = false;
            capsule.gameObject.SetActive(false);
            audioSource.PlayOneShot(explodeSound);
               enemy.TakeDamageRocket();
               
            

            // Destroy bullet
            Destroy(gameObject, destroyTime);
        }
        else if (other.gameObject.CompareTag("Zombie"))
        {
            // Deal damage to enemy
            Zombie enemy = other.gameObject.GetComponent<Zombie>();
            audioSource.PlayOneShot(explodeSound);
                enemy.TakeDamageRocket();
                CanvasManager.Instance.UpdateScore(10);
            GetComponent<BoxCollider>().enabled = false;
            capsule.gameObject.SetActive(false);

            // Destroy bullet
            Destroy(gameObject, destroyTime);
        }
        else if (other.gameObject.CompareTag("Soldier"))
        {
            // Deal damage to enemy
            Soldier enemy = other.gameObject.GetComponent<Soldier>();
            audioSource.PlayOneShot(explodeSound);
            CanvasManager.Instance.UpdateScore(10);
            GetComponent<BoxCollider>().enabled = false;
            capsule.gameObject.SetActive(false);
           enemy.TakeDamageRocket();
            // Destroy bullet
            Destroy(gameObject, destroyTime);
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            // Deal damage to enemy
            CyberTitan enemy = other.gameObject.GetComponent<CyberTitan>();
            audioSource.PlayOneShot(explodeSound);
           capsule.gameObject.SetActive(false);
            GetComponent<BoxCollider>().enabled = false;
            enemy.TakeDamageRocket();
            // Destroy bullet
            Destroy(gameObject, destroyTime);
            
        }
    }
}

