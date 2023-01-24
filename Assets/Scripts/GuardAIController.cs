using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardAIController : GuardController
{
    public static event System.Action OnGuardHasCaughtPlayer;
    public NavMeshAgent navMeshAgent;

    float currentWaitTime;
    float currentRotateTime;
    
    Vector3 lastKnownPlayerPos = Vector3.zero;
    Vector3 playerCurrentPos = Vector3.zero;

    int targetWaypointIndex = 0;
    Vector3[] aiWaypoints;

    bool playerInRange;
    bool playerNear;
    bool isPatrol;
    bool caughtPlayer;

    // Start is called before the first frame update
    void Start()
    {
        PlayerController.OnReachedEnd += Stop;
        GuardController.OnGuardHasSpottedPlayer += Stop;

        fov = GetComponent<FieldOfView>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        aiWaypoints = new Vector3[pathHolder.childCount];

        // Making an array of all points on the path for guard to patrol
        for(int i = 0; i < aiWaypoints.Length; i++){
            aiWaypoints[i] = pathHolder.GetChild(i).position;
            aiWaypoints[i] = new Vector3(aiWaypoints[i].x, transform.position.y, aiWaypoints[i].z);
        }

        playerInRange = false;
        playerNear = false;
        isPatrol = true;
        caughtPlayer = false;
        currentWaitTime = waitTime;
        currentRotateTime = turnSpeed;

        navMeshAgent.SetDestination(aiWaypoints[targetWaypointIndex]);
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerNear();

        if(!isPatrol){
            fov.viewMeshFilter.GetComponent<MeshRenderer>().material = alertMaterial;
            GuardIsChasing();
        } else {
            fov.viewMeshFilter.GetComponent<MeshRenderer>().material = viewMaterial;
            GuardIsPatrolling();
        }
    }

    void Move(float newSpeed){
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = newSpeed;
    }

    void Stop(){
        navMeshAgent.isStopped = true;
        navMeshAgent.velocity = Vector3.zero;
        navMeshAgent.speed = 0;
    }

    void UpdateNextPoint(){
        targetWaypointIndex = (targetWaypointIndex + 1) % aiWaypoints.Length;
        navMeshAgent.SetDestination(aiWaypoints[targetWaypointIndex]);
    }

    void PlayerIsCaught() {
        caughtPlayer = true;
        if(OnGuardHasCaughtPlayer != null){
            OnGuardHasCaughtPlayer();
        }
        Stop();
    }

    void LookForPlayer(Vector3 player) {
        navMeshAgent.SetDestination(player);
        if(Vector3.Distance(transform.position, player) <= 0.3) {
            if(currentWaitTime <= 0){
                playerNear = false;
                Move(speed);
                navMeshAgent.SetDestination(aiWaypoints[targetWaypointIndex]);
                currentWaitTime = waitTime;
                currentRotateTime = turnSpeed;
            } else {
                Stop();
                waitTime -= Time.deltaTime;
            }
        }
    }

    void CheckPlayerNear(){
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, fov.viewRadius, fov.targetMask);

        for(int i = 0; i < targetsInViewRadius.Length; i++){
            Transform player = targetsInViewRadius[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            
            if(Vector3.Angle(transform.forward, dirToPlayer) < fov.viewAngle/2){
                float dstToPlayer = Vector3.Distance(transform.position, player.position);

                if(!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, fov.obstacleMask)){
                    playerInRange = true;
                    isPatrol = false;
                } else {
                    playerInRange = false;
                }
            }

            if(Vector3.Distance(transform.position, player.position) > fov.viewRadius){
                playerInRange = false;
            }

            if(playerInRange){
                playerCurrentPos = player.transform.position;
            }
        }
    }

    void GuardIsPatrolling(){
        if(playerNear){
            if(currentRotateTime <= 0){
                Move(speed);
                LookForPlayer(lastKnownPlayerPos);
            } else {
                Stop();
                currentRotateTime -= Time.deltaTime;
            }
        } else {
            playerNear = false;
            lastKnownPlayerPos = Vector3.zero;
            navMeshAgent.SetDestination(aiWaypoints[targetWaypointIndex]);

            if(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance){
                if(currentWaitTime <= 0){
                    UpdateNextPoint();
                    Move(speed);
                    currentWaitTime = waitTime;
                } else {
                    Stop();
                    currentWaitTime -= Time.deltaTime;
                }
            }
        }
    }

    void GuardIsChasing() {
        playerNear = false;
        lastKnownPlayerPos = Vector3.zero;

        if(!caughtPlayer) {
            Move(speed + 1);
            navMeshAgent.SetDestination(playerCurrentPos);
        }

        if(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) {

            if(currentWaitTime <= 0 && !caughtPlayer && 
            Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 6f) {
                isPatrol = true;
                playerNear = false;
                Move(speed);
                currentRotateTime = turnSpeed;
                currentWaitTime = waitTime;
                navMeshAgent.SetDestination(aiWaypoints[targetWaypointIndex]);
            } else {
                if(Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 2.5f) {
                    Stop();
                    currentWaitTime -= Time.deltaTime;
                } 
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerIsCaught();
        }
    }

    void OnDestroy() {
        PlayerController.OnReachedEnd -= Stop;
        GuardController.OnGuardHasSpottedPlayer -= Stop;
    }
    
}
