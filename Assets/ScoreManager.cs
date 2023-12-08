using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text highScoreText;

    private float highScore;

    private static ScoreManager instance;

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;

        // Load the high score when the game starts
        LoadHighScore();

        // Check if the current scene is the title screen and update the high score text
        if (SceneManager.GetActiveScene().name == "TitleScene")
        {
            UpdateHighScoreText();
        }
    }

    void Update()
    {
        // Check if the current scene is the title screen and update the high score text
        if (SceneManager.GetActiveScene().name == "TitleScene")
        {
            UpdateHighScoreText();
        }
    }

    public void SetHighScore(float currentScore)
    {
        if (currentScore > highScore)
        {
            highScore = currentScore;

            // Save the high score when it's updated
            SaveHighScore();

            // Update the high score text only if the current scene is the title screen
            if (SceneManager.GetActiveScene().name == "TitleScene")
            {
                UpdateHighScoreText();
            }
        }
    }

     public void ResetHighScore()
    {
        // Reset the high score to 0
        highScore = 0;

        // Save the updated high score using PlayerPrefs
        SaveHighScore();

        // Update the high score text only if the current scene is the title screen
        if (SceneManager.GetActiveScene().name == "TitleScene")
        {
            UpdateHighScoreText();
        }
    }

    void UpdateHighScoreText()
    {
        highScoreText = GameObject.FindGameObjectWithTag("HighScoreText").GetComponent<TextMeshProUGUI>();
        highScoreText.text = "Current Best: " + highScore.ToString();
    }

    void SaveHighScore()
    {
        PlayerPrefs.SetFloat("HighScore", highScore);
        PlayerPrefs.Save();
    }

    void LoadHighScore()
    {
        highScore = PlayerPrefs.GetFloat("HighScore", 0f);
    }
}
