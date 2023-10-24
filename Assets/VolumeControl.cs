using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VolumeControl : MonoBehaviour
{
    public Slider volumeSlider;
    public AudioSource audioSource;
    public AudioClip menuSound;
    public float menuSoundCooldown = 0.1f; // Set your desired cooldown time here
    private float lastMenuSoundTime;

    private void Start()
    {
        // Get reference to the slider component
        volumeSlider = GetComponent<Slider>();

        // Set initial slider value to match current master volume
        volumeSlider.value = AudioListener.volume;

        // Add a listener to the slider's value change event
        volumeSlider.onValueChanged.AddListener(ChangeMasterVolume);
    }

    public void ChangeMasterVolume(float volume)
    {
        // Set the master volume based on the slider value
        AudioListener.volume = volume;

        // Check if enough time has passed since the last menu sound
        if (Time.time - lastMenuSoundTime >= menuSoundCooldown)
        {
            audioSource.PlayOneShot(menuSound);
            lastMenuSoundTime = Time.time; // Update the last sound time
        }
    }
}
