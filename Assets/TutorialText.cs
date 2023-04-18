using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialText : MonoBehaviour
{
    public float displayTime = 5.0f; // Time in seconds to display the tutorial text
    public TextMeshProUGUI tutorialText; // Reference to the tutorial text UI element
    private bool isShowing = false; // Flag to keep track of whether the tutorial text is currently being displayed

    void Start()
    {
        tutorialText.enabled = false; // Disable the tutorial text initially
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (!isShowing)
            {
                StartCoroutine(DisplayTutorialText());
            }
        }
    }

    IEnumerator DisplayTutorialText()
    {
        isShowing = true;
        tutorialText.enabled = true;
        yield return new WaitForSeconds(displayTime);
        tutorialText.enabled = false;
        isShowing = false;
    }
}
