using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.buildIndex == 1) // We're in the game scene
        {
            if (PhotonNetwork.IsConnectedAndReady)
            {
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
            }
            else
            {
                Debug.LogError("Failed to instantiate PlayerManager. PhotonNetwork is not connected and ready.");
            }
        }
        else if (scene.buildIndex == 0) // We're in the menu scene
        {
            if (PhotonNetwork.IsConnectedAndReady)
            {
                PhotonNetwork.Destroy(gameObject); // Destroy the RoomManager object when returning to the menu scene
            }
        }
    }
}
