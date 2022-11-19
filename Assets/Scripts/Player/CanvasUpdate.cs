using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasUpdate : MonoBehaviour
{
    [SerializeField] private Text ammoText;
    [SerializeField] private Text healthText;

    private PlayerData playerData;

    private void Start()
    {
        playerData = Resources.Load("ScriptableObject/PlayerData") as PlayerData;
    }

    private void Update()
    {
        ammoText.text = playerData.ammo.ToString();
        healthText.text = playerData.health.ToString();
    }
}
