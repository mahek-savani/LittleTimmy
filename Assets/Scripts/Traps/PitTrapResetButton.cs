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
    //backward direction
    float animDirection = -1f; 
    //forward direction
    float animDirectionFw = 1f; 


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
            }
        }
    }
}
