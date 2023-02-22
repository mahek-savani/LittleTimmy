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
        isTriggered = false;
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.name);
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            //Debug.Log("Enemy Inside sphere");
            if(!isTriggered){
                // Debug.Log(transform.position);
                other.GetComponent<StateMachine_Robust>().getNoise(transform.position);
                gameObject.GetComponent<MeshRenderer>().enabled = false;
                //Debug.Log("getNoise called");
                isTriggered = true;
                data.trapActiveOrder.Add("noiseTrap-0");
                data.trapActiveOrder.Add("noiseTrap-1");

                // this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                // this.GetComponent<Renderer>().material.color = Color.grey;
            }
        }
    }

    public void visible()
    {
        transform.GetComponent<MeshRenderer>().enabled = true;
    }

    public void invisible()
    {
        transform.GetComponent<MeshRenderer>().enabled = false;
    }
}
