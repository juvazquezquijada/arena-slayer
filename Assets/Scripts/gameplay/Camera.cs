using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public float sensitivity = 100.0f;
    public float clampAngle = 80.0f;

    public Transform firstPersonTransform;
    public Transform thirdPersonTransform;
    private Transform currentTransform;

    private float rotY = 0.0f;
    private float rotX = 0.0f;

    void Start()
    {
        Application.targetFrameRate = 60;
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentTransform = firstPersonTransform;

        if (CanvasManager.Instance.gameActive) 
        {
            Cursor.visible = true;
        } 
        else 
        {
            Cursor.visible = false;
        }
    }

    void FixedUpdate()
{
    float mouseX = Input.GetAxis("Mouse X");
    float mouseY = -Input.GetAxis("Mouse Y");

    if (Input.GetKeyDown(KeyCode.V))
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

    rotY += mouseX * sensitivity * Time.fixedDeltaTime;
    rotX += mouseY * sensitivity * Time.fixedDeltaTime;

    rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

    Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
    transform.rotation = localRotation;

    transform.position = currentTransform.position;

    // Set the player's rotation based on the camera's rotation
    transform.parent.rotation = Quaternion.Euler(0.0f, rotY, 0.0f);
}

}
