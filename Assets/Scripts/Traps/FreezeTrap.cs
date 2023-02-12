using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeTrap : MonoBehaviour
{
    //public Vector3 scaleChange = new Vector3(-0.25f, -0.25f, -0.25f);

    private bool triggered = false;

    void Start(){
        if(this.gameObject.layer == LayerMask.NameToLayer("Pickup")){
            this.transform.localScale = new Vector3(1f, 0.1f, 1f);
        }
    }

    void OnTriggerEnter(Collider triggerObject){
        if(!triggered && triggerObject.gameObject.layer == LayerMask.NameToLayer("Enemies")) {
            data.trapActiveOrder.Add("freezeTrap");
            triggerObject.gameObject.GetComponent<StateMachine_Robust>().MakeIncapacitated(3.0f);
            triggered = true;
        }
    }
}
