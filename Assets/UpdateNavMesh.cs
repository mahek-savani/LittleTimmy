using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class UpdateNavMesh : MonoBehaviour
{
    public bool rebuildNavMesh = false;

    public GameObject obstacle;
    public NavMeshSurface navMesh;
    public GameObject fallTrigger;
    public GameObject door;
    public GameObject trapDoor;
    public bool resetting;
    public FallNow fallBox;
    public GameObject NPCManager;

    void Update()
    {
        Animation doorAnimation = trapDoor.GetComponent<Animation>();
        //if (rebuildNavMesh && fallBox.operated)
        //{
        if (rebuildNavMesh)
        {
            if (!resetting)
            {
                door.layer = LayerMask.NameToLayer("Default");
                navMesh.RemoveData();
                navMesh.BuildNavMesh();
                rebuildNavMesh = false;
            }
            else if (!doorAnimation.isPlaying)
            {
                door.layer = LayerMask.NameToLayer("Default");
                navMesh.RemoveData();
                navMesh.BuildNavMesh();
                rebuildNavMesh = false;
            }

        }

        //    //if (!resetting)
        //    //{

        //    //}
        //    //else if (!doorAnimation.isPlaying)
        //    //{
        //    //    door.layer = LayerMask.NameToLayer("Default");
        //    //    navMesh.RemoveData();
        //    //    navMesh.BuildNavMesh();
        //    //    rebuildNavMesh = false;
        //    //}
        //}
    }
}
