using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class PitTrapButton : MonoBehaviour
{
    public PitTrap pitTrap;
    public GameObject buttonParent;
    public MeshCollider door;
    public GameObject trapDoor;
    public GameObject fallTrigger;
    public NavMeshSurface navMesh;
    public NavMeshData currentNavMesh;

    public void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == 3)
        {
            if (pitTrap.trapActive)
            {
                buttonParent.GetComponent<Animation>().Play("buttonAnim");
                pitTrap.playAnimation();
                pitTrap.trapActive = false;
                door.enabled = false;
                data.trapActiveOrder.Add("pitTrap");
                fallTrigger.SetActive(true);

                trapDoor.layer = LayerMask.NameToLayer("Ignore Raycast");

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
