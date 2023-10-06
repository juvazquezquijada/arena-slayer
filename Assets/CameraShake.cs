using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Transform cameraTransform;
    public Vector3 originalPosition;

    // Parameters for screen shake
    public float shakeDuration = 0.5f;
    public float shakeIntensity = 0.2f;

    private float shakeTimer = 0f;

    private void Awake()
    {
        cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            // Shake the camera
            cameraTransform.localPosition = originalPosition + Random.insideUnitSphere * shakeIntensity;

            // Decrease the timer
            shakeTimer -= Time.deltaTime;
        }
        else
        {
            // Reset the camera position
            shakeTimer = 0f;
            cameraTransform.localPosition = originalPosition;
        }
    }

    public void StartShake()
    {
        originalPosition = cameraTransform.localPosition;
        shakeTimer = shakeDuration;
    }
}
