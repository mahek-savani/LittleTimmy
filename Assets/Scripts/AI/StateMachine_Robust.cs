using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using static Unity.VisualScripting.Member;

public class StateMachine_Robust : MonoBehaviour
{
    // Enumerated type for possible states
    public enum STATE {IDLE, PATROLLING, SUSPICIOUS, CHASING, PARANOID, NOISE, UNCONSCIOUS};
    
    // Specifies a cardinal direction to look in for the idle state
    public enum DIRECTION { NORTH, SOUTH, EAST, WEST, HARDCODE};



    [Header("State Settings")]

    // Current state of the machine
    public STATE state = STATE.IDLE;
    
    // The machine will eventually revert back to this state by default
    public STATE defaultState = STATE.PATROLLING;
    
    // Determines whether this agent is alive or dying
    public bool alive = true;

    // A boolean determining whether the NPC is currently conscious
    public bool conscious = true;

    // Determines whether the enemy will become suspicious after getting hit-stunned
    public bool passive = false;

    // The default position for an idle NPC
    public waypoint defaultIdlePos;

    // The default direction to face for an idle NPC
    public DIRECTION defaultIdleDir = DIRECTION.NORTH;

    // The current idle position for the NPC
    private Vector3 idlePos;

    // The current idle direction for the NPC
    private Vector3 idleDir;



    [Header("Transition Settings")]

    // The time it takes for the NPC to get suspicious of a player in their FOV
    public float timeToSuspicion = 1f;

    // The amount of time an NPC stays suspicious before returning to passivity
    public float suspiciousTime = 5f;

    // The time it takes for the NPC to become hostile and chase a player in their FOV
    public float timeToChase = 2f;

    // Specifies the amount of time for the NPC to stop chasing the player once they're out of the FOV
    public float coolTime = 2f;

    // Specifies how long the NPC takes to wake up after being rendered unconscious
    public float unconsciousTime = 3f;

    // Denotes the state the NPC transitions to out of unconsciousness
    public STATE idleState = STATE.SUSPICIOUS;

    // A variable used to store the countdown from suspicious to passivity, or chase to suspicion
    private float timeCounter = 0f; 

    // Stores the countdown from idle or unconscious to another state
    private float waitTime = 0.0f;



    [Header("NPC Manager and Other Agents")]

    // The mesh of the NPC agent
    public MeshRenderer myMesh;

    // The rigidbody of the agent
    public Rigidbody myBody;

    // The NPC manager parenting this agent
    public LiveCounter NPCManager;

    // How long the NPC will wait before moving onto a non-colliding waypoint
    private float collisionTime;

    // The amount of time the NPC has been colliding with another
    private float collisionSoFar = 0f;



    [Header("FOV Visualization")]

    // The field of view script
    public FieldOfView fov;

    // The mesh renderer for visualizing the FOV
    public MeshRenderer FOVMesh;

    // The red alert material for the FOV
    public Material alertFOV;

    // The white passive material for the FOV
    public Material passiveFOV;



    [Header("Navigation and AI")]

    // The AI agent controller
    public NavMeshAgent agent;
    
    // The waypoint graph
    public Transform graph;

    // The speed for chases
    public float chaseSpeed = 12;

    // Speed for suspicious and noise states
    public float susSpeed = 5;

    // Speed for patrolling
    public float patrolSpeed = 5;
    
    // An array of the waypoint (transforms) this agent will follow while on patrol
    public Transform[] patrolPoints;

    // Specifies the index of the waypoint currently being traveled to while patrolling
    private int currentDest = 0;

    // Stores the waypoints and corresponding weights for paranoid mode
    private Dictionary<Transform, int> paranoidPoints = new Dictionary<Transform, int>();

    // Stores the current waypoint transform in suspicious / paranoid states
    private Transform currentPosition;

    // The point being investigated when in noise mode
    private Vector3 noiseSource;

    // Renders a line to a target the NPC is moving to
    public LineRenderer targetLine;



    [Header("Player Interaction")]

