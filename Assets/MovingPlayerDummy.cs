using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MovingPlayerDummy : MonoBehaviour
{
    public Transform movementPath;  // Reference to the parent of the path points
    public float moveSpeed = 2.0f;  // Speed of movement
    public Transform player;


    private NavMeshAgent navMeshAgent; // Reference to the NavMeshAgent component

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>(); // Get a reference to the NavMeshAgent component
        navMeshAgent.stoppingDistance = 2f; // Set the stopping distance for the agent
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        WalkTowardsPlayer();
    }

    private void WalkTowardsPlayer()
    {
        navMeshAgent.SetDestination(player.position);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Bullet") || collision.collider.CompareTag("Plasma"))
        {
            HandleProjectileCollision();
        }
    }

    private void HandleProjectileCollision()
    {
        gameObject.SetActive(false);
        Invoke("EnableDummy", 2f); // Re-enable after 2 seconds (adjust as needed)
    }

    private void EnableDummy()
    {
        gameObject.SetActive(true);
    }

  
}
