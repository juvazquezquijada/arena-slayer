using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BenchPressInteraction : MonoBehaviour
{
    public Animator benchPressAnimator;
    public Animator playerAnimator;
    public PlayerController1 playerController; // Reference to the PlayerController1 script

    [SerializeField] private bool isPlayerNearby = false;
    [SerializeField] private bool isBenchPressing = false;
    [Header("Interaction Settings")]
    public Transform interactionTransform; // Public Transform variable to set in the Inspector


    [Header("UI Settings")]
    public Image staminaBarImage; // Reference to the stamina UI bar image
    public GameObject textPrompt;

    // Adjust this value to determine how much stamina is drained per second
    public float staminaDrainRate = 1f;

    void Start()
    {
        textPrompt.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            Debug.Log("Player is nearby. Press 'E' to interact.");
            textPrompt.gameObject.SetActive(true);
            // Display a prompt to the player to interact
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            Debug.Log("Player is no longer nearby.");
            // Hide the interaction prompt
            textPrompt.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (isPlayerNearby && !isBenchPressing && Input.GetKeyDown(KeyCode.E))
        {
            textPrompt.gameObject.SetActive(false);
            Debug.Log("Interacting with bench press.");
            // Store the player's original position and rotation
            Vector3 originalPosition = playerController.transform.position;
            Quaternion originalRotation = playerController.transform.rotation;

            // Set the player's position and rotation to match the interaction transform
            playerController.transform.position = interactionTransform.position;
            playerController.transform.rotation = interactionTransform.rotation;

            // Calculate the rotation needed for the arm pivot
            Quaternion armPivotRotation = Quaternion.LookRotation(interactionTransform.forward, Vector3.up);

            // Set the arm pivot's rotation
            playerController.SetArmPivotRotation(armPivotRotation);

            // Play the bench press animation
            benchPressAnimator.SetTrigger("Press");
            // Play the player animation
            playerAnimator.SetTrigger("BenchPress");

            // Set the player's isBenchPressing flag to true
            playerController.isBenchPressing = true;
            //is bench press active?
            isBenchPressing = true;
            // Start a coroutine to drain stamina and re-enable camera control after the animation duration
            StartCoroutine(DrainStaminaAndEnableCameraControl(originalPosition, originalRotation));
        }
    }

    private IEnumerator DrainStaminaAndEnableCameraControl(Vector3 originalPosition, Quaternion originalRotation)
    {
        float interactionDuration = 16f; // Duration of the bench press interaction animation
        float delayBeforeDrain = 5f; // Delay before stamina starts draining

        // Wait for the initial delay
        yield return new WaitForSeconds(delayBeforeDrain);

        float startTime = Time.time;

        // Start draining stamina over time while the interaction is ongoing
        while (Time.time - startTime < interactionDuration)
        {
            float staminaDrainAmount = staminaDrainRate * Time.deltaTime;
            playerController.currentStamina -= staminaDrainAmount;

            // Update the stamina UI bar
            staminaBarImage.fillAmount = playerController.currentStamina / playerController.maxStamina;

            yield return null;
        }

        Debug.Log("Bench press interaction finished.");
        // Reset the player's position and rotation to their original values
        playerController.transform.position = originalPosition;
        playerController.transform.rotation = originalRotation;

        // bench press is not active
        isBenchPressing = false;

        // Reset the isBenchPressing flag
        playerController.isBenchPressing = false;

        // Reset the player's stamina to 0
        playerController.currentStamina = 0f;

        // Reset the stamina UI bar
        staminaBarImage.fillAmount = 0f; // Or set it to the desired starting value
    }

}