    // The interface for damaging the player
    public PlayerDamage damageInterface;

    // The amount of damage this NPC will do to the player each hit
    public int playerDamage = 1;

    // A timer storing the length of time the player has been in the agent's FOV
    public float playerVisibleTimer = 0.0f;

    // The transform of the player
    public Transform playerPos;

    [Header("Debugging")]

    public bool DEBUG = false;

    public Transform myTransform;

    private Vector3 ogPosition = new Vector3();
    private Quaternion ogRotation = new Quaternion();

    private bool justStarted = true;



    //[Header("Debugging")]

    //// Determines whether or not 
    //public bool showPath = true;

    //public Color pathColor = new Color(1f, 1f, 1f, 1f);

    IEnumerator assignOGTransform()
    {
        yield return new WaitForFixedUpdate();
        ogPosition.Set(myTransform.position.x, myTransform.position.y, myTransform.position.z);
        ogRotation.Set(myTransform.rotation.x, myTransform.rotation.y, myTransform.rotation.z, myTransform.rotation.w);
        //transform.SetPositionAndRotation(ogPosition, ogRotation);
    }

    //IEnumerator updateToOG()
    //{
    //    yield return new WaitForFixedUpdate();
    //    transform.SetPositionAndRotation(ogPosition, ogRotation);
    //}

    private void FixedUpdate()
    {
        if (justStarted)
        {
            ogPosition.Set(myTransform.position.x, myTransform.position.y, myTransform.position.z);
            ogRotation.Set(myTransform.rotation.x, myTransform.rotation.y, myTransform.rotation.z, myTransform.rotation.w);
            transform.SetPositionAndRotation(ogPosition, ogRotation);
            justStarted = false;
        }
    }

