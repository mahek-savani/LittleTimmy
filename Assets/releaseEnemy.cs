using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class releaseEnemy : MonoBehaviour
{
    public GameObject gate;
    public NavMeshSurface navMesh;

    //public GameObject npcFake;
    //public GameObject npcReal;
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == 3)
        {
            gate.SetActive(false);
            navMesh.BuildNavMesh();
            //npcFake.SetActive(false);
            //npcReal.SetActive(true);
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }

    }
}
