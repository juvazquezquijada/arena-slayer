using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;

    public int targetKills = 10; // The number of kills needed to end the game
    public TextMeshProUGUI winnerText; // Reference to the TMPro text component to display the winner's name

    public List<PlayerManager> playerManagers = new List<PlayerManager>(); // List of PlayerManager instances in the scene

    private bool isGameEnded = false; // Flag to check if the game has ended

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void RegisterPlayerManager(PlayerManager playerManager)
    {
        if (!playerManagers.Contains(playerManager))
        {
            playerManagers.Add(playerManager);
        }
    }

    public void UnregisterPlayerManager(PlayerManager playerManager)
    {
        if (playerManagers.Contains(playerManager))
        {
            playerManagers.Remove(playerManager);
        }
    }

    public void AddKill(PlayerManager playerManager)
    {
        if (isGameEnded)
            return;

        playerManager.GetKill();

        CheckGameEndConditions();
    }

    void CheckGameEndConditions()
    {
        foreach (PlayerManager playerManager in playerManagers)
        {
            if (playerManager.GetKills() >= targetKills)
            {
                // Game has ended
                isGameEnded = true;
                string winnerName = playerManager.GetPlayerName();
                winnerText.text = "Winner: " + winnerName;
                winnerText.gameObject.SetActive(true);

                Debug.Log("Game Over! Player " + winnerName + " wins!");

                // Trigger game end logic here (e.g., display a game over screen, show stats, etc.)

                // You can also implement a restart or back to menu functionality here
                // Example: StartCoroutine(RestartGameCoroutine());
                break;
            }
        }
    }

    IEnumerator RestartGameCoroutine()
    {
        yield return new WaitForSeconds(5f); // Wait for 5 seconds before restarting the game

        // Restart the game logic here
    }
}
