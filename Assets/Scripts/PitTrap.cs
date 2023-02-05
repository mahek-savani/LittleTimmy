using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitTrap : MonoBehaviour
{
    public bool trapActive = true;
    
    public void playAnimation()
    {
        gameObject.GetComponent<Animation>().Play("trapDoorAnim");
    }
}
