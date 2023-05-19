using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public int health = 150;
    public AudioClip destroySound;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0f)
        {
            GetComponent<BoxCollider>().enabled = false;
            Destroy(gameObject, 3f);
            audioSource.PlayOneShot(destroySound);
        }
    }
}
