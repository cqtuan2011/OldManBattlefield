using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roomName;

    private MenuLauncher menuLauncher;

    private void Awake()
    {
        menuLauncher = FindObjectOfType<MenuLauncher>();
    }

    public void SetRoomName(string _roomName)
    {
        roomName.text = _roomName;
    }

    public void OnClickRoom()
    {
        menuLauncher.JoinRoom(roomName.text);
        Debug.Log("Joined room: " + roomName.text);
    }
}
