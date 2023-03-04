using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeTrap : BaseTrapClass
{
    public Color activeColor = new Color(0.298f, 0.831f, 1f, 1f);
    public Color inactiveColor = new Color(0.392f, 0.392f, 0.392f, 1f); 

    void Start(){
        // Set trap to "Freeze" and if trap is a Pickup, we might
        // assign it specific effects (for now it is simply shrunk)
        trapName = "Freeze";
        transform.GetComponent<Renderer>().material.color = inactiveColor;
        if(this.gameObject.layer == LayerMask.NameToLayer("Pickup")){
            this.transform.localScale = new Vector3(1f, 0.1f, 1f);
        }
    }

    void Update(){
        if(!isTriggered){
            this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            transform.GetComponent<Renderer>().material.color = activeColor;
        }
        else
        {
            transform.GetComponent<Renderer>().material.color = inactiveColor;
        }
    }

    void OnTriggerEnter(Collider triggerObject){
        // If an enemy enters the trigger box, freeze enemy and deactivate trap
        if(!isTriggered && triggerObject.gameObject.layer == LayerMask.NameToLayer("Enemies")) {
            triggerObject.gameObject.GetComponent<StateMachine_Robust>().getUnconscious();
            isTriggered = true;

            data.trapActiveOrder.Add("freezeTrap");

            this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            //this.GetComponent<Renderer>().material.color = Color.grey;
        }
    }
}
