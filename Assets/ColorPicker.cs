using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using System.IO;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using TMPro;

public class ColorPicker : MonoBehaviour
{
    public Slider redSlider;
    public Slider greenSlider;
    public Slider blueSlider;
    public Image colorDisplay;
    public Renderer targetRenderer;  // The material renderer to apply the color
    public Material targetMaterial; // The material that will be modified

    private void Start()
    {
        // Load the saved color from PlayerPrefs (if it exists)
        LoadColor();

        // Get the material from the target renderer (for applying color)
        targetMaterial = targetRenderer.material;

        // Add listeners to sliders to update color and save when changed
        redSlider.onValueChanged.AddListener((value) => { UpdateColor(); SaveColor(); ApplyColorToMaterial(); });
        greenSlider.onValueChanged.AddListener((value) => { UpdateColor(); SaveColor(); ApplyColorToMaterial(); });
        blueSlider.onValueChanged.AddListener((value) => { UpdateColor(); SaveColor(); ApplyColorToMaterial(); });
    }

    // Update the color display based on slider values
    private void UpdateColor()
    {
        // Get the values from the sliders
        float red = redSlider.value;
        float green = greenSlider.value;
        float blue = blueSlider.value;

        // Create a new color based on the slider values (without alpha)
        Color newColor = new Color(red, green, blue);

        // Update the color display
        colorDisplay.color = newColor;
    }

    // Save the color (RGB) to PlayerPrefs
    private void SaveColor()
    {
        // Get the current color from the color display
        Color currentColor = colorDisplay.color;

        // Convert the color values to a string format, e.g., "R,G,B"
        string colorString = $"{currentColor.r},{currentColor.g},{currentColor.b}";

        // Save the color string in PlayerPrefs with the key "visorcolor"
        PlayerPrefs.SetString("visorcolor", colorString);

        // Save PlayerPrefs to make sure it persists
        PlayerPrefs.Save();
    }

    // Load the color (RGB) from PlayerPrefs
    private void LoadColor()
    {
        // Check if the color exists in PlayerPrefs
        if (PlayerPrefs.HasKey("visorcolor"))
        {
            // Get the color string from PlayerPrefs
            string colorString = PlayerPrefs.GetString("visorcolor");

            // Split the string into individual color components
            string[] colorValues = colorString.Split(',');

            // Convert the components to float and assign to the color
            if (colorValues.Length == 3)
            {
                float r = float.Parse(colorValues[0]);
                float g = float.Parse(colorValues[1]);
                float b = float.Parse(colorValues[2]);

                // Set the color of the sliders and the display (without alpha)
                redSlider.value = r;
                greenSlider.value = g;
                blueSlider.value = b;

                // Update the color display
                UpdateColor();

                // Apply the color to the material
                ApplyColorToMaterial();
            }
        }
    }

    // Apply the current color to the target material
    private void ApplyColorToMaterial()
    {
        // Get the current color from the color display
        Color currentColor = colorDisplay.color;

        // Apply the color to the material (e.g., change Albedo color)
        targetMaterial.color = currentColor;

         // Enable emission if it's not already enabled
        targetMaterial.EnableKeyword("_EMISSION");

        // Apply the color to the material's emission color
        targetMaterial.SetColor("_EmissionColor", currentColor);
    }
}
