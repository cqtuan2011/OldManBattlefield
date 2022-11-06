using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, ITakeDamage
{
    [SerializeField] private int health;

    private int maxHealth = 100;

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Debug.Log("Player is died");
        }
    }

    public void Heal(int amount)
    {
        if (health >= 100)
        {
            health = maxHealth;
            return;
        }

        health = Mathf.Min(maxHealth, health += amount);
    }

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
