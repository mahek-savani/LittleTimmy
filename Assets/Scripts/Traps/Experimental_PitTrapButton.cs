using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Experimental_PitTrapButton : MonoBehaviour
{
    public PitTrap pitTrap;
    public GameObject buttonParent;
    public MeshCollider door;
    public GameObject trapDoor;
    public GameObject fallTrigger;
    public GameObject dieTrigger;
    public GameObject resetButton;
    public GameObject smoke;

    //public NavMeshSurface navMesh;
    //public NavMeshData currentNavMesh;
    private bool switchView = false;

    //backward direction
    float animDirection = -1f;
    //forward direction
    float animDirectionFw = 1f;

    public static event System.Action ExperimentalPitButtonPushed;
    private Color startingMaterialColor;
    public LocalAudioManager localAudioManager;
    public LocalAudioManager pitlocalAudioManager;

    void Start(){
        startingMaterialColor = buttonParent.GetComponent<Renderer>().material.color;
    }

    public void Update(){
        if(pitTrap.trapActive){
            if(smoke) smoke.SetActive(false);
        }
     }
    public void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == 3)
        {
            if (pitTrap.trapActive)
            {

                // // Play Door Opening Sound
                // FindObjectOfType<AudioManager>().Play("DoorActiveSound");
                if(localAudioManager)
                {
                    // audioManager.Play(name: "ButtonPress", loop: false);
                     localAudioManager.Play(name: "ButtonPress", channel: 3, loop: false, volume: 0.3f);
     
                     
                }

                if(pitlocalAudioManager)
                { pitlocalAudioManager.Play(name: "DoorActiveSound", channel: 1, loop: false, volume: 0.3f);}


                //changing pit trap's trigger button's animation direction to forward
                buttonParent.GetComponent<Animation>()["buttonAnim"].speed = animDirectionFw;
                buttonParent.GetComponent<Animation>().Play("buttonAnim");
                pitTrap.playAnimation();
                pitTrap.trapActive = false;
                door.enabled = false;
                //data.trapActiveOrder.Add("pitTrap-0");
                data.pitTrap.Add(0);
                //fallTrigger.SetActive(true);
                //obstacle.SetActive(true);

                //trapDoor.layer = LayerMask.NameToLayer("Ignore Raycast");

                //fallTrigger.GetComponent<FallNow>().rebuildNavMesh = true;
                //obstacle.SetActive(true);
                fallTrigger.SetActive(true);
                dieTrigger.SetActive(true);
                //fallTrigger.GetComponent<NavMeshObstacle>().enabled = true;

                //manager.resetting = false;
                //manager.rebuildNavMesh = true;

                string sceneName = SceneManager.GetActiveScene().name;
                if(sceneName == "Level 1 Pit Trap Tutorial")
                {
                    if (!switchView)
                    {
                        switchView = true;
                        SwitchCameraView cameraView = FindObjectOfType<SwitchCameraView>();
                        if (cameraView != null)
                        {
                            cameraView.SetPanPitTrap(true);
                        }
                    }
                }



                if (resetButton)
                {
                    
                    resetButton.GetComponent<Animation>()["buttonAnim"].speed = animDirection;
                    resetButton.GetComponent<Animation>().Play("buttonAnim");
                    resetButton.GetComponent<Renderer>().material.color = startingMaterialColor;


                        
                }

                //navMesh.RemoveData();
                //navMesh.BuildNavMesh();

                //navMesh.UpdateNavMesh(currentNavMesh);

                //Destroy(gameObject);
                if(ExperimentalPitButtonPushed != null) 
                {

                    ExperimentalPitButtonPushed();
                }
                buttonParent.GetComponent<Renderer>().material.color = Color.grey;
                trapDoor.transform.GetComponent<Renderer>().material.color = Color.grey;
            }
        }
    }

    //IEnumerator rebuildNavMesh()
    //{
    //    yield return new WaitUntil(collisionChecked);
    //    navMesh.RemoveData();
    //    navMesh.BuildNavMesh();
    //}
}
