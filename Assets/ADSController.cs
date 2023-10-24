using UnityEngine;

public class ADSController : MonoBehaviour
{
    public Animator cameraAnimator;
    public Animator playerAnimator;
    private bool isADS = false;
    public SwayNBobScript swayBob;

    private void Update()
    {
        // Check if the right mouse button is held down
        if (Input.GetMouseButton(1))
        {
            if (!isADS)
            {
                // Enable ADS animation
                isADS = true;
                cameraAnimator.SetBool("IsADS", true);
                playerAnimator.SetBool("IsADS", true);
                swayBob.ResetBobEulerRotation();
            }
        }
        else if (Input.GetMouseButtonUp(1))
        {
            if (isADS)
            {
                // Disable ADS animation
                isADS = false;
                cameraAnimator.SetBool("IsADS", false);
                playerAnimator.SetBool("IsADS", false);
            }
        }

        // Check for weapon switch input (e.g., Q, E, mouse wheel, number keys 1 to 7)
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E) || Input.GetAxis("Mouse ScrollWheel") != 0 ||
            (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3) ||
             Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Alpha6) ||
             Input.GetKeyDown(KeyCode.Alpha7)))
        {
            if (isADS)
            {
                // Disable ADS animation when the player switches weapons
                isADS = false;
                cameraAnimator.SetBool("IsADS", false);
                playerAnimator.SetBool("IsADS", false);
            }
        }

    }
}
