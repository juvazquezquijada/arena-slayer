using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
public class TimerScript : MonoBehaviour
{
    public TMP_Text timerText;
    private float timer;
    private bool playerDied = false;

    void Update()
    {
        if (!playerDied)
        {
            timer += Time.deltaTime;
            UpdateTimerUI();
        }
    }

    void UpdateTimerUI()
    {
        string minutes = Mathf.Floor(timer / 60).ToString("00");
        string seconds = (timer % 60).ToString("00");
        timerText.text = $"{minutes}:{seconds}";
    }

    public void PlayerDied()
    {
        playerDied = true;
        ShowSurvivalTime();
    }

    void ShowSurvivalTime()
    {
        Debug.Log($"Survival Time: {timer} seconds");
        // You can display the survival time in UI or perform other actions.
    }
}
