using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Plasma: MonoBehaviour
{
    

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Demon"))
        {
            Destroy(gameObject);
            // Deal damage to enemy
            Demon enemy = other.gameObject.GetComponent<Demon>();
            GetComponent<SphereCollider>().enabled = false;
            
               enemy.TakeDamage(3);
               
            

            // Destroy bullet
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Zombie"))
        {
            // Deal damage to enemy
            Zombie enemy = other.gameObject.GetComponent<Zombie>();
            
                enemy.TakeDamage(3);

                
            GetComponent<SphereCollider>().enabled = false;

            // Destroy bullet
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Soldier"))
        {
            // Deal damage to enemy
            Soldier enemy = other.gameObject.GetComponent<Soldier>();
            
            enemy.TakeDamage(4);
            GetComponent<SphereCollider>().enabled = false;

            // Destroy bullet
            Destroy(gameObject);
        }

        else if (other.gameObject.CompareTag("Enemy"))
        {
            // Deal damage to enemy
            CyberTitan enemy = other.gameObject.GetComponent<CyberTitan>();
            
            enemy.TakeDamage(5);
            GetComponent<SphereCollider>().enabled = false;

            // Destroy bullet
            Destroy(gameObject);
        }

        else if (other.gameObject.CompareTag("BuffDemon"))
        {
            // Deal damage to enemy
            BuffDemonAI enemy = other.gameObject.GetComponent<BuffDemonAI>();


            GetComponent<SphereCollider>().enabled = false;
            enemy.TakeDamage(6);
            // Destroy bullet
            Destroy(gameObject);
        }

        else if (other.gameObject.CompareTag("CursedCaptain"))
        {
            // Deal damage to enemy
            CursedCaptainBoss enemy = other.gameObject.GetComponent<CursedCaptainBoss>();


            GetComponent<SphereCollider>().enabled = false;
            enemy.TakeDamage(6);
            // Destroy bullet
            Destroy(gameObject);
        }

        else if (other.gameObject.CompareTag("Wall"))
        {
            // Destroy bullet
            Destroy(gameObject);
        }
    }
}
