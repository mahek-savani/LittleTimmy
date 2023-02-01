using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonPress : MonoBehaviour
{
    public SpikeTrap spikeTrap;
    public GameObject buttonParent;
    public void OnTriggerEnter()
    {
        Debug.Log("Here");
        if (spikeTrap.trapActive)
        {
            buttonParent.GetComponent<Animation>().Play("buttonAnim");
            spikeTrap.playAnimation();
            spikeTrap.trapActive = false;
            //Destroy(gameObject);
        }
        
    }
}
