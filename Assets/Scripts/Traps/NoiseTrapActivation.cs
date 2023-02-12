using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseTrapActivation : MonoBehaviour
{
    bool isTriggered = false;
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
        Debug.Log("Something touched the noise trap");
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            if(!isTriggered){
                data.trapActiveOrder.Add("noiseTrap");
                other.GetComponent<StateMachine_Robust>().getNoise(this.transform.position);
                isTriggered = true;
            }
            Debug.Log(this.transform.position);
        }
    }
}
