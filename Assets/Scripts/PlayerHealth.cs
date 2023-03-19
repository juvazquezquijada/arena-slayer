using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100; // Starting health of the player
    public int currentHealth; // Current health of the player

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startingHealth; // Set the player's current health to the starting health
    }

    // Function for taking damage
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount; // Decrease the player's health by the damage amount

        if (currentHealth <= 0) // If the player's health is 0 or less, kill the player
        {
            Die();
        }
    }

    // Function for killing the player
    void Die()
    {
        // Implement the player's death behavior here, such as displaying a game over screen or resetting the level
    }
}


