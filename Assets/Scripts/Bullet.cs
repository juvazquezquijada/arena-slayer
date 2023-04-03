using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision other)
        {
            if(other.gameObject.CompareTag("Enemy"))
            {
                PlayerController player = other.gameObject.GetComponent<PlayerController>();
                if (player !=null)
                {
                    player.UpdateScore(1);
                }
            }
        }
    }