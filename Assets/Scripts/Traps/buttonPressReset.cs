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

    public float colorDelay = 2f;
    float colorBit = 0f;
    //backward direction
    float animDirection = -1f; 
    //forward direction
    float animDirectionFw = 1f; 

    void Update(){
        if(!spikeTrap.trapActive){
            if(colorDelay > 0) colorDelay -= 1f * Time.deltaTime;
            else {
                colorDelay = 2;
                if(colorBit == 0)
                { 
                    buttonParent.GetComponent<Renderer>().material.color = Color.red;
                    colorBit = 1;
                }
                else
                {
                    buttonParent.GetComponent<Renderer>().material.color = Color.white;
                    colorBit = 0;
                }
            }            
        }
    }

    public void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == 3)
        {
            if (!spikeTrap.trapActive)
            {
                //changing spike trap's reset trigger button's animation direction to forward
                buttonParent.GetComponent<Animation>()["buttonAnim"].speed = animDirectionFw;
                buttonParent.GetComponent<Animation>().Play("buttonAnim");
                buttonParent.GetComponent<Renderer>().material.color = Color.white;

                //changing spike trap's animation direction to backward
                spikeTrapWorking.GetComponent<Animation>()["Spike Tutorial Hallway Anim"].speed = animDirection;
                spikeTrapWorking.GetComponent<Animation>().Play("Spike Tutorial Hallway Anim");

                // spikeGrid.transform.position += new Vector3(0, 0, -23.6f);
                spikeTrap.trapActive = true;

                // trapButton.transform.localScale += new Vector3(0, 0.63f, 0);
                //changing spike trap's trigger button's animation direction to backward
                trapButton.GetComponent<Animation>()["buttonAnim"].speed = animDirection;
                trapButton.GetComponent<Animation>().Play("buttonAnim");

                //Destroy(gameObject);
            }
        }
    }
}
