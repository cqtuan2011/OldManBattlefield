using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField playerNameInputField;

    [SerializeField] private GameObject playerNameInputFieldPanel;
    [SerializeField] private GameObject loadingPanel;

    public void OnClickPlayButton()
    {
        if (!string.IsNullOrEmpty(playerNameInputField.text))
        {
            PhotonNetwork.NickName = playerNameInputField.text;
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.AutomaticallySyncScene = true;

            playerNameInputFieldPanel.SetActive(false);
            loadingPanel.SetActive(true);

            Debug.Log("Connecting to Sever...");
            Debug.Log("Player name is: " + PhotonNetwork.NickName);
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Sever");

        SceneLoader.Instance.LoadMenuScene();
    }
}
