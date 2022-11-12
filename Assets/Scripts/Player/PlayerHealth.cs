using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ThirdPersonShooterController))]
public class PlayerHealth : MonoBehaviour, ITakeDamage
{
    [SerializeField] private int health;
    [SerializeField] private Text healthText;

    private int maxHealth = 100;

    private ThirdPersonShooterController controller;

    private void Awake()
    {
        controller = GetComponent<ThirdPersonShooterController>();
    }
    void Start()
    {
        health = maxHealth;
        UpdateHealthText();
    }

    void Update()
    {
        
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            controller.isDead = true;
            Debug.Log("Player is died");
        }
        UpdateHealthText();
    }

    public void Heal(int amount)
    {
        if (health >= 100)
        {
            health = maxHealth;
            return;
        }

        health = Mathf.Min(maxHealth, health += amount);
        UpdateHealthText();
    }

    private void UpdateHealthText()
    {
        healthText.text = health.ToString();
    }
}
