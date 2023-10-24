using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBobbing : MonoBehaviour
{
    public Transform cameraTransform;
    public float bobbingSpeed = 5.0f;
    public float bobbingAmount = 0.05f;
    public float cameraYPosition = 0.5f;

    private float timer = 0.0f;

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f)
        {
            // Calculate bobbing motion
            float waveSlice = Mathf.Sin(timer);
            float translateChange = 0f;
            if (waveSlice != 0)
            {
                translateChange = waveSlice * bobbingAmount;
                timer += bobbingSpeed * Time.deltaTime;
            }
            else
            {
                timer = 0f;
            }

            // Apply bobbing motion to the camera's local position
            Vector3 localPos = cameraTransform.localPosition;
            localPos.y = cameraYPosition + translateChange; // Set Y position to cameraYPosition + bobbing effect
            cameraTransform.localPosition = localPos;
        }
        else
        {
            // Reset bobbing motion when not moving
            timer = 0f;
            Vector3 localPos = cameraTransform.localPosition;
            localPos.y = cameraYPosition; // Reset Y position to cameraYPosition
            cameraTransform.localPosition = localPos;
        }
    }
}
