using UnityEngine;
using UnityEngine.UI;

public class SensitivitySlider : MonoBehaviour
{
    public Slider sensitivitySlider;
    public float minSensitivity = 1f;
    public float maxSensitivity = 10f;

    private void Start()
    {
        sensitivitySlider.value = LoadSensitivity();
        sensitivitySlider.onValueChanged.AddListener(UpdateSensitivity);
    }

    public void UpdateSensitivity(float sensitivity)
    {
        sensitivity = Mathf.Lerp(minSensitivity, maxSensitivity, sensitivity);
        PlayerPrefs.SetFloat("Sensitivity", sensitivity);
    }

    private float LoadSensitivity()
    {
        return PlayerPrefs.GetFloat("Sensitivity", (minSensitivity + maxSensitivity) * 0.5f);
    }
}
