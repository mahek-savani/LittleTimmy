using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pitTrapTutorial : MonoBehaviour
{
    public TutorialManager tutorialManager;
    public GameObject trapSketch;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerStay(Collider c){
        if (tutorialManager && c.gameObject.layer == 3 && !tutorialManager.trapSketch)
        {
            trapSketch.SetActive(true);
        }
    }

    void OnTriggerExit(Collider c){
        if (trapSketch && c.gameObject.layer == 3)
        {
            tutorialManager.trapSketch = true;
            trapSketch.SetActive(false);
        }
    }
}
