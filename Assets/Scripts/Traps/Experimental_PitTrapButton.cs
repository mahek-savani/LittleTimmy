using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Experimental_PitTrapButton : MonoBehaviour
{
    public PitTrap pitTrap;
    public GameObject buttonParent;
    public MeshCollider door;
    public GameObject trapDoor;
    public GameObject fallTrigger;
    public GameObject resetButton;

    public GameObject obstacle;

    public UpdateNavMesh manager;
    //public NavMeshSurface navMesh;
    //public NavMeshData currentNavMesh;

    //backward direction
    float animDirection = -1f;
    //forward direction
    float animDirectionFw = 1f;

    public void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == 3)
        {
            if (pitTrap.trapActive)
            {
                //changing pit trap's trigger button's animation direction to forward
                buttonParent.GetComponent<Animation>()["buttonAnim"].speed = animDirectionFw;
                buttonParent.GetComponent<Animation>().Play("buttonAnim");
                pitTrap.playAnimation();
                pitTrap.trapActive = false;
                door.enabled = false;
                data.trapActiveOrder.Add("pitTrap");
                //fallTrigger.SetActive(true);
                //obstacle.SetActive(true);

                //trapDoor.layer = LayerMask.NameToLayer("Ignore Raycast");

                //fallTrigger.GetComponent<FallNow>().rebuildNavMesh = true;
                //obstacle.SetActive(true);
                fallTrigger.SetActive(true);

                manager.resetting = false;
                manager.rebuildNavMesh = true;




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
