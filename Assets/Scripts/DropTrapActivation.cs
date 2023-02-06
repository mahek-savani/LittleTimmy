using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropTrapActivation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something touched the trap");
        other.GetComponent<StateMachine_Robust>().die();
        //TODO: Ask Aaron if can tag NPCs, then can use this
        // if (other.CompareTag("Enemy"))
        // {
        //     StateMachine_Robust smr = other.transform.GetComponent<StateMachine_Robust>();
        //     if(smr != null){
        //         smr.die();
        //     }
        // }
    }
}
