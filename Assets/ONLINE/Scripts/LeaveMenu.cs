using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class LeaveMenu : MonoBehaviourPunCallbacks
{
    public GameObject leaveGameMenu; // Reference to the leave game menu UI object

    private bool isLeaving = false; // Flag to track if the player is in the process of leaving

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleLeaveGameMenu();
        }
    }

    public void LeaveGame()
    {
        if (!isLeaving && PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            isLeaving = true;
            PhotonNetwork.LeaveRoom(); // Leave the current room
            PhotonNetwork.Disconnect();
            PhotonNetwork.AutomaticallySyncScene = false;
            PhotonNetwork.LoadLevel(0);
        }

        // Hide the leave game menu
        leaveGameMenu.SetActive(false);
    }

    public void Cancel()
    {
        // Hide the leave game menu without leaving the game
        leaveGameMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void ToggleLeaveGameMenu()
    {
        bool isMenuActive = leaveGameMenu.activeSelf;
        leaveGameMenu.SetActive(!isMenuActive);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
