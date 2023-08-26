using System.Collections;
using UnityEngine;

public class MovingPlayerDummy : MonoBehaviour
{
    public Transform movementPath;  // Reference to the parent of the path points
    public float moveSpeed = 2.0f;  // Speed of movement

    public Transform[] pathPoints;
    private int currentPointIndex = 0;
    private bool movingForward = true;

    private void Start()
    {
        pathPoints = new Transform[movementPath.childCount];
        for (int i = 0; i < movementPath.childCount; i++)
        {
            pathPoints[i] = movementPath.GetChild(i);
        }
    }

    private void Update()
    {
        MoveAlongPath();
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

    private void MoveAlongPath()
    {
        if (pathPoints.Length == 0)
        {
            return;
        }

        Vector3 targetPosition = pathPoints[currentPointIndex].position;
        float step = moveSpeed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            if (movingForward)
            {
                currentPointIndex++;
                if (currentPointIndex >= pathPoints.Length)
                {
                    movingForward = false;
                    currentPointIndex = pathPoints.Length - 2;
                }
            }
            else
            {
                currentPointIndex--;
                if (currentPointIndex < 0)
                {
                    movingForward = true;
                    currentPointIndex = 1;
                }
            }
        }
    }
}
