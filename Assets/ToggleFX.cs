using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Add this line for UI components

public class ToggleFX : MonoBehaviour
{
    public GameObject postFX; // Reference to the post-processing effects object
    public Toggle toggleFX; // Reference to the Toggle UI element
    public AudioSource audioSource;
    public AudioClip menuSound;

    private void Start()
    {
        // Find the post-processing effects object by tag
        postFX = GameObject.FindGameObjectWithTag("PostFX");

        // Get a reference to the Toggle component on this GameObject
        toggleFX = GetComponent<Toggle>();

        // Attach an event listener to the toggle's value change
        toggleFX.onValueChanged.AddListener(OnToggleValueChanged);
    }

    public void OnToggleValueChanged(bool isFXActive)
    {
        // Set the post-processing effects object's active state based on the toggle value
        postFX.SetActive(isFXActive);

        audioSource.PlayOneShot(menuSound);
    }
}
