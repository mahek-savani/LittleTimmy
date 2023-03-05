using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonPress : MonoBehaviour
{
    public SpikeTrap spikeTrap;
    public Spike spike;
    public GameObject spikeGrid;
    public GameObject spikeTrapObject;
    public GameObject buttonParent;
    public GameObject spikeResetButton;
    //backward direction
    float animDirection = -1f; 
    //forward direction
    float animDirectionFw = 1f; 
    public float movementSpeed = 15f;

    public void Start(){
        spikeTrap.originalPos = spikeGrid.transform.position;
    }

    public void Update(){
        if (spike.isTrapMoving){
            spikeGrid.transform.position += spikeTrapObject.transform.rotation * transform.forward * Time.deltaTime * movementSpeed;
        }
    }

    public void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == 3)
        {
            if (spikeTrap.trapActive)
            {
                spike.isTrapMoving = true;
                //changing spike trap's trigger button's animation direction to forward
                buttonParent.GetComponent<Animation>()["buttonAnim"].speed = animDirectionFw;
                buttonParent.GetComponent<Animation>().Play("buttonAnim");

                // spikeTrap.playAnimation();
                // spikeResetButton.transform.localScale += new Vector3(0, 0.63f, 0);
                //data.trapActiveOrder.Add("spikeTrap-0");
                data.spikeTrap.Add(0);
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
