using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;

public class ItemManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Item[] items;

    private int currentItemIndex;
    private int previousItemindex = -1;

    private PhotonView PV;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    private void Start()
    {
        if (PV.IsMine)
        {
            EquipItem(0);
        }
    }

    private void Update()
    {
        CheckEquipItem();
    }

    public void EquipItem(int index)
    {
        if (index == previousItemindex)
            return;

        currentItemIndex = index;

        items[currentItemIndex].gameObject.SetActive(true);

        if (previousItemindex != -1)
        {
            items[previousItemindex].gameObject.SetActive(false);
        }

        previousItemindex = currentItemIndex;

        if (PV.IsMine)
        {
            Hashtable hash = new Hashtable();

            hash.Add("currentItemIndex", currentItemIndex);

            PhotonNetwork.LocalPlayer.SetCustomProperties(hash); 
        }
    }

    private void CheckEquipItem()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                EquipItem(i);
                break;
            }
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
        {
            if (currentItemIndex >= items.Length - 1)
            {
                EquipItem(0);
            } else
            {
                EquipItem(currentItemIndex + 1);
            }
        } else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
        {
            if (currentItemIndex <= 0)
            {
                EquipItem(items.Length - 1);
            } else
            {
                EquipItem(currentItemIndex - 1);
            }
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!PV.IsMine && targetPlayer == PV.Owner)
        {
            EquipItem((int)changedProps["currentItemIndex"]);
        }
    }
}
