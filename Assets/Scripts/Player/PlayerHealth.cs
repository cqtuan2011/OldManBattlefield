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

    private void Awake()
    {
        controller = GetComponent<ThirdPersonShooterController>();
    }
    void Start()
    {
        playerData = Resources.Load("ScriptableObject/PlayerData") as PlayerData;
        playerData.health = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        playerData.health -= amount;
        if (playerData.health <= 0)
        {
            controller.isDead = true;
            Debug.Log("Player is died");
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
}
