using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int health;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            //player is dead
        }
      if(Input.GetKeyDown(KeyCode.RightShift))
      {
        DamagePlayer(30);
        Debug.Log("Taken Damage");
      }
    }

    public void DamagePlayer(int damage)
    {
        health -= damage;  
    }
}
