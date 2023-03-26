using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitTrap : MonoBehaviour
{
    public TutorialManager tutorialManager;
    public bool trapActive = true;
    //forward direction
    float animDirection = 1.0f; 
    public GameObject trapSketch;
    
    public void playAnimation()
    {
        //changing spike trap's animation direction to forward
        gameObject.GetComponent<Animation>()["trapDoorAnim"].speed = animDirection;
        gameObject.GetComponent<Animation>().Play("trapDoorAnim");
    }

     void OnTriggerStay(Collider c){
        if (tutorialManager && c.gameObject.layer == 3 && !tutorialManager.trapSketch)
        {
            trapSketch.SetActive(true);
            tutorialManager.trapSketch = true;
        }
     }

     void OnTriggerExit(Collider c){
        if (trapSketch && c.gameObject.layer == 3)
        {
            trapSketch.SetActive(false);
        }
     }
}
