using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GrabHitbox : MonoBehaviour
{
    public CursedCaptainBoss boss;
    public Collider grabHitbox;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Move the player to the position of the captain's hand
            boss.MovePlayerToCaptainHand();
            grabHitbox.enabled = false;
            // Trigger the grab attack logic
            boss.PerformGrab();
        }
    }
}
