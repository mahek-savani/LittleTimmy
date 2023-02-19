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
    float animDirection = -1f; 
    float animDirectionFw = 1f; 


    public void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == 3)
        {
            if (!pitTrap.trapActive)
            {
                buttonParent.GetComponent<Animation>()["buttonAnim"].speed = animDirectionFw;
                buttonParent.GetComponent<Animation>().Play("buttonAnim");
                trapDoor.GetComponent<Animation>()["trapDoorAnim"].speed = animDirection;
                trapDoor.GetComponent<Animation>().Play("trapDoorAnim");
                pitTrap.trapActive = true;
                door.enabled = true;
                // trapDoor.transform.Rotate(0f, 0f, -85f, Space.Self);
                // trapButton.transform.localScale += new Vector3(0, 0.63f, 0);
                trapButton.GetComponent<Animation>()["buttonAnim"].speed = animDirection;
                trapButton.GetComponent<Animation>().Play("buttonAnim");
            }
        }
    }
}
