using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class ImmediateUpdate : MonoBehaviour
{

    public NavMeshSurface navMesh;
    private void OnCollisionEnter(Collision collision)
    {
        navMesh.RemoveData();
        navMesh.BuildNavMesh();
    }
}
