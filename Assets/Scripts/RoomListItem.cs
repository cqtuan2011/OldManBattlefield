using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class RoomListItem : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI roomName;
    [SerializeField] private TextMeshProUGUI roomCapacity;

    private MenuLauncher menuLauncher;

    private void Awake()
    {
        menuLauncher = FindObjectOfType<MenuLauncher>();
    }

    public void SetRoomInfo(RoomInfo _roomInfo)
    {
        roomName.text = _roomInfo.Name;
        roomCapacity.text = _roomInfo.PlayerCount + "/" + _roomInfo.MaxPlayers;

    }

    public void OnClickRoom()
    {
        menuLauncher.JoinRoom(roomName.text);
        Debug.Log("Joined room: " + roomName.text);
    }
}
