using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movementBTWaypoint : MonoBehaviour
{
    public GameObject[] waypoints;
    private int currentWaypoint = 0;
    public float speed = 1f;

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, waypoints[currentWaypoint].transform.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, waypoints[currentWaypoint].transform.position) < 0.1f)
        {
            currentWaypoint++;
            if (currentWaypoint >= waypoints.Length)
            {
                currentWaypoint = 0;
            }
        }
    }
}
