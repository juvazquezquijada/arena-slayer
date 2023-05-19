using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour

{
    public AudioSource  audioSource;
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
            GetComponent<CapsuleCollider>().enabled = false;
        }
        else if(other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject, 1f);
            GetComponent<CapsuleCollider>().enabled = false;
            audioSource.PlayOneShot(explodeSound);
            capsule.gameObject.SetActive (false);
        }
    }
}
