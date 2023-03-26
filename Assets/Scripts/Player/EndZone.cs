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
    private bool switchView = false;
    private float timeGap = 2f;

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
            StartCoroutine(TurnGreen());
            if (!switchView)
            {
                switchView = true;
                SwitchCameraView cameraView = FindObjectOfType<SwitchCameraView>();
                if (cameraView != null)
                {
                    cameraView.SetPanEndZone(true);
                }
            }
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
        }
    }

    private IEnumerator TurnGreen()
    {
        yield return new WaitForSeconds(timeGap);
        this.gameObject.GetComponent<MeshRenderer>().material = open_mat;
    }
}
