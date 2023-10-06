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
        if (PhotonNetwork.IsConnectedAndReady)
        {
            if (scene.name == "MPBattlefield" || scene.name == "MPPool" || scene.name == "MPCity" || scene.name == "MPWarehouse" || scene.name == "MPStore")
            {
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
            }
        }

        if (scene.buildIndex == 0) // We're in the menu scene
        {
            if (PhotonNetwork.IsConnectedAndReady)
            {
                PhotonNetwork.Destroy(gameObject); // Destroy the RoomManager object when returning to the menu scene
            }
        }
    }
}
