using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public Slider volumeSlider;

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
    }
}
