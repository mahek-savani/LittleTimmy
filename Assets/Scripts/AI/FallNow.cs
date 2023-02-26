using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class FallNow : MonoBehaviour
{
    public NavMeshSurface navMesh;
    public NavMeshData currentNavMesh;
    public PitTrap trap;
    //private bool lastTrapState = true;
    public GameObject obstacle;
    public bool operated = false;
    public UpdateNavMesh manager;
    //public GameObject offLink;

    private void OnEnable()
    {
        operated = false;
    }

    private void OnTriggerStay(Collider c)
    {
        if (!(manager.rebuildNavMesh) && c.gameObject.layer == LayerMask.NameToLayer("Enemies") && c.gameObject.GetComponent<StateMachine_Robust>().enabled)
        {
            StateMachine_Robust SM = c.gameObject.GetComponent<StateMachine_Robust>();



            //SM.die();

            //SM.die();
            //SM.agent.velocity = new Vector3(0f, -9f, 0f);
            c.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            SM.die();
            SM.enabled = false;
            SM.agent.enabled = false;

            operated = true;

            //if (lastTrapState != trap.trapActive)
            //{
            //    lastTrapState = trap.trapActive;
            //}

            
           
            //SM.enabled = false;

            //SM.agent.isStopped = false;
            //SM.agent.SetDestination(fallPoint.position);
            //SM.agent.destination = fallPoint.position;
        }
    }

    //private void LateUpdate()
    //{
    //    if (rebuildNavMesh){
    //        obstacle.SetActive(!obstacle.activeSelf);
    //        navMesh.RemoveData();
    //        navMesh.BuildNavMesh();
    //        //offLink.SetActive(!offlink.activeSelf);
    //        rebuildNavMesh = false;
    //    }
    //}

    //public checkCollision()
    //{
    //    gameObject.GetComponent<BoxCollider>();
    //}
}
