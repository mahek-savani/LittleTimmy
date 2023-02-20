using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Spike : MonoBehaviour
{
    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == 7)
        {
            data.trapActiveOrder.Add("spikeTrap");
            StateMachine_Robust SM = c.gameObject.GetComponent<StateMachine_Robust>();

            SM.die();
        }

        if (c.gameObject.layer == 3)
        {
            PlayerDamage damageInterface = c.gameObject.GetComponent<PlayerDamage>();

            damageInterface.TakeDamage(1);
        }
    }
}
