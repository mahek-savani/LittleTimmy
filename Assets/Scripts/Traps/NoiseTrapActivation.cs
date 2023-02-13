using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseTrapActivation : MonoBehaviour
{
    public bool isTriggered;
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
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            if(!isTriggered){
                other.GetComponent<StateMachine_Robust>().getNoise(this.transform.position);
                Debug.Log("getNoise called");
            }
        }
    }
}
