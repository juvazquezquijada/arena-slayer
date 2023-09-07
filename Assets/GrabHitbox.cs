using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GrabHitbox : MonoBehaviour
{
    public CursedCaptainBoss boss;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !boss.isDead)
        {
            // Move the player to the position of the captain's hand
            boss.MovePlayerToCaptainHand();

            // Trigger the grab attack logic
            boss.PerformGrab();
        }
    }
}
