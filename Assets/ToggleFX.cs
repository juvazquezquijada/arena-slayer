using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class ToggleFX : MonoBehaviour
{
    [SerializeField] private GameObject postFX; // Reference to the post-processing effects object
    public Toggle toggleFX; // Reference to the Toggle UI element
    public AudioSource audioSource;
    public AudioClip menuSound;
    private PostProcessVolume postProcessVolume;

    private void Start()
    {
        // Find the post-processing effects object by tag
        postFX = GameObject.FindGameObjectWithTag("PostFX");

        // Get a reference to the Toggle component on this GameObject
        toggleFX = GetComponent<Toggle>();

        // Get the PostProcessVolume component from the postFX GameObject
        postProcessVolume = postFX.GetComponent<PostProcessVolume>();

        // Attach an event listener to the toggle's value change
        toggleFX.onValueChanged.AddListener(OnToggleValueChanged);
    }

    public void OnToggleValueChanged(bool isFXActive)
    {
        // Enable or disable the Post Process Volume component based on the toggle value
        postProcessVolume.enabled = isFXActive;

        audioSource.PlayOneShot(menuSound);
    }
}
