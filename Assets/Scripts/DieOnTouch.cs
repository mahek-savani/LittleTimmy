using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieOnTouch : MonoBehaviour
{
    public PitTrap pitTrap;

    void OnTriggerEnter(Collider c)
    {
        //Debug.Log("It's activating");
        if (!pitTrap.trapActive && c.gameObject.layer == 7)
        {
            c.GetComponent<StateMachine_Robust>().die();
        }
    }
}
