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

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;

    public float gameDuration = 300f; // 5 minutes in seconds

    bool isGameRunning = false;
    [SerializeField] float elapsedTime = 0f;

    [SerializeField] TMP_Text timerText;
    [SerializeField] TMP_Text winnerText;

    public List<PlayerManager> playerManagers = new List<PlayerManager>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Find all PlayerManager components in the scene and add them to the list
        PlayerManager[] managers = FindObjectsOfType<PlayerManager>();
        playerManagers.AddRange(managers);
    }

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartGame();
        }
    }

    void Update()
    {
        if (isGameRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerUI();
        }
    }

    public bool IsGameRunning
    {
        get { return isGameRunning; }
    }

    public float ElapsedTime
    {
        get { return elapsedTime; }
    }

    public float GameDuration
    {
        get { return gameDuration; }
    }

    public void StartGame()
    {
        isGameRunning = true;
    }

    public void EndGame()
    {
        isGameRunning = false;

        // Determine the winner based on kills
        PlayerManager winner = playerManagers[0];
        bool isTie = false;

        foreach (PlayerManager manager in playerManagers)
        {
            int playerKills = manager.GetKills();
            int winnerKills = winner.GetKills();

            if (playerKills > winnerKills)
            {
                winner = manager;
                isTie = false;
            }
            else if (playerKills == winnerKills)
            {
                isTie = true;
            }
        }

        if (isTie)
        {
            // Handle tie scenario (e.g., display tie message)
            winnerText.text = "It's a tie!";
            Debug.Log("Game Over! It's a tie!");
        }
        else
        {
            // Display the winner's name
            winnerText.text = "Winner: " + winner.GetPlayerName().ToString();
            Debug.Log("Game Over! Winner: " + winner.GetPlayerName());
        }
    }


    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (targetPlayer == PhotonNetwork.LocalPlayer && changedProps.ContainsKey("kills"))
        {
            CheckEndGameCondition();
        }
    }

    void CheckEndGameCondition()
    {
        if (isGameRunning && elapsedTime >= gameDuration)
        {
            EndGame();
        }
        else if (isGameRunning && elapsedTime < gameDuration)
        {
            float remainingTime = Mathf.Clamp(gameDuration - elapsedTime, 0f, gameDuration);
            int minutes = Mathf.FloorToInt(remainingTime / 60f);
            int seconds = Mathf.FloorToInt(remainingTime % 60f);

            if (minutes == 0 && seconds == 0)
            {
                EndGame();
            }
        }
    }

    void UpdateTimerUI()
    {
        float remainingTime = Mathf.Clamp(gameDuration - elapsedTime, 0f, gameDuration);
        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);
        string timerString = string.Format("{0:00}:{1:00}", minutes, seconds);
        timerText.text = timerString;
    }
}
