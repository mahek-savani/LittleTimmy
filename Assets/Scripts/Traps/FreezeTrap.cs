using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeTrap : BaseTrapClass
{
    public Color activeColor = new Color(0.298f, 0.831f, 1f, 1f);
    public Color inactiveColor = new Color(0.392f, 0.392f, 0.392f, 1f);

    private GameObject ring;

    public void respawnMe()
    {
        isTriggered = true;
        transform.GetComponent<Renderer>().material.color = inactiveColor;
        gameObject.GetComponent<Respawn>().respawnMe();
    }

    void Start(){
        // Set trap to "Freeze" and if trap is a Pickup, we might
        // assign it specific effects (for now it is simply shrunk)
        trapName = "Freeze";
        transform.GetComponent<Renderer>().material.color = inactiveColor;
        if(this.gameObject.layer == LayerMask.NameToLayer("Pickup")){
            this.transform.localScale = new Vector3(1f, 0.1f, 1f);
        }

        GameObject particle = this.gameObject.transform.GetChild(0).gameObject;
        ring = particle.transform.GetChild(1).gameObject;
        //ring.GetComponent<ParticleSystem>().startColor = Color.white;
    }

    void Update(){
        if(!isTriggered){
            //this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            // ring.GetComponent<ParticleSystem>().startColor = new Color(18/255f, 174/255f, 1f);
            transform.GetComponent<Renderer>().material.color = activeColor;
              ring.GetComponent<ParticleSystem>().enableEmission =false;
        }
        else
        {

            transform.GetComponent<Renderer>().material.color = inactiveColor;
              ring.GetComponent<ParticleSystem>().enableEmission  = true;
        }
    }

    void OnTriggerEnter(Collider triggerObject){
        // If an enemy enters the trigger box, freeze enemy and deactivate trap
        if(!isTriggered && triggerObject.gameObject.layer == LayerMask.NameToLayer("Enemies")) {
            triggerObject.gameObject.GetComponent<StateMachine_Robust>().stop(transform.position);
            triggerObject.gameObject.GetComponent<StateMachine_Robust>().getUnconscious(3.5f);


            isTriggered = true;

            //data.trapActiveOrder.Add("freezeTrap");
            data.freezeTrap.Add(1);
            //this.gameObject.transform.GetChild(0).gameObject.SetActive(false);

            // ring.GetComponent<ParticleSystem>().startColor = Color.white;
            ring.GetComponent<ParticleSystem>().Clear();
            //ring.GetComponent<ParticleSystem>().Emit(1);

            //this.GetComponent<Renderer>().material.color = Color.grey;
        }
    }
}
