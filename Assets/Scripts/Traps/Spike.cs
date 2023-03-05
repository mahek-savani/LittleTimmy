using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Spike : MonoBehaviour
{
    public bool isTrapMoving = false;

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == 7)
        {
            //data.trapActiveOrder.Add("spikeTrap-1");
            data.spikeTrap.Add(1);
            StateMachine_Robust SM = c.gameObject.GetComponent<StateMachine_Robust>();

            SM.die();
        }

        if (c.gameObject.layer == 3)
        {
            Debug.Log("Hit the player");
            
            PlayerDamage damageInterface = c.gameObject.GetComponent<PlayerDamage>();

            damageInterface.TakeDamage(1);
        }

        if (c.gameObject.layer == 12)
        {
            Debug.Log("Hit the obstacle");
            isTrapMoving = false;
        }
    }
}
