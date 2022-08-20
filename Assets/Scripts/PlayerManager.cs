using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;

public class PlayerManager : MonoBehaviour
{
    private PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (photonView.IsMine) // if the photon view is owned by the local player
        {
            CreateController();
        }
    }

    private void CreateController()
    {
        Debug.Log("Created controller");
    }
}
