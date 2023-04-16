using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCDieOnTouch : MonoBehaviour
{
    public PitTrap pitTrap;
    public PlayerController playercontroller;
    // private int check = 0;
    private GameObject trapInHand;
    private string temmp;
    private GameObject trap;

    // Function to respawn pickup traps
    void respawnPickupTraps(GameObject c)
    {
        // When player falls into pit with trap in hand , trap will be inactive so make it active if false
        if (c.activeInHierarchy==false)
        {
            c.SetActive(true);
        }
        
        // If we encounter noiseTrap, get child component "sphere" which contains function for respawn
        if (c.CompareTag("noiseTrap"))
        {
            trap = c.transform.GetChild(0).gameObject;
            trap.GetComponent<NoiseTrapActivation>().respawnMe();
        }
        // else If freezeTrap, the respawn function is already within parent component
        else if (c.CompareTag("freezeTrap"))
        {
            c.GetComponent<FreezeTrap>().respawnMe();
        }
    }
    void OnTriggerStay(Collider c)
    {
        if (!pitTrap.trapActive)
        {
            // condition if player falls into pit 
            if (c.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                PlayerDamage damageInterface = c.gameObject.GetComponent<PlayerDamage>();
                // player should take 1 damage
                damageInterface.TakeDamage(1);
                // Respawn the player at start loc
                playercontroller.RespawnPlayer();
                
                /*
                // Condition if player falls into pit with pickup trap in hand
                trapInHand = c.gameObject.GetComponent<PlayerController>().trapInHand;
                if (trapInHand) {
                    // set trapPlacer elocked as 1 otherwise won't be able to use E to pickup
                    c.gameObject.GetComponent<TrapPlacer>().eLocked = 1;
                    // call function to respawn the pickup trap
                    respawnPickupTraps(trapInHand);
                }
                */
                
            }
            // condition if pickup trap falls into pit
            else if (c.gameObject.layer == LayerMask.NameToLayer("Pickup"))
            {
                // call function to respawn the pickup trap
                respawnPickupTraps(c.gameObject);
            }

        }
    }
    
}
