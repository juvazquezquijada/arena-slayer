using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRocket : MonoBehaviour

{
    public AudioSource audioSource;
    public AudioClip explodeSound;
    public GameObject capsule;
    public float destroyTime = 3f;
    public GameObject explosionPrefab;
    public float splashDamageRadius = 5f;
    
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
              Instantiate(explosionPrefab, other.contacts[0].point, Quaternion.identity);
              BlastRadius();
        }
        
        else if (other.gameObject.CompareTag("Enemy"))
        {
            Instantiate(explosionPrefab, other.contacts[0].point, Quaternion.identity);
            // Deal damage to enemy
            EnemyHealth enemy = other.gameObject.GetComponent<EnemyHealth>();
            audioSource.PlayOneShot(explodeSound);
           capsule.gameObject.SetActive(false);
            GetComponent<BoxCollider>().enabled = false;
            enemy.TakeDamage(20);
            // Destroy bullet
            Destroy(gameObject, destroyTime);     
        }
    }

    public void BlastRadius()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, splashDamageRadius);
         foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Demon"))
            {
                // Deal damage to enemy
            EnemyHealth enemy = collider.GetComponent<EnemyHealth>();
            enemy.TakeDamage(20);
                PlayerController1.Instance.UpdateScore(4);
                // Destroy bullet
                Destroy(gameObject, destroyTime);
            }
        }
    }
}


