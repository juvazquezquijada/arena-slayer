using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Leave : MonoBehaviourPunCallbacks
{
    void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
    }
}
