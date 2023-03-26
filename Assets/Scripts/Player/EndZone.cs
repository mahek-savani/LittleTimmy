using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndZone : MonoBehaviour
{
    public TutorialManager tutorialManager;
    public LiveCounter NPCManager;
    //public Object nextScene;
    private int ttrCount = 0;
    public int nextScene;

    public Material open_mat;
    public Material close_mat;
    public GameObject endZoneSketch;
    void LateUpdate(){
        if (NPCManager.getNumLiving() != 0){
            this.gameObject.GetComponent<MeshRenderer>().material = close_mat;
        } else {
            if(ttrCount == 0)
            {
                //Debug.Log("hi");
                data.ttrstart = System.DateTime.Now;
                ttrCount = 1;
            }
            this.gameObject.GetComponent<MeshRenderer>().material = open_mat;
        }
    }
    
    void OnTriggerStay(Collider playerObject){
        //Debug.Log(NPCManager.getNumLiving());
        if (playerObject.gameObject.layer == LayerMask.NameToLayer("Player")){
            if(NPCManager.getNumLiving() == 0){
                data.levelName = SceneManager.GetActiveScene().name;
                data.userLevelComplete = true;
                data.checkUserLevelCompleted();
                data.checkData();                
                SceneManager.LoadScene(nextScene);
            }
            if (tutorialManager && !tutorialManager.endTrapSketch)
            {
                endZoneSketch.SetActive(true);
                tutorialManager.endTrapSketch = true;
            }
        }
        
    }
    void OnTriggerExit(Collider c){
        if (endZoneSketch && c.gameObject.layer == 3)
        {
            endZoneSketch.SetActive(false);
        }
     }
}
