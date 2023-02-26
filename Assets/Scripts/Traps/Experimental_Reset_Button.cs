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
    public GameObject resetButton;
    public GameObject obstacle;

    public NavMeshSurface navMesh;

    public UpdateNavMesh manager;

    //backward direction
    float animDirection = -1f;
    //forward direction
    float animDirectionFw = 1f;

    public void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == 3)
        {
            if (!pitTrap.trapActive)
            {
                //changing pit trap's trigger button's animation direction to forward
                buttonParent.GetComponent<Animation>()["buttonAnim"].speed = animDirectionFw;
                buttonParent.GetComponent<Animation>().Play("buttonAnim");
                //pitTrap.playAnimation();

                //changing pit trap's animation direction to backward
                trapDoor.GetComponent<Animation>()["trapDoorAnim"].speed = animDirection;
                trapDoor.GetComponent<Animation>().Play("trapDoorAnim");

                pitTrap.trapActive = true;
                door.enabled = true;
                data.trapActiveOrder.Add("pitTrap");
                //fallTrigger.SetActive(false);

                //trapDoor.layer = LayerMask.NameToLayer("CanTrap");

                //fallTrigger.GetComponent<FallNow>().rebuildNavMesh = true;
                

                //obstacle.SetActive(false);
                fallTrigger.SetActive(false);

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
