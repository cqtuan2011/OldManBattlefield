using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private Item[] items;

    private int currentItemIndex;
    private int previousItemindex = -1;

    private void Start()
    {
        PhotonView photonView = GetComponentInParent<PhotonView>();

        if (photonView.IsMine)
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
}
