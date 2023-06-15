using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRocket : MonoBehaviour

{
    public AudioSource audioSource;
    public AudioClip explodeSound;
    public GameObject capsule;
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
            Destroy(gameObject, 1f);
            audioSource.PlayOneShot(explodeSound);
            capsule.gameObject.SetActive(false);
            GetComponent<BoxCollider>().enabled = false;
        }
        else if (other.gameObject.CompareTag("Demon"))
        {
            Destroy(gameObject);
            // Deal damage to enemy
            Demon enemy = other.gameObject.GetComponent<Demon>();
            GetComponent<BoxCollider>().enabled = false;
            
               enemy.TakeDamageRocket();
               
            

            // Destroy bullet
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Zombie"))
        {
            // Deal damage to enemy
            Zombie enemy = other.gameObject.GetComponent<Zombie>();
            
                enemy.TakeDamageRocket();
                CanvasManager.Instance.UpdateScore(10);
            GetComponent<BoxCollider>().enabled = false;

            // Destroy bullet
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Soldier"))
        {
            // Deal damage to enemy
            Soldier enemy = other.gameObject.GetComponent<Soldier>();
            
            CanvasManager.Instance.UpdateScore(10);
            GetComponent<BoxCollider>().enabled = false;
           enemy.TakeDamageRocket();
            // Destroy bullet
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            // Deal damage to enemy
            CyberTitan enemy = other.gameObject.GetComponent<CyberTitan>();

            
            GetComponent<BoxCollider>().enabled = false;
            enemy.TakeDamageRocket();
            // Destroy bullet
            Destroy(gameObject);
        }
    }
}

