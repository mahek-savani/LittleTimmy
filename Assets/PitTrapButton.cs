using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitTrapButton : MonoBehaviour
{
    public PitTrap pitTrap;
    public GameObject buttonParent;
    public void OnTriggerEnter()
    {
        Debug.Log("Here");
        if (pitTrap.trapActive)
        {
            buttonParent.GetComponent<Animation>().Play("buttonAnim");
            pitTrap.playAnimation();
            pitTrap.trapActive = false;
            //Destroy(gameObject);
        }
        
    }
}
