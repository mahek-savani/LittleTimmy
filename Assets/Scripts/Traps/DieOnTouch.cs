using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieOnTouch : MonoBehaviour
{
    public PitTrap pitTrap;
    // Start is called before the first frame update

    void Update()
    {
        //Debug.Log(pitTrap.trapActive);
    }
    void OnTriggerStay(Collider c)
    {
        //Debug.Log("It's activating");
        if (!pitTrap.trapActive && c.gameObject.layer == 7)
        {
            c.GetComponent<StateMachine_Robust>().die();
        }

        if (!pitTrap.trapActive && c.gameObject.CompareTag("Player"))
        {
            //player
            StartCoroutine(c.GetComponent<playerMovement>().playerDie(2f));
        }
    }
}
