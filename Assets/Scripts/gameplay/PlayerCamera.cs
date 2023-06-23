using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float sensitivity = 100.0f;
    public float clampAngle = 80.0f;
    public float smoothness = 0.1f;

    public Transform firstPersonTransform;
    public Transform thirdPersonTransform;
    private Transform currentTransform;

    private float rotY = 0.0f;
    private float rotX = 0.0f;

    private Quaternion desiredRotation;

    void Start()
    {
        Application.targetFrameRate = 60;
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
        currentTransform = firstPersonTransform;
        desiredRotation = transform.rotation;
    }

    void Update()
    {
        if (CanvasManager.Instance.gameActive)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

       
    }

    void FixedUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float joystickX = Input.GetAxis("JoystickAxis4");
        

        if (Input.GetKeyDown(KeyCode.V))
        {
            SwitchPOV();
        }
        else if (Input.GetKeyDown(KeyCode.Joystick1Button9))
        {
            SwitchPOV();
        }
        

        rotY += mouseX * sensitivity * Time.fixedDeltaTime;
        rotY += joystickX * sensitivity * Time.fixedDeltaTime;

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);


        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        desiredRotation = localRotation;

        transform.position = currentTransform.position;

        // Set the player's rotation based on the camera's rotation
        transform.parent.rotation = Quaternion.Euler(0.0f, rotY, 0.0f);

        // Smoothly interpolate between the current rotation and the desired rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, smoothness);
    }

    public void SwitchPOV()
    {
        if (currentTransform == firstPersonTransform)
            {
                currentTransform = thirdPersonTransform;
            }
            else
            {
                currentTransform = firstPersonTransform;
            }
    }
}