using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour

{
    public AudioSource  audioSource;
    public GameObject explosionPrefab;
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
            Destroy(gameObject, 1.5f);
            audioSource.PlayOneShot(explodeSound);
            capsule.gameObject.SetActive(false);
            GetComponent<BoxCollider>().enabled = false;
            Instantiate(explosionPrefab, other.contacts[0].point, Quaternion.identity);
        }
        else if(other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject, 1.5f);
            GetComponent<BoxCollider>().enabled = false;
            audioSource.PlayOneShot(explodeSound);
            capsule.gameObject.SetActive (false);
            Instantiate(explosionPrefab, other.contacts[0].point, Quaternion.identity);
        }
    }
}
