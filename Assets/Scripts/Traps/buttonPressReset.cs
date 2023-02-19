using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonPressReset : MonoBehaviour
{
    public SpikeTrap spikeTrap;
    public GameObject spikeGrid;
    public GameObject buttonParent;
    public GameObject trapButton;
    public void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == 3)
        {
            if (!spikeTrap.trapActive)
            {
                buttonParent.GetComponent<Animation>().Play("buttonAnim");
                spikeGrid.transform.position += new Vector3(0, 0, -23.6f);
                spikeTrap.trapActive = true;
                trapButton.transform.localScale += new Vector3(0, 0.63f, 0);
                //Destroy(gameObject);
            }
        }
    }
}
