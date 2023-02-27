using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonPress : MonoBehaviour
{
    public SpikeTrap spikeTrap;
    public GameObject buttonParent;
    public GameObject spikeResetButton;
    //backward direction
    float animDirection = -1f; 
    //forward direction
    float animDirectionFw = 1f; 

    public void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == 3)
        {
            if (spikeTrap.trapActive)
            {
                //changing spike trap's trigger button's animation direction to forward
                buttonParent.GetComponent<Animation>()["buttonAnim"].speed = animDirectionFw;
                buttonParent.GetComponent<Animation>().Play("buttonAnim");

                spikeTrap.playAnimation();
                // spikeResetButton.transform.localScale += new Vector3(0, 0.63f, 0);
                data.trapActiveOrder.Add("spikeTrap-0");
                if (spikeResetButton)
                {
                    //changing spike trap's reset trigger button's animation direction to backward
                    spikeResetButton.GetComponent<Animation>()["buttonAnim"].speed = animDirection;
                    spikeResetButton.GetComponent<Animation>().Play("buttonAnim");
                }

                spikeTrap.trapActive = false;
                //Destroy(gameObject);
            }
        }
    }
}
