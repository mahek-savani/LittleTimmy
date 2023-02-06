using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    // Start is called before the first frame update
    public int maxHealth;
    public int currentHealth;
    
    public Health_Bar healthbar;
    

    void Start()
    {   
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage( int damage)
    {
        currentHealth -= damage;
        healthbar.SetHealth(currentHealth);

    }
}
