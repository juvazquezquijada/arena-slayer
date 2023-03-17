using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DemonAI : MonoBehaviour {
    
    public Transform target;    // the player's transform
    public float moveSpeed = 3; // the speed at which the Demon moves
    public float attackRange = 2;   // the range at which the Demon can attack the player
    public GameObject fireballPrefab; // the prefab of the fireball the demon throws
    public float fireballSpeed = 5; // the speed at which the fireball moves
    public float fireballCooldown = 2; // the time in seconds between fireball throws
    private float lastFireballTime = -1000; // the time at which the demon last threw a fireball
    
    void Start () {
        target = GameObject.FindGameObjectWithTag("Player").transform;  // find the player's transform
    }
    
    void Update () {
        // calculate the distance between the Demon and the player
        float distance = Vector3.Distance(transform.position, target.position);
        
        // if the player is within range, attack
        if (distance <= attackRange) {
            if (distance <= 1) {
                MeleeAttack();
            }
            else {
                RangedAttack();
            }
        }
        // otherwise, move towards the player
        else {
            Move();
        }
    }
    
    void Move () {
        // rotate towards the player
        transform.LookAt(target);
        
        // move towards the player
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }
    
    void RangedAttack () {
        // throw a fireball if the cooldown has expired
        if (Time.time - lastFireballTime >= fireballCooldown) {
            ThrowFireball();
            lastFireballTime = Time.time;
        }
        
        // move towards the player even within attack range
        Move();
    }
    
    void MeleeAttack () {
        // perform the Demon's melee attack behavior
        Debug.Log("Demon performs melee attack!");
        
        // stop throwing fireballs
        lastFireballTime = -1000;
    }
    
    void ThrowFireball () {
        // instantiate the fireball prefab
        GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity) as GameObject;
        
        // set the fireball's velocity towards the player
        Vector3 direction = (target.position - transform.position).normalized;
        fireball.GetComponent<Rigidbody>().velocity = direction * fireballSpeed;
    }
}
