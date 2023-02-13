using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseTrapActivation : MonoBehaviour
{
    public bool isTriggered = true;
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
        Debug.Log(other.gameObject.name);
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            Debug.Log("Enemy Inside sphere");
            if(!isTriggered){
                other.GetComponent<StateMachine_Robust>().getNoise(this.transform.position);
                Debug.Log("getNoise called");
                isTriggered = true;
            }
        }
    }
}
