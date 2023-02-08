using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndZone : MonoBehaviour
{
    public LiveCounter NPCManager;
    public Object nextScene;

    public Material open_mat;
    public Material close_mat;

    void LateUpdate(){
        if(NPCManager.getNumLiving() != 0){
            this.gameObject.GetComponent<MeshRenderer>().material = close_mat;
        } else {
            this.gameObject.GetComponent<MeshRenderer>().material = open_mat;
        }
    }

    void OnTriggerEnter(Collider playerObject){
        if(playerObject.gameObject.layer == LayerMask.NameToLayer("Player")){
            if(NPCManager.getNumLiving() == 0){
                SceneManager.LoadScene(nextScene.name);
            }
        }
    }
}
