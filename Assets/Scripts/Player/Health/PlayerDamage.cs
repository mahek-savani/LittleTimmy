using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    // Start is called before the first frame update
    public int maxHealth = 4;
    public int currentHealth = 4;
    
    public Health_Bar healthbar;

    public PlayerController playerController;
    
    private float timeRed;
    private Color origColor;

    void Start()
    {
        data.healthRemaining = maxHealth;
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
        origColor = this.GetComponent<Renderer>().material.color;
    }

    void Update(){
        if(timeRed > 0) timeRed -= 1f * Time.deltaTime;
        else this.GetComponent<Renderer>().material.color = origColor;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        data.healthRemaining = currentHealth;
        data.healthLost = data.healthLost + 1;
        healthbar.SetHealth(currentHealth);
        this.GetComponent<Renderer>().material.color = Color.red;
        timeRed = 0.3f;
        if (currentHealth == 0)
        {
            playerController.playerDie();
        }
    }

    public void HealDamage()
    {
        currentHealth++;
        healthbar.SetHealth(currentHealth);
    }
}
