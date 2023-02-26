using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
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

    void Update()
    {
        Animation doorAnimation = trapDoor.GetComponent<Animation>();
        if (rebuildNavMesh && !doorAnimation.IsPlaying("trapDoorAnim"))
        {
            obstacle.SetActive(!obstacle.activeSelf);
            fallTrigger.SetActive(!fallTrigger.activeSelf);
            door.layer = LayerMask.NameToLayer("Default");
            navMesh.RemoveData();
            navMesh.BuildNavMesh();
            rebuildNavMesh = false;
        }
    }
}
