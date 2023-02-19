using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonPress : MonoBehaviour
{
    public SpikeTrap spikeTrap;
    public GameObject buttonParent;
    public GameObject spikeResetButton;
    float animDirection = -1f; 
    float animDirectionFw = 1f; 

    public void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == 3)
        {
            if (spikeTrap.trapActive)
            {
                buttonParent.GetComponent<Animation>()["buttonAnim"].speed = animDirectionFw;
                buttonParent.GetComponent<Animation>().Play("buttonAnim");
                spikeTrap.playAnimation();
                // spikeResetButton.transform.localScale += new Vector3(0, 0.63f, 0);
                spikeResetButton.GetComponent<Animation>()["buttonAnim"].speed = animDirection;
                spikeResetButton.GetComponent<Animation>().Play("buttonAnim");
                spikeTrap.trapActive = false;
                //Destroy(gameObject);
            }
        }
    }
}
