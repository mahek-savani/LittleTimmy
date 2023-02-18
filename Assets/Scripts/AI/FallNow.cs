using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallNow : MonoBehaviour
{

    public Transform fallPoint;
    private void OnTriggerStay(Collider c)
    {
        if (c.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            StateMachine_Robust SM = c.gameObject.GetComponent<StateMachine_Robust>();
            
            SM.die();
            SM.agent.isStopped = false;
            SM.agent.SetDestination(fallPoint.position);

        }
    }
}
