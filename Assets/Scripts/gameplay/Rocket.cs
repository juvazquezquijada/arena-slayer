using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour

{
    public AudioSource  audioSource;
    public GameObject explosionPrefab;
    public AudioClip explodeSound;
    public GameObject capsule;

    public AudioSource fireSound;
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
        if (other.gameObject.CompareTag("Wall") || (other.gameObject.CompareTag("Floor")))
        {
            Destroy(gameObject, 1.5f);
            audioSource.PlayOneShot(explodeSound);
            capsule.gameObject.SetActive(false);
            capsule.GetComponent<CapsuleCollider>().enabled = false;
            Instantiate(explosionPrefab, other.contacts[0].point, Quaternion.identity);
            fireSound.Stop();
        }
        if(other.gameObject.CompareTag("Player1"))
        {
            Destroy(gameObject, 1.5f);
            capsule.GetComponent<CapsuleCollider>().enabled = false;
            audioSource.PlayOneShot(explodeSound);
            capsule.gameObject.SetActive (false);
            Instantiate(explosionPrefab, other.contacts[0].point, Quaternion.identity);
            fireSound.Stop();
        }
    }
}
