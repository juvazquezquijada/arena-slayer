using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float lifeTime = 5f;
    public int damage = 10;

    void Start()
    {
        Destroy(gameObject, lifeTime);    
    }

   private void OnCollisionEnter(Collision other)
    {       
    if (other.gameObject.CompareTag("Player"))
    {
        Debug.Log("Player hit by fireball!");
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            player.TakeDamage(10);
        }
    }
}

}
