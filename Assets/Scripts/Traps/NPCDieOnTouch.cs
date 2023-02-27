using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCDieOnTouch : MonoBehaviour
{
    public PitTrap pitTrap;
    public PlayerController playercontroller;
    private int check = 0;

    void OnTriggerStay(Collider c)
    {
        if (!pitTrap.trapActive && c.gameObject.layer == 3)
        {
            PlayerDamage damageInterface = c.gameObject.GetComponent<PlayerDamage>();

            damageInterface.TakeDamage(1);

            playercontroller.RespawnPlayer();

        }
    }
    
}
