using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class FFAGameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] Scoreboard Scoreboard;
    public CanvasGroup scoreboard;
    public Camera mapCamera;
    public float gameDuration = 300f; // 5 minutes in seconds
    public TMP_Text timerText;
    public TMP_Text gameTimerText;
    public GameObject endGame;
    public AudioSource gameMusicSource;
    public AudioSource lastSecondsMusicSource;
    public float fadeDuration = 1f; // Duration of the fade in seconds
    
    private Player winningPlayer;

    public WinnerUI winnerUI;

    public static FFAGameManager Instance;

    public bool isGameOver = false;
    private double networkTime = 0f;
    private double syncTime = 0f;

    private bool isTimerInitialized = false;
    private bool isHost = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;


    }

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            isHost = true;
            StartGame();
            
        }
        Scoreboard.ResetStatsForAllPlayers();
        mapCamera.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!isGameOver && isHost)
        {
            double elapsedSeconds = PhotonNetwork.Time - syncTime;
            if (elapsedSeconds >= gameDuration)
            {
                isGameOver = true;
                EndGame();
            }
            else
            {
                double remainingSeconds = gameDuration - elapsedSeconds;
                UpdateTimerText(remainingSeconds);

                // Check if there are 30 seconds remaining
                if (remainingSeconds <= 60 && !lastSecondsMusicSource.isPlaying)
                {
                    // Stop the initial game music and play the 30-second music
                    photonView.RPC("RPC_StopGameMusic", RpcTarget.All);
                    photonView.RPC("RPC_PlayLastSecondsMusic", RpcTarget.All);
                }
            }
        }
        else if (isTimerInitialized)
        {
            double remainingSeconds = gameDuration - (PhotonNetwork.Time - networkTime);
            UpdateTimerText(remainingSeconds);
        }
    }

    private void UpdateTimerText(double remainingSeconds)
    {
        int minutes = Mathf.FloorToInt((float)(remainingSeconds / 60f));
        int seconds = Mathf.FloorToInt((float)(remainingSeconds % 60f));

        // Format the timer text
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        gameTimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void StartGame()
    {
        // Activate the map camera at the start
        mapCamera.gameObject.SetActive(true);

        isGameOver = false;
        networkTime = PhotonNetwork.Time;
        syncTime = networkTime;
        isTimerInitialized = true;
        // Play the initial game music
        photonView.RPC("RPC_PlayGameMusic", RpcTarget.All);
        

    }

    public void EndGame()
    {
        DetermineWinner();

        // Notify all clients that the game has ended
        photonView.RPC("RPC_GameEnded", RpcTarget.All, winningPlayer);


    }

    public void DetermineWinner()
    {
        Player[] players = PhotonNetwork.PlayerList;
        int maxKills = -1;
        winningPlayer = null;

        foreach (Player player in players)
        {
            if (player.CustomProperties.TryGetValue("kills", out object killsValue))
            {
                int kills = (int)killsValue;
                if (kills > maxKills)
                {
                    maxKills = kills;
                    winningPlayer = player;
                }
            }
        }
    }



    [PunRPC]
    void RPC_GameEnded(Player winner)
    {
        isGameOver = true;

        // Show the scoreboard
        scoreboard.alpha = 1;

        timerText.gameObject.SetActive(false);

        endGame.gameObject.SetActive(true);

        // Switch to the map camera
        mapCamera.gameObject.SetActive(true);

        // Display the winner's name
        winnerUI.SetWinner(winner);

        if (lastSecondsMusicSource.isPlaying)
        {
            StartCoroutine(FadeOutMusic(lastSecondsMusicSource, fadeDuration));
        }
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(networkTime);
        }
        else
        {
            networkTime = (double)stream.ReceiveNext();
            syncTime = PhotonNetwork.Time;
            isTimerInitialized = true;
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (isHost && isTimerInitialized)
        {
            // Synchronize the timer with the newly joined player
            photonView.RPC("SyncNetworkTime", newPlayer, networkTime);
        }
    }

    [PunRPC]
    private void SyncNetworkTime(double time)
    {
        networkTime = time;
        syncTime = PhotonNetwork.Time;
        isTimerInitialized = true;
    }

    [PunRPC]
    void RPC_PlayGameMusic()
    {
        gameMusicSource.Play();
    }

    [PunRPC]
    void RPC_PlayLastSecondsMusic()
    {
        lastSecondsMusicSource.Play();
    }

    [PunRPC]
    void RPC_StopGameMusic()
    {
        gameMusicSource.Stop();
    }

    private IEnumerator FadeOutMusic(AudioSource audioSource, float duration)
    {
        // Store the initial volume of the audio source
        float startVolume = audioSource.volume;

        // Calculate the target volume (0)
        float targetVolume = 0f;

        // Calculate the current time and the end time of the fade
        float currentTime = 0f;
        float endTime = currentTime + duration;

        // Gradually decrease the volume over time
        while (currentTime < endTime)
        {
            // Calculate the current volume based on the fade progress
            float t = currentTime / duration;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, t);

            // Increment the current time
            currentTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the volume is set to the target volume
        audioSource.volume = targetVolume;

        // Stop the audio source
        audioSource.Stop();
    }

}
