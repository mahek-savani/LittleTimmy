using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitTrap : MonoBehaviour
{
    public bool trapActive = true;
    //forward direction
    float animDirection = 1.0f; 
    public void playAnimation()
    {
        //changing spike trap's animation direction to forward
        gameObject.GetComponent<Animation>()["trapDoorAnim"].speed = animDirection;
        gameObject.GetComponent<Animation>().Play("trapDoorAnim");
    }

}
