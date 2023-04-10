using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonPressReset : MonoBehaviour
{
    public SpikeTrap spikeTrap;
    public Spike spike;
    public GameObject spikeGrid;
    public GameObject buttonParent;
    public GameObject trapButton;
    public GameObject spikeTrapWorking;
    public GameObject smoke;

    public float colorDelay = 2f;
    float colorBit = 0f;
    //backward direction
    float animDirection = -1f; 
    //forward direction
    float animDirectionFw = 1f; 
    //public float movementSpeed = -30f;

    public static event System.Action SpikeTrapResetPushed;
    private Color startingMaterialColor;

    void Start(){
        startingMaterialColor = buttonParent.GetComponent<Renderer>().material.color;
        buttonParent.GetComponent<Renderer>().material.color = Color.grey;
    }

    void Update(){
        if(!spikeTrap.trapActive){
            if (smoke) smoke.SetActive(true);
            if(colorDelay > 0) colorDelay -= 2f * Time.deltaTime;
            else {
                colorDelay = 2;
                if(colorBit == 0)
                { 
                    buttonParent.GetComponent<Renderer>().material.color = Color.green;
                    colorBit = 1;
                }
                else
                {
                    buttonParent.GetComponent<Renderer>().material.color = startingMaterialColor;
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
                //spike.isTrapMovingBack = true;

                spikeGrid.transform.position = spikeTrap.originalPos;
                //changing spike trap's reset trigger button's animation direction to forward
                buttonParent.GetComponent<Animation>()["buttonAnim"].speed = animDirectionFw;
                buttonParent.GetComponent<Animation>().Play("buttonAnim");

                //changing spike trap's animation direction to backward
                // spikeTrapWorking.GetComponent<Animation>()["Spike Tutorial Hallway Anim"].speed = animDirection;
                // spikeTrapWorking.GetComponent<Animation>().Play("Spike Tutorial Hallway Anim");
                // spikeGrid.transform.position += new Vector3(0, 0, -23.6f);
                spikeTrap.trapActive = true;
                spike.isTrapMoving = false;
                // trapButton.transform.localScale += new Vector3(0, 0.63f, 0);
                //changing spike trap's trigger button's animation direction to backward
                trapButton.GetComponent<Animation>()["buttonAnim"].speed = animDirection;
                trapButton.GetComponent<Animation>().Play("buttonAnim");

                //BUGFIX: Reset the hitbox size to original large size after reset
                //hitboxsize.ResetHitBoxSize();

                //Destroy(gameObject);
                if(SpikeTrapResetPushed != null) SpikeTrapResetPushed();
                buttonParent.GetComponent<Renderer>().material.color = Color.grey;
                trapButton.GetComponent<Renderer>().material.color = startingMaterialColor;
                spikeTrapWorking.transform.GetChild(0).GetComponent<Renderer>().material.color = startingMaterialColor;  
            }
        }
    }
}
