using UnityEngine;
using TMPro;

public class FPSCounter: MonoBehaviour
{
    public TextMeshProUGUI fpsText;
    private float updateInterval = 0.5f; // Update the FPS counter every half second
    private float accum = 0.0f;
    private int frames = 0;
    private float timeLeft;
    private bool isFPSVisible = true; // Start with FPS counter visible
                                      // Create a static reference to the singleton instance
    private static FPSCounter instance;
    private void Start()
    {
        timeLeft = updateInterval;
        DontDestroyOnLoad(gameObject); // Prevent this GameObject from being destroyed on scene change

        SetFPSVisibility(isFPSVisible);

        // Ensure only one instance exists
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }

    private void Update()
    {
        timeLeft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        frames++;

        if (timeLeft <= 0.0)
        {
            float fps = accum / frames;
            string fpsTextValue = Mathf.Round(fps) + "FPS";
            fpsText.text = fpsTextValue;

            timeLeft = updateInterval;
            accum = 0.0f;
            frames = 0;
        }

        if (Input.GetKeyDown(KeyCode.F12))
        {
            isFPSVisible = !isFPSVisible; // Toggle visibility
            SetFPSVisibility(isFPSVisible);
        }
    }

    private void SetFPSVisibility(bool isVisible)
    {
        fpsText.gameObject.SetActive(isVisible);
    }
}
