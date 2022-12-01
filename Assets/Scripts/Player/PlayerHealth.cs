using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ThirdPersonShooterController))]
public class PlayerHealth : MonoBehaviour, ITakeDamage
{
    [SerializeField] private int health;

    private int maxHealth = 100;

    private ThirdPersonShooterController controller;
    private PlayerData playerData;
    private PhotonView PV;
    private PlayerManager playerManager;

    private void Awake()
    {
        controller = GetComponent<ThirdPersonShooterController>();
        PV = GetComponent<PhotonView>();
        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
    }
    void Start()
    {
        playerData = Resources.Load("ScriptableObject/PlayerData") as PlayerData;
        playerData.health = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        PV.RPC("RPC_TakeDamage", RpcTarget.All, amount);
    }

    [PunRPC]
    private void RPC_TakeDamage(int amount)
    {
        if (!PV.IsMine) return;

        playerData.health -= amount;
        if (playerData.health <= 0)
        {
            controller.isDead = true;
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (playerData.health >= 100)
        {
            playerData.health = maxHealth;
            return;
        }

        playerData.health = Mathf.Min(maxHealth, playerData.health += amount);
    }

    public void Die()
    {
        //Destroy(this.gameObject);
        playerManager.Die();
    }
}
