using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardController : MonoBehaviour
{
    public static event System.Action OnGuardHasSpottedPlayer;

    public Transform pathHolder;
    public float speed = 5;
    public float waitTime = .3f;
    public float turnSpeed = 90;
    public float timeToSpotPlayer = .5f;

    [Header("Materials")]
    [SerializeField]
    public Material alertMaterial;
    public Material viewMaterial;

    [HideInInspector]
    public FieldOfView fov;

    float playerVisibleTimer;
    bool disabled;
    

    void Start() {
        fov = GetComponent<FieldOfView>();
        Vector3[] waypoints = new Vector3[pathHolder.childCount];

        // Making an array of all points on the path for guard to patrol
        for(int i = 0; i < waypoints.Length; i++){
            waypoints[i] = pathHolder.GetChild(i).position;
            waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints[i].z);
        }

        PlayerController.OnReachedEnd += DisableGuard;
        GuardAIController.OnGuardHasCaughtPlayer += DisableGuard;

        StartCoroutine(FollowPath(waypoints));
    }

    void Update() {
        if(fov.visibleTargets.Count != 0){
            playerVisibleTimer += Time.deltaTime;
        } else {
            playerVisibleTimer -= Time.deltaTime;
        }
        playerVisibleTimer = Mathf.Clamp(playerVisibleTimer, 0, timeToSpotPlayer);      
        fov.viewMeshFilter.GetComponent<MeshRenderer>().material.Lerp(viewMaterial, alertMaterial, playerVisibleTimer/timeToSpotPlayer);

        if(playerVisibleTimer >= timeToSpotPlayer){
            if(OnGuardHasSpottedPlayer != null) {
                OnGuardHasSpottedPlayer();
                DisableGuard();
            }
        }
    }

    IEnumerator FollowPath(Vector3[] waypoints){
        // Set guard object to the first waypoint
        transform.position = waypoints[0];

        int targetWaypointIndex = 1;
        Vector3 targetWaypointPos = waypoints[targetWaypointIndex];
        transform.LookAt(targetWaypointPos);

        // Indefinitely loop through the list of target waypoints and have the guard move towards each one
        // At each waypoint, it will wait for "waitTime" amount of seconds, pause a frame, and then
        // move towards the next waypoint
        while(!disabled) {
            transform.position = Vector3.MoveTowards(transform.position, targetWaypointPos, speed * Time.deltaTime);

            if(transform.position == targetWaypointPos){
                targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
                targetWaypointPos = waypoints[targetWaypointIndex];

                yield return new WaitForSeconds(waitTime);
                yield return StartCoroutine(TurnToFace(targetWaypointPos));
            }
            yield return null;
        }
    }

    IEnumerator TurnToFace(Vector3 lookTarget) {
        // Direction to look is normalized of lookTarget - current position
        Vector3 dirToLookTarget = (lookTarget - transform.position).normalized;
        float targetAngle = 90 - Mathf.Atan2(dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;

        // DeltaAngle tells us the distance/difference between our current rotation and the
        // target rotation angle. DeltaAngle can be negative.
        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f && !disabled) {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;

            yield return null;
        }
    }

    void DisableGuard() {
        disabled = true;
    }

    // Draw Gizmos for the Guard Path
    void OnDrawGizmos() {
        // Get starting position inside of pathHolder
        Vector3 startPosition = pathHolder.GetChild(0).position;
        Vector3 previousPosition = startPosition;

        // Iterate through all waypoint transforms and draw a spherical gizmo
        // as well as inter-connecting lines for editor visualization
        foreach( Transform waypoint in pathHolder) {
            Gizmos.DrawSphere(waypoint.position, .3f);
            Gizmos.DrawLine(previousPosition, waypoint.position);
            previousPosition = waypoint.position;
        }

        // Draw the final line for visualization to close loop.
        Gizmos.DrawLine(previousPosition, startPosition);
    }

    void OnDestroy() {
        PlayerController.OnReachedEnd -= DisableGuard;
        GuardAIController.OnGuardHasCaughtPlayer -= DisableGuard;
    }
}
