using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography;
using UnityEngine;

public class Adjacency_List : MonoBehaviour
{
    public LayerMask obstacleMask;
    private Transform[] wayPoints;
    void Start()
    {
        //Debug.Log(wayPoints.Length);
        wayPoints = gameObject.GetComponentsInChildren<Transform>();

        //Debug.Log(wayPoints.Length);

        // for (int i = 0; i < wayPoints.Length; i++)
        // {
        //     Debug.Log(wayPoints[i].gameObject.name);
        // }


        for (int i = 1; i < wayPoints.Length; i++)
        {
            for (int j = i + 1; j < wayPoints.Length; j++)
            {
                if (isNeighbor(wayPoints[i].transform.position, wayPoints[j].transform.position))
                {
                    wayPoints[i].GetComponent<waypoint>().addNeighbor(wayPoints[j].gameObject);
                    wayPoints[j].GetComponent<waypoint>().addNeighbor(wayPoints[i].gameObject);
                }
            }
        }
    }

    bool isNeighbor(Vector3 wayPoint, Vector3 tentativeNeighbor)
    {
        Vector3 diffVector = tentativeNeighbor - wayPoint;
        if (!Physics.Raycast(wayPoint, diffVector, diffVector.magnitude, obstacleMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
