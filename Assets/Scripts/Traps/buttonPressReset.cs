using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonPressReset : MonoBehaviour
{
    public SpikeTrap spikeTrap;
    public GameObject spikeGrid;
    public GameObject buttonParent;
    public GameObject trapButton;
    public GameObject spikeTrapWorking;
    //backward direction
    float animDirection = -1f; 
    //forward direction
    float animDirectionFw = 1f; 

    public HitBoxSize hitboxsize;

    public void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == 3)
        {
            if (!spikeTrap.trapActive)
            {
                //changing spike trap's reset trigger button's animation direction to forward
                buttonParent.GetComponent<Animation>()["buttonAnim"].speed = animDirectionFw;
                buttonParent.GetComponent<Animation>().Play("buttonAnim");

                //changing spike trap's animation direction to backward
                spikeTrapWorking.GetComponent<Animation>()["Spike Tutorial Hallway Anim"].speed = animDirection;
                spikeTrapWorking.GetComponent<Animation>().Play("Spike Tutorial Hallway Anim");

                // spikeGrid.transform.position += new Vector3(0, 0, -23.6f);
                spikeTrap.trapActive = true;

                // trapButton.transform.localScale += new Vector3(0, 0.63f, 0);
                //changing spike trap's trigger button's animation direction to backward
                trapButton.GetComponent<Animation>()["buttonAnim"].speed = animDirection;
                trapButton.GetComponent<Animation>().Play("buttonAnim");

                //BUGFIX: Reset the hitbox size to original large size after reset
                //hitboxsize.ResetHitBoxSize();

                //Destroy(gameObject);
            }
        }
    }
}
