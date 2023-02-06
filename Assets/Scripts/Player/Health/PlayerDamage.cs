using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    // Start is called before the first frame update
    public int maxHealth;
    public int currentHealth;
    
    public Health_Bar healthbar;

    public PlayerController playerController;
    

    void Start()
    {   
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage( int damage)
    {
        currentHealth -= damage;
        healthbar.SetHealth(currentHealth);

        if (currentHealth == 0)
        {
            playerController.playerDie();
        }
    }
}
