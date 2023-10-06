using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    public int damage = 6;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Demon"))
        {
            Destroy(gameObject);
            // Deal damage to enemy
            Demon enemy = other.gameObject.GetComponent<Demon>();
            GetComponent<SphereCollider>().enabled = false;

            enemy.TakeDamage(damage);



            // Destroy bullet
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Zombie"))
        {
            // Deal damage to enemy
            Zombie enemy = other.gameObject.GetComponent<Zombie>();

            enemy.TakeDamage(damage);
            //CanvasManager.Instance.UpdateScore(10);
            GetComponent<SphereCollider>().enabled = false;

            // Destroy bullet
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Soldier"))
        {
            // Deal damage to enemy
            Soldier enemy = other.gameObject.GetComponent<Soldier>();

            //CanvasManager.Instance.UpdateScore(10);
            GetComponent<SphereCollider>().enabled = false;
            enemy.TakeDamage(damage);
            // Destroy bullet
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            // Deal damage to enemy
            CyberTitan enemy = other.gameObject.GetComponent<CyberTitan>();


            GetComponent<SphereCollider>().enabled = false;
            enemy.TakeDamage(damage);
            // Destroy bullet
            Destroy(gameObject);
        }

        else if (other.gameObject.CompareTag("BuffDemon"))
        {
            // Deal damage to enemy
            BuffDemonAI enemy = other.gameObject.GetComponent<BuffDemonAI>();
            GetComponent<SphereCollider>().enabled = false;
            enemy.TakeDamage(damage);
            // Destroy bullet
            Destroy(gameObject);
        }

        else if (other.gameObject.CompareTag("Robo Demon"))
        {
            // Deal damage to enemy
            RoboDemonAI enemy = other.gameObject.GetComponent<RoboDemonAI>();


            GetComponent<SphereCollider>().enabled = false;
            enemy.TakeDamage(damage);
            // Destroy bullet
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("CursedCaptain"))
        {
            // Deal damage to enemy
            CursedCaptainBoss enemy = other.gameObject.GetComponent<CursedCaptainBoss>();


            GetComponent<SphereCollider>().enabled = false;
            enemy.TakeDamage(damage);
            // Destroy bullet
            Destroy(gameObject);
        }

        else if (other.gameObject.CompareTag("Dragon"))
        {
            // Deal damage to enemy
            AncientDragonBoss enemy = other.gameObject.GetComponentInParent<AncientDragonBoss>();


            GetComponent<SphereCollider>().enabled = false;
            enemy.TakeDamage(damage);
            // Destroy bullet
            Destroy(gameObject);
        }

        else if (other.gameObject.CompareTag("Wall") || (other.gameObject.CompareTag("Floor")))
        {
            // Destroy bullet
            Destroy(gameObject);
        }
    }
}