    private void Start()
    {
        //StartCoroutine(assignOGTransform());
    }
    void OnEnable() {
        
        collisionTime = Random.Range(1.0f, 3.0f);

        if (patrolPoints.Length == 0)
        {
            defaultState = STATE.IDLE;
        }

        getDefault();

        if (passive)
        {
            FOVMesh.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 distOffMesh = getPointNearestNavMesh(playerPos.position) - playerPos.position;

        if (distOffMesh.magnitude >= 1f)
        {
            fov.visibleTargets.Clear();
        }

        // Make the visual FOV redder as the player stays inside of it
        if (fov.visibleTargets.Count != 0 && !passive)
        {
            playerVisibleTimer += Time.deltaTime;
        }
        else
        {
            playerVisibleTimer -= Time.deltaTime;
        }

        //Debug.Log(playerVisibleTimer);

        fov.viewMeshFilter.GetComponent<MeshRenderer>().material.Lerp(passiveFOV, alertFOV, playerVisibleTimer / timeToChase);

        if (agent.isOnOffMeshLink)
        {
            agent.speed = susSpeed / 2;
        }

        // Kill NPC 1 on hitting backspace (for debug purposes)
        if (DEBUG && Input.GetKeyDown("backspace"))
        {
            die();
        } 

        // The body of the state machine, checking the state every frame and acting accordingly
        switch (state) {

            // The NPC can't hurt or see the player, and can't move until the timer runs out
            case STATE.UNCONSCIOUS:
                playerVisibleTimer = Mathf.Clamp(playerVisibleTimer, 0, 0);
                if (waitTime > 0)
                {
                    waitTime -= Time.deltaTime;
                }
                else
                {
                    conscious = true;
                    if (passive)
                    {
                        getIdle();
                    }
                    else
                    {
                        FOVMesh.enabled = true;
                        getCustom(idleState, transform.position);
                        //getSuspicious(transform.position);
                    }
                    
                }

                break;

            // In the idle state, the NPC will remain motionless for some amount of specified time
            // If the NPC is conscious while idle, their FOV will be visible and they can aggro, else they can't
            // When waking up from unconsciousness, the NPC will become suspicious
            // When ceasing awake idling, the NPC will return to their default state
            case STATE.IDLE:



                if (agent.remainingDistance <= Mathf.Epsilon)
                {
                    agent.isStopped = true;
                    transform.LookAt(transform.position + idleDir);
                }
                else
                {
                    agent.isStopped = false;
                }
                

                
                // if (Vector3.Distance(idlePos, transform.position) > 0.12)
                // {
                
                
                // }
                if (waitTime > 0)
                {
                    waitTime -= Time.deltaTime;
                }
                else
                {
                    getDefault();
                }

                if (playerVisibleTimer >= timeToSuspicion)
                {
                    getNoise(playerPos.position);
                }

                playerVisibleTimer = Mathf.Clamp(playerVisibleTimer, 0, timeToChase);

                break;

            // While patrolling, the NPC will walk between waypoints in the order of their list
            // Once they finish, they will return to the first waypoint on the list
            // Can transition into suspicion if the player stays in the FOV
            case STATE.PATROLLING:
                if ( playerVisibleTimer >= timeToSuspicion)
                {
                    getNoise(playerPos.position);
                }

                playerVisibleTimer = Mathf.Clamp(playerVisibleTimer, 0, timeToChase);

                // Assign new waypoint if current one has been reached
                if (agent.remainingDistance <= Mathf.Epsilon)
                {
                    //Debug.Log(patrolPoints[currentDest].position);
                    if (currentDest < patrolPoints.Length - 1)
                    {
                        currentDest++;
                    } else{
                        currentDest = 0;
                    }
                    agent.SetDestination(patrolPoints[currentDest].position);
                }
                //Debug.Log(Vector3.Distance(transform.position, patrolPoints[currentDest]) );
                break;
                
            
            // While chasing the player, the NPC knows their exact position
            // Transitions into suspicion when the player stays out of the enemy FOV for long enough
            case STATE.CHASING:
                playerVisibleTimer = Mathf.Clamp(playerVisibleTimer, timeToChase, timeToChase);

                targetLine.SetPosition(0, transform.position);
                targetLine.SetPosition(1, playerPos.position);



                agent.SetDestination(playerPos.position);

                if (fov.visibleTargets.Count != 0)
                {
                    timeCounter = coolTime;
                }
                else if (fov.visibleTargets.Count == 0)
                {
                    timeCounter -= Time.deltaTime;

                    if (timeCounter <= Mathf.Epsilon)
                    {
                        getNoise(playerPos.position);
                    }
                    //agent.SetDestination(patrolPoints[currentDest]);
                }


                break;

            // While suspicious, the NPC will randomly travel from waypoint to adjacent waypoint
            // It's unpredictable, but enemies will trend toward waypoints closer to the position at which they became suspicious
            // Can transition into chase if the player spends too long in the FOV, else it will revert to default after some time
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
                }
                else if (fov.visibleTargets.Count != 0)
                {
                    getNoise(playerPos.position);
                }
                else if (agent.remainingDistance <= Mathf.Epsilon)
                {
                    //waypoint currentPoint = graph.GetChild(currentDest).GetComponent<waypoint>();
                    paranoidPoints[currentPosition] += 10000;
                    waypoint currentPoint = currentPosition.GetComponent<waypoint>();

                    currentPosition = getRandomNeighbor(currentPoint);
                    agent.SetDestination(currentPosition.position);
                }

                break;

            // The NPC walks directly to some specified position
            // Can transition into chase if the player hangs around the FOV for long enough
            // Transitions into suspicion once the NPC reaches the specified position
            case STATE.NOISE:
                if (playerVisibleTimer >= timeToChase)
                {
                    getChase();
                }

                //Debug.DrawLine(transform.position, noiseSource, Color.yellow, 0.0f);

                targetLine.SetPosition(0, transform.position);
                targetLine.SetPosition(1, noiseSource);

                playerVisibleTimer = Mathf.Clamp(playerVisibleTimer, timeToSuspicion, timeToChase);

                if (fov.visibleTargets.Count != 0)
                {
                    noiseSource = noiseSource = getPointNearestNavMesh(playerPos.position);
                    agent.SetDestination(playerPos.position);
                    transform.LookAt(playerPos.position, transform.up);
                    timeCounter = suspiciousTime;
                }
                else if (agent.remainingDistance <= 0.12)
                {
                    getSuspicious(transform.position);
                }
                else
                {
                    timeCounter -= Time.deltaTime;
                }
                break;

            // CURRENTLY NOT USED
            // The NPC will randomly walk between adjacent waypoints
            // They will prefer some waypoints over others, assigning each a random weight when this state is entered
            case STATE.PARANOID:
                if ( playerVisibleTimer - timeToSuspicion >= Mathf.Epsilon)
                {
                    getNoise(playerPos.position);
                }

                playerVisibleTimer = Mathf.Clamp(playerVisibleTimer, 0, timeToChase);

                if (agent.remainingDistance <= Mathf.Epsilon)
                {
                    waypoint currentPoint = currentPosition.GetComponent<waypoint>();

                    currentPosition = getRandomNeighbor(currentPoint);
                    agent.SetDestination(currentPosition.position);
                }

                break;
        }
    }

