using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndZone : MonoBehaviour
{
    public LiveCounter NPCManager;
    //public Object nextScene;
    private int ttrCount = 0;
    public int nextScene;

    public Material open_mat;
    public Material close_mat;
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
    void resetData()
    {
        data.startTime = System.DateTime.Now;
        data.endTime = System.DateTime.Now;
        data.playerDeath = "no";
        data.levelName = "demo";
        data.gameCompleted = false;
        data.trapActiveOrder = new List<string>();
        data.healthRemaining = 0;
        data.enemyHit = 0;
        data.ttrstart = System.DateTime.Now;
        data.userLevelComplete = false;
        data.attempts = 1;
        data.NPCChase = 0;
        data.NPCSuspicion = 0;
    }
    void OnTriggerStay(Collider playerObject){
        //Debug.Log(NPCManager.getNumLiving());
        if (playerObject.gameObject.layer == LayerMask.NameToLayer("Player")){
            if(NPCManager.getNumLiving() == 0){
                data.endTime = System.DateTime.Now;
                data.gameCompleted = true;
                data.userLevelComplete = true;
                //Debug.Log(data.gameCompleted);
                data.levelName = SceneManager.GetActiveScene().name;
                data.checkGameCompleted(data.gameCompleted);
                data.checkUserLevelCompleted();
                resetData();
                SceneManager.LoadScene(nextScene);
            }
        }
    }
}
