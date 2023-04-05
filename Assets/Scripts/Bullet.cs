using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    public int damage = 1;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Demon"))
        {
            // Deal damage to enemy
            Demon enemy = other.gameObject.GetComponent<Demon>();
            enemy.health -= damage;

            // Check if enemy is dead
            if (enemy.health <= 0)
            {
                enemy.Die();
                CanvasManager.Instance.UpdateScore(1);
            }

            // Destroy bullet
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Zombie"))
        {
            // Deal damage to enemy
            Zombie enemy = other.gameObject.GetComponent<Zombie>();
            enemy.health -= damage;

            // Check if enemy is dead
            if (enemy.health <= 0)
            {
                enemy.Die();
                CanvasManager.Instance.UpdateScore(1);
            }

            // Destroy bullet
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Soldier"))
        {
            // Deal damage to enemy
            Soldier enemy = other.gameObject.GetComponent<Soldier>();
            enemy.health -= damage;

            // Check if enemy is dead
            if (enemy.health <= 0)
            {
                enemy.Die();
                CanvasManager.Instance.UpdateScore(1);
            }

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
