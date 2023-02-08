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
        // Debug.Log("Something touched the trap");
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            other.GetComponent<StateMachine_Robust>().die();
        }
    }
}
