using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDeath : MonoBehaviour
{
    void onTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == 7)
        {
            StateMachine_Robust SM = c.gameObject.GetComponent<StateMachine_Robust>();

            SM.die();
        }
    }
}
