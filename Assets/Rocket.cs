using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour

{
    public AudioSource  audioSource;
    public AudioClip explodeSound;
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
            Destroy(gameObject);
            audioSource.PlayOneShot(explodeSound);
        }
        else if(other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            GetComponent<CapsuleCollider>().enabled = false;
            audioSource.PlayOneShot(explodeSound);
        }
    }
}
