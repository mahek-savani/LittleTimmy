using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseTrapActivation : MonoBehaviour
{
    public bool isTriggered = true;
    //public Transform cylinder;


    void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.name);
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            //Debug.Log("Enemy Inside sphere");
            if(!isTriggered){
                // Debug.Log(transform.position);
                other.GetComponent<StateMachine_Robust>().getNoise(transform.position);
                //Debug.Log("getNoise called");
                isTriggered = true;
            }
        }
    }
}
