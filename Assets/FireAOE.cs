using UnityEngine;

public class FireAOE : MonoBehaviour
{
    public int damage = 20;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Apply damage to the player
            PlayerController1 playerController = other.GetComponent<PlayerController1>();
            if (playerController != null)
            {
                playerController.TakeDamage(damage);
            }


            // Trigger visual/audio effects (e.g., particle systems, sounds)
            // You can also deactivate the trigger collider or the entire particle system if needed
        }
    }
}
