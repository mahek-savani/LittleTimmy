using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateMachine_Robust : MonoBehaviour
{
/*     [Header("View Visualization")]
    [SerializeField]
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;
    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();

    [Header("Visualization Smoothing")]
    [SerializeField]
    public float meshResolution;
    public int edgeResolveIter;
    public float edgeDstThreshold;
    public MeshFilter viewMeshFilter;
    Mesh viewMesh; */


    public enum STATE {IDLE, PATROLLING, SUSPICIOUS, CHASING, PARANOID, NOISE};
    public STATE state = STATE.IDLE;
    public STATE defaultState = STATE.PATROLLING;
    public int PLAYER_LAYER = 3;
    public MeshRenderer myMesh;
    public PlayerDamage playerDamage;
    public bool alive = true;


    private int currentDest = 0;
    public NavMeshAgent agent;
    public Transform playerPos;
    public Transform[] newPatrolPoints;
    public Transform graph;
    public float speedVar = 4;


    public LiveCounter NPCManager;
    private Vector3 noiseSource;
    private Dictionary<Transform, int> paranoidPoints = new Dictionary<Transform, int>();
    public float coolTime; // Specifies the amount of time (in seconds) for an NPC to stop chasing the player


    public FieldOfView fov;
    public MeshRenderer FOVMesh;
    public Material alertFOV;
    public Material passiveFOV;
    
    
    //float pointDist = 0.5f;
    float suspiciousTime;
    float timeCounter = 0f; 
    float playerVisibleTimer = 0.0f;
    float waitTime = 0.0f;
    private float timeToSuspicion;
    private float timeToChase;
    bool conscious = true;
    private Transform currentPosition;

    void Start() {
        /*         viewMesh = new Mesh();
                viewMesh.name = "View Mesh";
                viewMeshFilter.mesh = viewMesh; */
        timeToSuspicion = 1f;
        timeToChase = 2f;
        suspiciousTime = 5f;
        coolTime = 2f;

        agent.speed = speedVar;
        getDefault();

        //StartCoroutine(die());
    }

    // Update is called once per frame
    void Update()
    {
        if (conscious)
        {
            if (fov.visibleTargets.Count != 0)
            {
                playerVisibleTimer += Time.deltaTime;
            }
            else
            {
                playerVisibleTimer -= Time.deltaTime;
            }

            //Debug.Log(playerVisibleTimer);

            fov.viewMeshFilter.GetComponent<MeshRenderer>().material.Lerp(passiveFOV, alertFOV, playerVisibleTimer / timeToChase);
        }

        if (Input.GetKeyDown("backspace"))
        {
            if (gameObject.name == "NPC 1")
            {
                die();
            }
        } 
        //DrawFieldOfView();
        switch (state) {

            case STATE.IDLE:
                if ( playerVisibleTimer >= timeToSuspicion)
                {
                    getNoise(playerPos.position);
                }

                playerVisibleTimer = Mathf.Clamp(playerVisibleTimer, 0, timeToChase);
                if (waitTime > 0)
                {
                    waitTime -= Time.deltaTime;
                }
                else
                {
                    if (!conscious)
                    {
                        conscious = true;
                        FOVMesh.enabled = true;
                        getSuspicious(transform.position);
                    }
                    else
                    {
                        getDefault();
                    }
                }
                break;

            case STATE.PATROLLING:
                if ( playerVisibleTimer >= timeToSuspicion)
                {
                    getNoise(playerPos.position);
                }

                playerVisibleTimer = Mathf.Clamp(playerVisibleTimer, 0, timeToChase);
                // Replace with a ray cast spotting function later
                //Debug.Log(Vector3.Distance(transform.position, patrolPoints[currentDest]));
/*                 if (Vector3.Distance(transform.position, playerPos.position) < 5)
                {
                    Debug.Log("Moving from PATROLLING to CHASING");
                    state = STATE.CHASING;
                    agent.SetDestination(playerPos.position);
                } */

                // Assign new waypoint if current one has been reached
                if (Vector3.Distance(transform.position, newPatrolPoints[currentDest].position) < 0.5)
                {
                    //Debug.Log(newPatrolPoints[currentDest].position);
                    if (currentDest < newPatrolPoints.Length - 1)
                    {
                        currentDest++;
                    } else{
                        currentDest = 0;
                    }
                    agent.SetDestination(newPatrolPoints[currentDest].position);
                }
                //Debug.Log(Vector3.Distance(transform.position, patrolPoints[currentDest]) );
                break;
                
            case STATE.CHASING:
                playerVisibleTimer = Mathf.Clamp(playerVisibleTimer, timeToChase, timeToChase);
            
                agent.SetDestination(playerPos.position);
                if (fov.visibleTargets.Count != 0)
                {
                    coolTime = 2f;
                }
                else if (fov.visibleTargets.Count == 0)
                {
                    coolTime -= Time.deltaTime;

                    if (coolTime <= Mathf.Epsilon)
                    {
                        getNoise(playerPos.position);
                    }
                    //agent.SetDestination(patrolPoints[currentDest]);
                }


                break;

            case STATE.SUSPICIOUS:
                if (playerVisibleTimer >= timeToChase)
                {
                    getChase();
                }

                playerVisibleTimer = Mathf.Clamp(playerVisibleTimer, timeToSuspicion, timeToChase);
            
                timeCounter -= Time.deltaTime;

                if (timeCounter < Mathf.Epsilon)
                {
                    getDefault();
                } else if (Vector3.Distance(transform.position, currentPosition.position) < 0.5)
                {
                    //waypoint currentPoint = graph.GetChild(currentDest).GetComponent<waypoint>();

                    waypoint currentPoint = currentPosition.GetComponent<waypoint>();

                    currentPosition = getRandomNeighbor(currentPoint);
                    agent.SetDestination(currentPosition.position);
                }

                break;

            // Investigate a noise or disturbance of some sort
            case STATE.NOISE:
                if (playerVisibleTimer >= timeToChase)
                {
                    getChase();
                }

                playerVisibleTimer = Mathf.Clamp(playerVisibleTimer, timeToSuspicion, timeToChase);

                if (fov.visibleTargets.Count != 0)
                {
                    agent.SetDestination(playerPos.position);
                    transform.LookAt(playerPos.position, transform.up);
                }
                else
                {
                    getSuspicious(transform.position);
                }
                break;

            case STATE.PARANOID:
                if ( playerVisibleTimer - timeToSuspicion >= Mathf.Epsilon)
                {
                    getNoise(playerPos.position);
                }

                playerVisibleTimer = Mathf.Clamp(playerVisibleTimer, 0, timeToChase);

                if (Vector3.Distance(transform.position, currentPosition.position) < 0.5)
                {
                    waypoint currentPoint = currentPosition.GetComponent<waypoint>();

                    currentPosition = getRandomNeighbor(currentPoint);
                    agent.SetDestination(currentPosition.position);
                }

                break;
        }
    }

/*     IEnumerator idleAction (int numSeconds = -1)
    {  
        if (numSeconds >= 0) 
        {
            yield return new WaitForSeconds(numSeconds);
        } else
        {
            yield return new WaitForSeconds(0);
        }
    } */

    void getDefault()
    {
        switch (defaultState)
        {
            case STATE.PATROLLING:
                getPatrol();
                break;
            case STATE.PARANOID:
                getParanoid();
                break;
            case STATE.IDLE:
                getIdle();
                break;
            case STATE.NOISE:
                getNoise(new Vector3(0, 0, 0));
                break;
            case STATE.SUSPICIOUS:
                getSuspicious(transform.position);
                break;
            case STATE.CHASING:
                getChase();
                break;
        }
    }

    void getChase()
    {
        agent.isStopped = false;
        myMesh.material.color = Color.red;
        state = STATE.CHASING;
    }

    void getIdle(float time)
    {
        myMesh.material.color = Color.blue;
        agent.isStopped = true;
        waitTime = time;
        state = STATE.IDLE;
    }

    void getIdle()
    {
        myMesh.material.color = Color.blue;
        agent.isStopped = true;
        waitTime = Mathf.Infinity;
        state = STATE.IDLE;
    }

    public void getNoise(Vector3 source)
    {
        agent.isStopped = false;
        myMesh.material.color = Color.yellow;
        noiseSource = source;
        state = STATE.NOISE;
        agent.SetDestination(source);
    }

    // The NPC should investigate the area around themself when they get suspicious
    void getSuspicious(Vector3 source)
    {
        agent.isStopped = false;
        myMesh.material.color = Color.yellow;
        assignSuspiciousWalk(source);

        returnToSuspicion();

        state = STATE.SUSPICIOUS;

        timeCounter = suspiciousTime;
    }

    public void getParanoid()
    {
        agent.isStopped = false;
        myMesh.material.color = new Color(252/255f, 139/255f, 0f);
        assignParanoidWalk();

        returnToSuspicion();

        timeToSuspicion /= 2;
        timeToChase /= 2;
        playerVisibleTimer /= 2;

        state = STATE.PARANOID;
    }

    void getPatrol()
    {
        agent.isStopped = false;
        returnToPatrol();
        myMesh.material.color = Color.cyan;

        state = STATE.PATROLLING;
    }


    public void dieIdle()
    {
        StartCoroutine("dieAsynchronous");
    }
    public void die()
    {
        alive = false;
        conscious = false;
        state = STATE.IDLE;
        waitTime = 10f;
        agent.isStopped = true;
        fov.viewMeshFilter.mesh.Clear();
        NPCManager.decrement();
        myMesh.material.color = Color.black;
        //Destroy(fov.gameObject);
        StartCoroutine("dieAsynchronous");
    }

    IEnumerator dieAsynchronous()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
        data.enemyRemaining = data.enemyRemaining - 1;
        Debug.Log(data.enemyRemaining);
        if (data.enemyRemaining == 0)
        {
            data.endTime = System.DateTime.Now;
            data.gameCompleted = true;
        }
        Debug.Log(data.gameCompleted);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == PLAYER_LAYER && conscious && alive)
        {
            conscious = false;
            FOVMesh.enabled = false;
            playerVisibleTimer = 0;
            getIdle(3.0f);
            myMesh.material.color = new Color(145 / 255f, 145 / 255f, 145 / 255f);
            playerDamage.TakeDamage(1);
        }
    }

    // Maps paranoid points according to the source
    // Waypoints closer to the source get a larger value, incentivizing movement near it
    void assignSuspiciousWalk(Vector3 source)
    {
        for (int i = 0; i < graph.childCount; i++)
        {
            Transform wayPoint = graph.GetChild(i);
            float dist = Mathf.Pow(Vector3.Distance(source, wayPoint.position), 2.0f);
            int roundedDist = Mathf.RoundToInt(dist);
            paranoidPoints[wayPoint] = roundedDist;
        }
    }

    void assignParanoidWalk()
    {
        for (int i = 0; i < graph.childCount; i++)
        {
            Transform wayPoint = graph.GetChild(i);
            paranoidPoints[wayPoint] = Random.Range(1, 6);
        }
    }

    // Given a waypoint, returns a random neighbor according to paranoidPoints
    Transform getRandomNeighbor(waypoint wayPoint)
    {
        Transform bestPoint = transform;
        int largestWeight = -1;

        for (int i = 0; i < wayPoint.neighbors.Length; i++)
        {
            Transform currentPoint = wayPoint.neighbors[i].transform;
            int diceRoll = Random.Range(1, 6);

            int weight = paranoidPoints[currentPoint] + diceRoll;
            if (weight > largestWeight)
            {
                largestWeight = weight;
                bestPoint = currentPoint;
            }
        }

        return bestPoint;
    }

    // Return to the nearest waypoint on a patrol
    void returnToPatrol()
    {
        Vector3 nearestPoint = new Vector3();
        float closestDist = Mathf.Infinity;
        int nearestIndex = -1;

        for (int i = 0; i < newPatrolPoints.Length; i++)
        {
            Vector3 wayPoint = newPatrolPoints[i].position;
            float currentDist = Vector3.Distance(transform.position, wayPoint);

            if (currentDist < closestDist)
            {
                closestDist = currentDist;
                nearestPoint = wayPoint;
                nearestIndex = i;
            }
        }

        currentDest = nearestIndex;
        agent.SetDestination(nearestPoint);
    }


    // Go to the nearest waypoint from your location
    void returnToSuspicion()
    {
        Transform nearestPoint = transform;
        float closestDist = Mathf.Infinity;

        for (int i = 0; i < graph.childCount; i++)
        {
            Transform wayPoint = graph.GetChild(i);
            float currentDist = Vector3.Distance(transform.position, wayPoint.position);

            if (currentDist < closestDist)
            {
                closestDist = currentDist;
                nearestPoint = wayPoint;
            }
        }

        currentPosition = nearestPoint;
        agent.SetDestination(nearestPoint.position);
    }

    public void MakeIncapacitated(float time){
        conscious = false;
        FOVMesh.enabled = false;
        getIdle(time);
        myMesh.material.color = new Color(145 / 255f, 145 / 255f, 145 / 255f);
    }
}
/* 
    IEnumerator FindTargetsWithDelay(float delay) {
        while (true) {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    

    void LateUpdate() {
        DrawFieldOfView();
    }

    void FindVisibleTargets() {
        // visibleTargets List is our list of all transforms
        // This list only updated using the targetMask (set layer to Target)
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        // targetsInViewRadius uses a physics object to get all current objects that are
        // within our radius
        for (int i = 0; i < targetsInViewRadius.Length; i++){
            // Get the normalized direction to each target we can currently "see"
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            // Check to make sure that the angle from "front" to the target is within
            // our viewing angle
            if(Vector3.Angle(transform.forward, dirToTarget) < viewAngle/2){
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                // Only add to the visibleTargets list if we don't encounter any
                if(!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask)) {
                    visibleTargets.Add(target);
                }
            }
        }
    }

    void DrawFieldOfView() {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();

        ViewCastInfo oldViewCast = new ViewCastInfo();

        for ( int i = 0; i <= stepCount; i++) {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast (angle);

            if (i > 0) {
                bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.dst - newViewCast.dst) > edgeDstThreshold;
                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDstThresholdExceeded)){
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if(edge.pointA != Vector3.zero) {
                        viewPoints.Add(edge.pointA);
                    }

                    if(edge.pointB != Vector3.zero) {
                        viewPoints.Add(edge.pointB);
                    }
                }
            }

            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount-2) * 3];

        vertices[0] = Vector3.zero;
        for(int i = 0; i < vertexCount-1; i++){
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if(i < vertexCount - 2){
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast){
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for(int i = 0; i < edgeResolveIter; i++){
            float angle = (minAngle + maxAngle) /2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > edgeDstThreshold;
            if(newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded) {
                minAngle = angle;
                minPoint = newViewCast.point;
            } else {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }

    ViewCastInfo ViewCast(float globalAngle) {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;

        if(Physics.Raycast(transform.position, dir, out hit, viewRadius, obstacleMask)) {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        } else {
            return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal) {
        if (!angleIsGlobal) {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public struct ViewCastInfo {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle) {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
        }
    }

    public struct EdgeInfo {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 _pointA, Vector3 _pointB) {
            pointA = _pointA;
            pointB = _pointB;
        }
    }
}
 */