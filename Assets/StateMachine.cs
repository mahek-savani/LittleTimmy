using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateMachine : MonoBehaviour
{

    private enum STATE {IDLE, PATROLLING, CHASING};
    private STATE state = STATE.IDLE;
    public Camera cam;

    private List<Vector3> patrolPoints = new();
    private int currentDest = -1;
    public NavMeshAgent agent;
    public Transform playerPos;

    // Update is called once per frame
    void Update()
    {
        switch (state) {
            case STATE.IDLE:
                if (Input.GetMouseButtonDown(0))
                    {
                        Vector3 screenPos = Input.mousePosition;
                        Ray ray = cam.ScreenPointToRay(screenPos);

                        RaycastHit collPos;

                        int layerMask = 1 << 3;
                        layerMask = ~layerMask;

                        if (Physics.Raycast(ray, out collPos, Mathf.Infinity, layerMask))
                        {
                            patrolPoints.Add(collPos.point);
                        }
                    }
                else if (Input.GetKeyDown("return"))
                {
                    Debug.Log("Moving from IDLE to PATROLLING");
                    currentDest = 0;
                    agent.SetDestination(patrolPoints[0]);
                    state = STATE.PATROLLING;
                }
                break;
            case STATE.PATROLLING:
                // Replace with a ray cast spotting function later
                if (Vector3.Distance(transform.position, playerPos.position) < 3)
                {
                    Debug.Log("Moving from PATROLLING to CHASING");
                    state = STATE.CHASING;
                    agent.SetDestination(playerPos.position);
                }
                else if (Vector3.Distance(transform.position, patrolPoints[currentDest]) < 0.6)
                {
                    if (currentDest < patrolPoints.Count - 1)
                    {
                        currentDest++;
                    } else{
                        currentDest = 0;
                    }
                    agent.SetDestination(patrolPoints[currentDest]);
                }
                //Debug.Log(Vector3.Distance(transform.position, patrolPoints[currentDest]) );
                break;
            case STATE.CHASING:
                if (Vector3.Distance(transform.position, playerPos.position) > 3)
                {
                    Debug.Log("Moving from CHASING to PATROLLING");
                    state = STATE.PATROLLING;
                    agent.SetDestination(patrolPoints[currentDest]);
                } else
                {
                    agent.SetDestination(playerPos.position);
                }
                break;
        }
    }
}
