using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waypoint : MonoBehaviour
{
    public List<GameObject> neighbors = new List<GameObject>();

    private void Start()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    public void addNeighbor(GameObject newNeighbor)
    {
        neighbors.Add(newNeighbor);
    }
}
