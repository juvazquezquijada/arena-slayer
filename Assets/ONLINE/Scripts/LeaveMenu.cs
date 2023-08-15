using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class LeaveMenu : MonoBehaviourPunCallbacks
{
    public GameObject leaveGameMenu; // Reference to the leave game menu UI object

    private bool menuActive = false; // Flag to track if the player is in the process of leaving

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !menuActive)
        {
            ToggleLeaveGameMenu();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && menuActive)
        {
            Cancel();
        }
    }

    public void LeaveGame()
    {
        if  (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            
            PhotonNetwork.LeaveRoom(); // Leave the current room
            PhotonNetwork.Disconnect();
            PhotonNetwork.AutomaticallySyncScene = false;
            PhotonNetwork.LoadLevel(2);
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
        menuActive = false;
    }

    private void ToggleLeaveGameMenu()
    {
        menuActive = true;
        bool isMenuActive = leaveGameMenu.activeSelf;
        leaveGameMenu.SetActive(!isMenuActive);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;


    }
}
