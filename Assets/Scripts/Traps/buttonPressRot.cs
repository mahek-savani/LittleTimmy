using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonPressRot : MonoBehaviour
{
    public SpikeTrapRot spikeTrapRot;
    public GameObject buttonParent;

    public void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == 3)
        {
            if (spikeTrapRot.trapActive)
            {
                buttonParent.GetComponent<Animation>().Play("buttonAnim");

                spikeTrapRot.playAnimation();
                spikeTrapRot.trapActive = false;
            }
        }
    }
}
