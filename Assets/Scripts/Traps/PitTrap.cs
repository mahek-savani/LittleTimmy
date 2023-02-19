using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitTrap : MonoBehaviour
{
    public bool trapActive = true;
    float animDirection = 1.0f; 
    
    public void playAnimation()
    {
        gameObject.GetComponent<Animation>()["trapDoorAnim"].speed = animDirection;
        gameObject.GetComponent<Animation>().Play("trapDoorAnim");
    }
}
