using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    private bool isPaused;

    void Start()
    {
        // Hide the pause menu initially
        pauseMenuPanel.SetActive(false);
        isPaused = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                // Show pause menu
                Time.timeScale = 0f; // Pause the game
                isPaused = true;
                pauseMenuPanel.SetActive(true);
            }
            else
            {
                // Hide pause menu
                Time.timeScale = 1f; // Unpause the game
                isPaused = false;
                pauseMenuPanel.SetActive(false);
            }
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // Unpause the game
        isPaused = false;
        pauseMenuPanel.SetActive(false);
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("TitleScene");
    }
}

