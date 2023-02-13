using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseTrapActivation : BaseTrapClass
{
    // Start is called before the first frame update
    void Start()
    {
        // Set name of trap to Noise
        trapName = "Noise";
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something touched the noise trap");
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            if(!isTriggered){
                other.GetComponent<StateMachine_Robust>().getNoise(this.transform.position);
                isTriggered = true;

                this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                this.GetComponent<Renderer>().material.color = Color.grey;
            }
            Debug.Log(this.transform.position);
        }
    }
}
