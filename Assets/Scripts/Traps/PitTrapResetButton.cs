using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitTrapResetButton : MonoBehaviour
{
    public PitTrap pitTrap;
    public GameObject buttonParent;
    public GameObject trapButton;
    public MeshCollider door;
    public GameObject trapDoor;

    public float colorDelay = 2f;
    float colorBit = 0f;

    //backward direction
    float animDirection = -1f; 
    //forward direction
    float animDirectionFw = 1f; 

    public static event System.Action PitTrapResetButtonPushed;
    private Color startingMaterialColor;

    void Start(){
        startingMaterialColor = buttonParent.GetComponent<Renderer>().material.color;
        buttonParent.GetComponent<Renderer>().material.color = Color.grey;
    }

    void Update(){
        if(!pitTrap.trapActive){
            if(colorDelay > 0) colorDelay -= 1f * Time.deltaTime;
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
            if (!pitTrap.trapActive)
            {
                //changing pit trap's reset trigger button's animation direction to forward
                buttonParent.GetComponent<Animation>()["buttonAnim"].speed = animDirectionFw;
                buttonParent.GetComponent<Animation>().Play("buttonAnim");

                //changing pit trap's animation direction to backward
                trapDoor.GetComponent<Animation>()["trapDoorAnim"].speed = animDirection;
                trapDoor.GetComponent<Animation>().Play("trapDoorAnim");

                pitTrap.trapActive = true;
                door.enabled = true;
                // trapDoor.transform.Rotate(0f, 0f, -85f, Space.Self);
                // trapButton.transform.localScale += new Vector3(0, 0.63f, 0);

                //changing pit trap's trigger button's animation direction to backward
                trapButton.GetComponent<Animation>()["buttonAnim"].speed = animDirection;
                trapButton.GetComponent<Animation>().Play("buttonAnim");

                if(PitTrapResetButtonPushed != null) PitTrapResetButtonPushed();
                buttonParent.GetComponent<Renderer>().material.color = Color.grey;
                trapButton.GetComponent<Renderer>().material.color = startingMaterialColor;
                trapDoor.transform.GetChild(0).GetComponent<Renderer>().material.color = startingMaterialColor;                
            }
        }
    }
}
