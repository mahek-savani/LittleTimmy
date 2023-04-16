using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Experimental_Reset_Button : MonoBehaviour
{
    public PitTrap pitTrap;
    public GameObject buttonParent;
    public MeshCollider door;
    public GameObject trapDoor;
    public GameObject fallTrigger;
    public GameObject dieTrigger;
    public GameObject resetButton;
    public GameObject smoke;

    public NavMeshSurface navMesh;
    public float colorDelay = 2f;
    float colorBit = 0f;

    //backward direction
    float animDirection = -1f;
    //forward direction
    float animDirectionFw = 1f;

    public static event System.Action ExperimentalResetPushed;
    private Color startingMaterialColor;

    public LocalAudioManager localAudioManager;
    public LocalAudioManager pitlocalAudioManager;
    void Start(){
        startingMaterialColor = buttonParent.GetComponent<Renderer>().material.color;
        buttonParent.GetComponent<Renderer>().material.color = Color.grey;
    }

    private void Update()
    {
        //navMesh.RemoveData();
        //navMesh.BuildNavMesh();

        if(!pitTrap.trapActive){
            if(smoke) smoke.SetActive(true);
            if(colorDelay > 0) colorDelay -= 2f * Time.deltaTime;
            else {
                colorDelay = 2;
                if(colorBit == 0)
                { 
                    buttonParent.GetComponent<Renderer>().material.color = Color.green;
                    colorBit = 1;
                }
                else
                {
                    buttonParent.GetComponent<Renderer>().material.color = startingMaterialColor;
                    colorBit = 0;
                }
            }            
        }
    }

    public void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == 3)
        {
            if (!pitTrap.trapActive)
            {

                // Play Reset Door SOund
                // FindObjectOfType<AudioManager>().Play("DoorResetSound");

                if(localAudioManager)
                {
                    // audioManager.Play(name: "ButtonPress", loop: false);
                     localAudioManager.Play(name: "ButtonPress", channel: 3, loop: false, volume: 0.2f);

                     
                }

                if(pitlocalAudioManager)
                { pitlocalAudioManager.Play(name: "DoorResetSound", channel: 1, loop: false, volume: 0.3f);}



                //changing pit trap's trigger button's animation direction to forward
                buttonParent.GetComponent<Animation>()["buttonAnim"].speed = animDirectionFw;
                buttonParent.GetComponent<Animation>().Play("buttonAnim");
                //pitTrap.playAnimation();

                //changing pit trap's animation direction to backward
                trapDoor.GetComponent<Animation>()["trapDoorAnim"].speed = animDirection;
                trapDoor.GetComponent<Animation>().Play("trapDoorAnim");

                pitTrap.trapActive = true;
                door.enabled = true;
                //data.trapActiveOrder.Add("pitTrap");
                //fallTrigger.SetActive(false);

                //trapDoor.layer = LayerMask.NameToLayer("CanTrap");

                //fallTrigger.GetComponent<FallNow>().rebuildNavMesh = true;
                

                //obstacle.SetActive(false);
                fallTrigger.SetActive(false);
                fallTrigger.GetComponent<NavMeshObstacle>().enabled = false;

                dieTrigger.SetActive(false);
                dieTrigger.GetComponent<NavMeshObstacle>().enabled = false;

                //manager.resetting = true;
                //manager.rebuildNavMesh = true;


                if (resetButton)
                {
                    resetButton.GetComponent<Animation>()["buttonAnim"].speed = animDirection;
                    resetButton.GetComponent<Animation>().Play("buttonAnim");
                }

                //navMesh.RemoveData();
                //navMesh.BuildNavMesh();

                //navMesh.UpdateNavMesh(currentNavMesh);

                //Destroy(gameObject);
                if(ExperimentalResetPushed != null) ExperimentalResetPushed();
                buttonParent.GetComponent<Renderer>().material.color = Color.grey;
                resetButton.GetComponent<Renderer>().material.color = startingMaterialColor;
                trapDoor.transform.GetChild(0).GetComponent<Renderer>().material.color = startingMaterialColor;
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