    // Enter the default state
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

    void getCustom(STATE newState, Vector3? location = null)
    {
        if (location == null)
        {
            location = new Vector3(0, 0, 0);
        }

        switch (newState)
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
                getNoise((Vector3) location);
                break;
            case STATE.SUSPICIOUS:
                getSuspicious((Vector3) location);
                break;
            case STATE.CHASING:
                getChase();
                break;
        }
    }

    public void getChase()
    {
        agent.isStopped = false;
        targetLine.enabled = true;
        myMesh.material.color = Color.red;
        agent.speed = chaseSpeed;
        state = STATE.CHASING;

        targetLine.material.color = Color.yellow;

        data.NPCChase = data.NPCChase + 1;
    }

    public void getIdle(float time, DIRECTION dir, Vector3 pos)
    {
        myMesh.material.color = Color.blue;
        // agent.isStopped = true;
        idlePos = pos;
        agent.SetDestination(idlePos);
        waitTime = time;

        switch (dir)
        {
            case DIRECTION.NORTH:
                idleDir = new Vector3(0, 0, 1);
                break;
            case DIRECTION.SOUTH:
                idleDir = new Vector3(0, 0, -1);
                break;
            case DIRECTION.EAST:
                idleDir = new Vector3(1, 0, 0);
                break;
            case DIRECTION.WEST:
                idleDir = new Vector3(-1, 0, 0);
                break;
        }

        state = STATE.IDLE;
    }

    public void getIdle()
    {
        myMesh.material.color = Color.blue;
        //agent.isStopped = true;
        waitTime = Mathf.Infinity;
        idlePos = defaultIdlePos.transform.position;
        agent.SetDestination(idlePos);

        switch (defaultIdleDir)
        {
            case DIRECTION.NORTH:
                idleDir = new Vector3(0, 0, 1);
                break;
            case DIRECTION.SOUTH:
                idleDir = new Vector3(0, 0, -1);
                break;
            case DIRECTION.EAST:
                idleDir = new Vector3(1, 0, 0);
                break;
            case DIRECTION.WEST:
                idleDir = new Vector3(-1, 0, 0);
                break;
            case DIRECTION.HARDCODE:
                idleDir = new Vector3(1, 0, -1);
                break;
        }

        state = STATE.IDLE;
    }

    public void getUnconscious(float time)
    {
        agent.isStopped = true;
        targetLine.enabled = false;
        conscious = false;
        FOVMesh.enabled = false;
        state = STATE.UNCONSCIOUS;
        waitTime = time;
        myMesh.material.color = new Color(145 / 255f, 145 / 255f, 145 / 255f);
    }

    public void getUnconscious()
    {
        agent.isStopped = true;
        conscious = false;
        FOVMesh.enabled = false;
        state = STATE.UNCONSCIOUS;
        waitTime = unconsciousTime;
        myMesh.material.color = new Color(145 / 255f, 145 / 255f, 145 / 255f);
    }

    public void getNoise(Vector3 source)
    {
        agent.isStopped = false;
        targetLine.enabled = true;
        agent.speed = susSpeed;
        myMesh.material.color = Color.yellow;
        state = STATE.NOISE;

        targetLine.material.color = Color.red;

        noiseSource = getPointNearestNavMesh(source);
        timeCounter = suspiciousTime;
        agent.SetDestination(noiseSource);
    }

    // Forcibly transitions into suspicious state, even if currently chasing
    public void forceSuspicious()
    {
        playerVisibleTimer = Mathf.Clamp(playerVisibleTimer, timeToSuspicion, timeToSuspicion);
        getSuspicious(transform.position);
    }

    public void getSuspicious(Vector3 source)
    {
        agent.isStopped = false;
        targetLine.enabled = false;
        agent.speed = susSpeed;
        myMesh.material.color = Color.yellow;
        timeCounter = suspiciousTime;

        assignSuspiciousWalk(getPointNearestNavMesh(source));

        returnToSuspicion();

        state = STATE.SUSPICIOUS;

        data.NPCSuspicion = data.NPCSuspicion + 1;
        //timeCounter = suspiciousTime;
    }

    public void getParanoid()
    {
        agent.isStopped = false;
        targetLine.enabled = false;
        myMesh.material.color = new Color(252/255f, 139/255f, 0f);
        assignParanoidWalk();

        returnToSuspicion();

        timeToSuspicion /= 2;
        timeToChase /= 2;
        playerVisibleTimer /= 2;

        state = STATE.PARANOID;
    }

    public void getPatrol()
    {
        agent.isStopped = false;
        targetLine.enabled = false;
        agent.speed = patrolSpeed;
        returnToPatrol();
        myMesh.material.color = Color.cyan;

        state = STATE.PATROLLING;
    }


    //public void dieIdle()
    //{
    //    StartCoroutine("dieAsynchronous");
    //}
    public void die()
    {
        alive = false;
        conscious = false;
        targetLine.enabled = false;
        state = STATE.UNCONSCIOUS;
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
    }

    Vector3 getPointNearestNavMesh(Vector3 source)
    {
        NavMeshHit destHit;

        NavMesh.SamplePosition(source, out destHit, 500f, NavMesh.AllAreas);

        return destHit.position;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (state == STATE.PATROLLING && collision.gameObject.layer == LayerMask.NameToLayer("Enemies") && conscious && alive)
        {
            collisionSoFar = 0f;
        }

    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && conscious && alive)
        {
            getUnconscious();
            damageInterface.TakeDamage(playerDamage);
            data.enemyHit = data.enemyHit + 1;
            collision.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            collision.gameObject.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
        }
        else if (state == STATE.PATROLLING && collision.gameObject.layer == LayerMask.NameToLayer("Enemies") && conscious && alive)
        {
            collisionSoFar += Time.deltaTime;
            if (collisionSoFar >= collisionTime)
            {
                if (currentDest < patrolPoints.Length - 1)
                {
                    currentDest++;
                }
                else
                {
                    currentDest = 0;
                }
                agent.SetDestination(patrolPoints[currentDest].position);
                collisionSoFar = -1000f;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        collisionSoFar = -2f;
    }

    // Maps a weight to each waypoint
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

    // Maps a weight to each waypoint
    // Each waypoint is given a random weight, incentivizing unpredictable but locally consistent movement
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

        for (int i = 0; i < wayPoint.neighbors.Count; i++)
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

    // Return to the nearest waypoint on this NPC's patrol route
    void returnToPatrol()
    {
        Vector3 nearestPoint = new Vector3();
        float closestDist = Mathf.Infinity;
        int nearestIndex = -1;

        for (int i = 0; i < patrolPoints.Length; i++)
        {
            Vector3 wayPoint = patrolPoints[i].position;
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

    public void stop()
    {
        myBody.velocity = Vector3.zero;
        agent.isStopped = true;
        myBody.ResetInertiaTensor();
    }

    //public void start()
    //{

    //}

    // public void MakeIncapacitated(float time){
    //     getUnconscious(time);
    // }
}
