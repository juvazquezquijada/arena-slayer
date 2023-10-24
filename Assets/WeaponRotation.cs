using UnityEngine;

public class WeaponRotation : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 5f;

    private void Update()
    {
        // Get mouse input for the Y-axis rotation (Mouse Y)
        float rotationInput = Input.GetAxis("Mouse Y");

        // Create a rotation quaternion around the Y-axis
        Quaternion rotation = Quaternion.Euler(rotationInput * rotationSpeed, 0, 0);

        // Apply the rotation to the weapon holder
        transform.localRotation *= rotation;
    }
}
