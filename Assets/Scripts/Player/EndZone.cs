using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndZone : MonoBehaviour
{
    public LiveCounter NPCManager;
    //public Object nextScene;

    public int nextScene;

    public Material open_mat;
    public Material close_mat;
    void LateUpdate(){
        if (NPCManager.getNumLiving() != 0){
            this.gameObject.GetComponent<MeshRenderer>().material = close_mat;
        } else {
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
    }
    void OnTriggerStay(Collider playerObject){
        Debug.Log(NPCManager.getNumLiving());
        if (playerObject.gameObject.layer == LayerMask.NameToLayer("Player")){
            if(NPCManager.getNumLiving() == 0){
                data.endTime = System.DateTime.Now;
                data.gameCompleted = true;
                Debug.Log(data.gameCompleted);
                data.levelName = SceneManager.GetActiveScene().name;
                data.checkGameCompleted(data.gameCompleted);
                SceneManager.LoadScene(nextScene);
            }
        }
        resetData();
    }
}
