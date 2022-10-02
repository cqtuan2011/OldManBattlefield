using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private Item[] items;

    private short currentItemIndex;
    private short previousItemindex = -1;

    private void Start()
    {
        PhotonView photonView = GetComponentInParent<PhotonView>();

        if (photonView.IsMine)
        {
            EquipItem(0);
        }
    }

    public void EquipItem(short index)
    {
        currentItemIndex = index;

        items[currentItemIndex].gameObject.SetActive(true);

        if (previousItemindex != -1)
        {
            items[previousItemindex].gameObject.SetActive(false);
        }

        previousItemindex = currentItemIndex;
    }
}
