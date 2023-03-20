using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 5f; // The amount of time before the fireball is destroyed

    void Start()
    {
        Destroy(gameObject, lifeTime); // Destroy the fireball after a certain amount of time
    }

void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit!");
        }
    }
}

