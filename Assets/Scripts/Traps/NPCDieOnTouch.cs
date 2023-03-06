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
        if (!pitTrap.trapActive)
        {
            if (c.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                PlayerDamage damageInterface = c.gameObject.GetComponent<PlayerDamage>();

                damageInterface.TakeDamage(1);

                playercontroller.RespawnPlayer();
            }
            else if (c.gameObject.layer == LayerMask.NameToLayer("Pickup") || c.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
            {
                if (c.gameObject.GetComponent<NoiseTrapActivation>())
                {
                    c.gameObject.GetComponent<NoiseTrapActivation>().respawnMe();
                }
                else if (c.gameObject.GetComponent<FreezeTrap>())
                {
                    c.gameObject.GetComponent<FreezeTrap>().respawnMe();
                }
            }

        }
    }
    
}
