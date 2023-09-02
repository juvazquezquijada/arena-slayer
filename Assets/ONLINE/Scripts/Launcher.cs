using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance;

    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText, mapNameText;
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject PlayerListItemPrefab;
    [SerializeField] GameObject startGameButton;
    [SerializeField] GameObject RoomManager;
    [SerializeField] AudioSource audioSource;
    [SerializeField] TMP_Dropdown mapDropdown; // Reference to the map selection dropdown
    [SerializeField] TMP_Text onlineUsersText; // Reference to the Text component that will show online user count
    private string selectedMap; // Store the selected map

    public AudioClip clickSound;
    public AudioClip backSound;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();

        // Populate map selection dropdown
        List<string> maps = GetAvailableMaps(); // Replace with your implementation to get the available maps
        mapDropdown.ClearOptions();
        mapDropdown.AddOptions(maps);

        // Set the selected map to the first option in the dropdown
        mapDropdown.value = 0;
        selectedMap = maps[0];
    }

    void Update()
    {
        UpdateOnlineUsersText(); // Update the online user count
    }

    // Map selection event handler
    public void OnMapSelected(int index)
    {
        mapDropdown.onValueChanged.AddListener(OnMapSelected);

        selectedMap = mapDropdown.options[index].text;
        Debug.Log("Selected Map: " + selectedMap);
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("TitleScene");
        Debug.Log("Disconnected");
        Destroy(RoomManager.gameObject);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
        UpdateOnlineUsersText(); // Update the online user count
    }

    public override void OnJoinedLobby()
    {
        MenuManager.Instance.OpenMenu("title");
        Debug.Log("Joined Lobby");
        UpdateOnlineUsersText(); // Update the online user count
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;
        }

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.CustomRoomProperties = new Hashtable { { "map", selectedMap } };
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "map" };


        PhotonNetwork.CreateRoom(roomNameInputField.text, roomOptions);
        MenuManager.Instance.OpenMenu("loading");
    }

    // Joined room callback
    public override void OnJoinedRoom()
    {
        MenuManager.Instance.OpenMenu("room");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        Player[] players = PhotonNetwork.PlayerList;

        foreach (Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }

        startGameButton.SetActive(PhotonNetwork.IsMasterClient);

        // Display selected map for local player
        mapNameText.text = "Selected map: " + selectedMap;

        // Check if the custom property "map" exists in the room
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("map", out object mapValue))
        {
            string mapName = mapValue as string;
            mapNameText.text = "Selected map: " + mapName;
        }
        else
        {
            Debug.LogError("Failed to retrieve selected map from custom room properties.");
        }

        UpdateOnlineUsersText(); // Update the online user count
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room Creation Failed: " + message;
        Debug.LogError("Room Creation Failed: " + message);
        MenuManager.Instance.OpenMenu("error");
    }

    public void StartGame()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("map"))
        {
            string selectedMap = PhotonNetwork.CurrentRoom.CustomProperties["map"].ToString();
            PhotonNetwork.LoadLevel(selectedMap);
        }
        else
        {
            Debug.LogError("Failed to start the game. Map property is not set in the custom room properties.");
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("loading");
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenu("title");
    }

    public void PlaySound()
    {
        audioSource.PlayOneShot(clickSound);
    }

    public void BackSound()
    {
        audioSource.PlayOneShot(backSound);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }

        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
                continue;
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }

        UpdateOnlineUsersText(); // Update the online user count
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);

        // When a new player enters the room, update their UI to display the selected map
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("map", out object mapValue))
        {
            string mapName = mapValue as string;
            photonView.RPC("UpdateMapDisplayForNewPlayer", newPlayer, mapName);
        }
    }
    private List<string> GetAvailableMaps()
    {
        List<string> maps = new List<string>();
        maps.Add("MPPool");
        maps.Add("MPBattlefield");
        maps.Add("MPCity");
        maps.Add("MPWarehouse");
        // Add more maps as needed

        return maps;
    }

    private void UpdateOnlineUsersText()
    {
        int onlinePlayers = PhotonNetwork.CountOfPlayers;
        onlineUsersText.text = "Players currently active (Including you): " + onlinePlayers;
    }

    // Remote Procedure Call (RPC) method to update the map display for new players
    [PunRPC]
    void UpdateMapDisplayForNewPlayer(string mapName)
    {
        mapNameText.text = "Selected map: " + mapName;
    }
}
