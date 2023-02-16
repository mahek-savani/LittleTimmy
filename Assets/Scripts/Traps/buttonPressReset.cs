using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonPressReset : MonoBehaviour
{
    public SpikeTrap spikeTrap;
    public GameObject buttonParent;
    public void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == 3)
        {
            if (!spikeTrap.trapActive)
            {
                buttonParent.GetComponent<Animation>().Play("buttonResetAnim");
                spikeTrap.playAnimation();
                spikeTrap.trapActive = true;
                //Destroy(gameObject);
            }
        }
    }
}
