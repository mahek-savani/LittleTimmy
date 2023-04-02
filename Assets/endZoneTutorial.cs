using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endZoneTutorial : MonoBehaviour
{
    public TutorialManager tutorialManager;
    public GameObject endZoneSketch;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerStay(Collider playerObject){
        //Debug.Log(NPCManager.getNumLiving());
        if (playerObject.gameObject.layer == LayerMask.NameToLayer("Player")){
            if (tutorialManager && !tutorialManager.endTrapSketch)
            {
                endZoneSketch.SetActive(true);
            }
        }
        
    }
    void OnTriggerExit(Collider c){
        if (endZoneSketch && c.gameObject.layer == 3)
        {
            tutorialManager.endTrapSketch = true;
            endZoneSketch.SetActive(false);
        }
     }
}
