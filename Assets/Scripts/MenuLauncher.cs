using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class MenuLauncher : MonoBehaviourPunCallbacks
{
    [SerializeField] private Menu[] menus;
    [SerializeField] private GameObject loadingMenu;

    [Header("Find Game UI")]
    [SerializeField] private Transform roomListView;
    [SerializeField] private RoomListItem roomItemPrefab;
    private List<RoomListItem> roomList = new List<RoomListItem>();

    [Space(10)]
    [Header("Create Game UI")]
    [SerializeField] private byte maxPlayers = 4; // default
    [SerializeField] private TMP_InputField createGameInputField;
    [SerializeField] private TMP_InputField roomCapacityInputField;

    [Space(10)]
    [Header("In-room UI")]
    [SerializeField] private GameObject roomMenu;
    [SerializeField] private TextMeshProUGUI roomName;
    [SerializeField] private PlayerListItem playerItemPrefab;
    [SerializeField] private Transform playerListView;
    [SerializeField] private GameObject playGameButton;
    [SerializeField] private Button playbtn;
    private List<PlayerListItem> playerList = new List<PlayerListItem>();

    void Start()
    {
        PhotonNetwork.JoinLobby();
        playbtn.onClick.AddListener(StartGame);
    }

    public void OpenMenu(string _menuName)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].menuName == _menuName)
            {
                menus[i].gameObject.SetActive(true);
            } else if (menus[i].gameObject.activeSelf)
            {
                menus[i].gameObject.SetActive(false);
            }
        }
    }

    public void OnClickCreateGame()
    {
        if (createGameInputField.text.Length >= 1)
        {
            RoomOptions roomOptions = new RoomOptions();

            if (roomCapacityInputField.text.Length >= 1 && byte.Parse(roomCapacityInputField.text) >= 2)
            {
                roomOptions.MaxPlayers = byte.Parse(roomCapacityInputField.text);

            } else
            {
                roomOptions.MaxPlayers = maxPlayers; // set the default room capacity to 4
            }

            PhotonNetwork.CreateRoom(createGameInputField.text, roomOptions);
        }
    }

    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        roomMenu.SetActive(false);
        Debug.Log("Left room");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedRoom()
    {
        roomName.text = "ROOM NAME: " + PhotonNetwork.CurrentRoom.Name;
        roomMenu.SetActive(true);

        if (loadingMenu.activeSelf)
        {
            loadingMenu.SetActive(false);
        }

        UpdatePlayerList();

        playGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }

    public void UpdatePlayerList()
    {
        //clear the current player list
        foreach (PlayerListItem player in playerList)
        {
            Destroy(player.gameObject);
        }

        if (PhotonNetwork.CurrentRoom == null) return;

        playerList.Clear();

        foreach(Player player in PhotonNetwork.PlayerList)
        {
            PlayerListItem newPlayer = Instantiate(playerItemPrefab, playerListView);
            newPlayer.SetPlayerName(player.NickName);
            playerList.Add(newPlayer);
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        UpdateRoomList(roomList);
    }

    private void UpdateRoomList(List<RoomInfo> list)
    {
        //First we delete all the exist room in List
        foreach (RoomListItem room in roomList)
        {
            Destroy(room.gameObject);
        }

        roomList.Clear();

        //Then we update all the new rooms in roomInfo
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].RemovedFromList) // prevent "empty-room" show up
            {
                continue;
            } else if (list[i].PlayerCount > 0)
            {
                RoomListItem newRoom = Instantiate(roomItemPrefab, roomListView);
                newRoom.SetRoomInfo(list[i]);
                roomList.Add(newRoom);
            }
        }

        Debug.Log("OnRoomListUpdate() was called");
        Debug.Log("this is number of rooms: " + list.Count);
    }

    public void JoinRoom(string roomName)
    {
        loadingMenu.SetActive(true);
        PhotonNetwork.JoinRoom(roomName);   
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connecting to Master!");
        PhotonNetwork.JoinLobby();
        Debug.Log("Joined lobby");
        PhotonNetwork.AutomaticallySyncScene = true; // Load scene for all the client when the Host changes scene
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        playGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

}
