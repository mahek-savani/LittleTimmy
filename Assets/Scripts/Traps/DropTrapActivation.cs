using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropTrapActivation : BaseTrapClass
{
    // Start is called before the first frame update
    void Start()
    {
        // Set name of trap to Instant Death
        trapName = "Instant Death";
    }

    void OnTriggerEnter(Collider other)
    {
        // Debug.Log("Something touched the trap");
        if (!isTriggered && other.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            //data.trapActiveOrder.Add("dropTrap");
            other.GetComponent<StateMachine_Robust>().die();
            isTriggered = true;

            this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            this.GetComponent<Renderer>().material.color = Color.grey;
        }
    }
}
