using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using static Unity.VisualScripting.Member;

public class StateMachine_Robust : MonoBehaviour
{
    // Enumerated type for possible states
    public enum STATE { IDLE, PATROLLING, SUSPICIOUS, CHASING, PARANOID, NOISE, UNCONSCIOUS, FROZEN };

    // Specifies a cardinal direction to look in for the idle state
    public enum DIRECTION { NORTH, SOUTH, EAST, WEST, HARDCODE };

    // Specifies the type of NPC this is
    public enum TYPE { STANDARD, SPEEDBALL };



    [Header("State Settings")]

    // Current state of the machine
    public STATE state = STATE.IDLE;

    // The machine will eventually revert back to this state by default
    public STATE defaultState = STATE.PATROLLING;

    // Denotes the type of NPC this is
    public TYPE enemyType = TYPE.STANDARD;

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

    // Denotes whether the NPC is following a noise trap or not
    public bool noiseTriggered;

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
    public float waitTime = 0.0f;

    // Stores the state of the NPC when it's frozen
    private STATE stateCache;

    // Stores whether the NPC is stopped when frozen
    private bool stopCache;

    // Stores the NPC's current destination when frozen
    private Vector3 destinationCache;

    // Stores the NPC's current path when frozen
    private NavMeshPath pathCache;

    private Vector3 aiVelocityCache;

    private Vector3 positionCache;

    private float speedCache;

    private Vector3 massCache;

    private Vector3 inertiaCache;

    private Quaternion inertiaRotationCache;

    private Vector3 velocityCache;

    private Vector3 angularVelocityCache;



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

    // Script that manages the NPC's offMeshLink
    public workingOffLinkScript linkScript;



    [Header("Player Interaction")]

    // The interface for damaging the player
    public PlayerDamage damageInterface;

    // The amount of damage this NPC will do to the player each hit
    public int playerDamage = 1;

    // A timer storing the length of time the player has been in the agent's FOV
    public float playerVisibleTimer = 0.0f;

    // The transform of the player
    public Transform playerPos;

    //public AudioSource AIAudio;

    [Header("Debugging")]

    public bool DEBUG = false;

    public Transform myTransform;

    private Vector3 ogPosition = new Vector3();
    private Quaternion ogRotation = new Quaternion();

    private bool justStarted = true;

    public Material FOVPassive;

    [Header("Art and Audio")]

    //public AudioManager audioManager;

    public LocalAudioManager localAudioManager;


    //[Header("Debugging")]

    //// Determines whether or not 
    //public bool showPath = true;

    //public Color pathColor = new Color(1f, 1f, 1f, 1f);

    // IEnumerator assignOGTransform()
    // {
    //     yield return new WaitForFixedUpdate();
    //     ogPosition.Set(myTransform.position.x, myTransform.position.y, myTransform.position.z);
    //     ogRotation.Set(myTransform.rotation.x, myTransform.rotation.y, myTransform.rotation.z, myTransform.rotation.w);
    //     //transform.SetPositionAndRotation(ogPosition, ogRotation);
    // }

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
        //audioManager.Play("360BallSound");

        //audioManager.Play(name: "360BallSound", channel: 1, volume: 0.2f);
        //AIAudio = this.GetComponent<AudioSource>();
    }
    void OnEnable()
    {
        fov.viewMeshFilter.GetComponent<MeshRenderer>().material = FOVPassive;

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
        else
        {
            FOVMesh.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.pathPending)
        {
            return;
        }
        Vector3 distOffMesh = getPointNearestNavMesh(playerPos.position) - playerPos.position;

        if (distOffMesh.magnitude >= 1f)
        {
            fov.visibleTargets.Clear();
        }

        // Make the visual FOV redder as the player stays inside of it

        //Debug.Log(fov.visibleTargets.Count);

        fov.FindVisibleTargets();

        if (fov.visibleTargets.Count != 0 && !passive && state != STATE.FROZEN)
        {
            playerVisibleTimer += Time.deltaTime;
        }
        else
        {
            playerVisibleTimer -= Time.deltaTime;
        }

        //Debug.Log(playerVisibleTimer);

        fov.viewMeshFilter.GetComponent<MeshRenderer>().material.Lerp(passiveFOV, alertFOV, playerVisibleTimer / timeToChase);

        //if (agent.isOnOffMeshLink)
        //{
        //    agent.speed = susSpeed / 2;
        //}

        //Debug.Log(state);

        if (linkScript && linkScript.enabled)
        {
            linkScript.UpdateLink();
        }

        // The body of the state machine, checking the state every frame and acting accordingly
        switch (state)
        {

            case STATE.FROZEN:
                break;

            // The NPC can't hurt or see the player, and can't move until the timer runs out
            case STATE.UNCONSCIOUS:

                // Stop chase  sound 
                // FindObjectOfType<AudioManager>().Stop("NPCChaseSound");


                playerVisibleTimer = Mathf.Clamp(playerVisibleTimer, 0, 0);
                if (waitTime > 0)
                {
                    waitTime -= Time.deltaTime;
                }
                else
                {
                    conscious = true;
                    agent.enabled = true;
                    myBody.isKinematic = false;
                    // playSound(clipName: "NPCWakes", channelno: 3, vol: 0.2f, looptf: false);
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

                //if (audioManager && AIAudio)
                //{
                //    if (AIAudio.isPlaying)
                //    {
                //        stopallSounds();
                //    }
                //}

                break;

            // In the idle state, the NPC will remain motionless for some amount of specified time
            // If the NPC is conscious while idle, their FOV will be visible and they can aggro, else they can't
            // When waking up from unconsciousness, the NPC will become suspicious
            // When ceasing awake idling, the NPC will return to their default state
            case STATE.IDLE:

                


                if (agent.remainingDistance <= Mathf.Epsilon && !agent.isStopped)
                {
                    agent.isStopped = true;
                    transform.LookAt(transform.position + idleDir);
                }
                //else
                //{
                //    agent.isStopped = false;
                //}



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
                    return;
                }

                playerVisibleTimer = Mathf.Clamp(playerVisibleTimer, 0, timeToChase);


                //if (fov.visibleTargets.Count != 0)
                //{
                //    getNoise(playerPos.position);
                //}
                // Assign new waypoint if current one has been reached
                if (fov.visibleTargets.Count != 0)
                {
                    //noiseSource = noiseSource = getPointNearestNavMesh(playerPos.position);
                    //agent.SetDestination(playerPos.position);
                    transform.LookAt(playerPos.position, transform.up);
                    // timeCounter = suspiciousTime;
                }
                else if (agent.remainingDistance <= Mathf.Epsilon)
                {
                    //Debug.Log(patrolPoints[currentDest].position);
                    if (currentDest < patrolPoints.Length - 1)
                    {
                        currentDest++;
                    }
                    else
                    {
                        currentDest = 0;
                    }
                    agent.SetDestination(patrolPoints[currentDest].position);
                }
                //Debug.Log(Vector3.Distance(transform.position, patrolPoints[currentDest]) );

                //if(audioManager && AIAudio)
                //{
                //    if(AIAudio.clip.name != "NPC_Walk")
                //    {
                //        stopallSounds();
                //        AIAudio.loop = true;
                //        AIAudio.volume = 0.3f;
                //        AIAudio.clip = audioManager.findSound("NPCFootStepsWalk").clip;
                //        AIAudio.Play();
                //    }
                //}

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

                //if (audioManager && AIAudio)
                //{
                //    if (AIAudio.clip.name != "NPCChase")
                //    {
                //        stopallSounds();
                //        AIAudio.loop = true;
                //        AIAudio.volume = 0.3f;
                //        AIAudio.clip = audioManager.findSound("NPCChaseSound").clip;
                //        AIAudio.Play();
                //    }
                //}

                break;

            // While suspicious, the NPC will randomly travel from waypoint to adjacent waypoint
            // It's unpredictable, but enemies will trend toward waypoints closer to the position at which they became suspicious
            // Can transition into chase if the player spends too long in the FOV, else it will revert to default after some time
            case STATE.SUSPICIOUS:
                if (playerVisibleTimer >= timeToChase)
                {
                    getChase();
                    return;
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
                    //paranoidPoints[currentPosition] = Mathf.RoundToInt(Mathf.Pow(paranoidPoints[currentPosition], 2f));

                    paranoidPoints[currentPosition] += 10000;
                    waypoint currentPoint = currentPosition.GetComponent<waypoint>();

                    Transform neighbor = getRandomNeighbor(currentPoint);

                    if (neighbor != null)
                    {
                        currentPosition = neighbor;
                        agent.SetDestination(currentPosition.position);
                    }
                }

                //if (audioManager && AIAudio)
                //{
                //    if (AIAudio.clip.name != "NPCSus")
                //    {
                //        stopallSounds();
                //        AIAudio.loop = false;
                //        AIAudio.volume = 0.3f;
                //        AIAudio.clip = audioManager.findSound("NPCSus").clip;
                //        AIAudio.Play();
                //    }
                //}

                break;

            // The NPC walks directly to some specified position
            // Can transition into chase if the player hangs around the FOV for long enough
            // Transitions into suspicion once the NPC reaches the specified position
            case STATE.NOISE:
                if (playerVisibleTimer >= timeToChase)
                {
                    disableLink();
                    getChase();
                    return;
                }

                //Debug.DrawLine(transform.position, noiseSource, Color.yellow, 0.0f);

                targetLine.SetPosition(0, transform.position);
                targetLine.SetPosition(1, noiseSource);

                playerVisibleTimer = Mathf.Clamp(playerVisibleTimer, timeToSuspicion, timeToChase);

                //Debug.Log(agent.speed);

                if (agent.isOnOffMeshLink)
                {
                    agent.speed = susSpeed / 2;
                }
                else
                {
                    agent.speed = susSpeed;
                }

                if (fov.visibleTargets.Count != 0)
                {
                    //noiseSource = getPointNearestNavMesh(playerPos.position);
                    //agent.SetDestination(playerPos.position);
                    //transform.LookAt(playerPos.position, transform.up);
                    //timeCounter = suspiciousTime;
                    disableLink();
                    getNoise(playerPos.position);


                }
                else if (agent.remainingDistance <= 0.12)
                {
                    disableLink();
                    getSuspicious(transform.position);
                }
                //else
                //{
                //    timeCounter -= Time.deltaTime;
                //}

                //if (audioManager && AIAudio)
                //{
                //    if (AIAudio.clip.name != "NPCSus")
                //    {
                //        stopallSounds();
                //        AIAudio.loop = false;
                //        AIAudio.volume = 0.3f;
                //        AIAudio.clip = audioManager.findSound("NPCSus").clip;
                //        AIAudio.Play();
                //    }
                //}

                break;

            // CURRENTLY NOT USED
            // The NPC will randomly walk between adjacent waypoints
            // They will prefer some waypoints over others, assigning each a random weight when this state is entered
            case STATE.PARANOID:
                if (playerVisibleTimer - timeToSuspicion >= Mathf.Epsilon)
                {
                    getNoise(playerPos.position);
                    return;
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

        // Kill NPC 1 on hitting backspace (for debug purposes)
        if (DEBUG && Input.GetKeyDown("backspace"))
        {
            //die();
            if (state != STATE.FROZEN)
            {
                getFrozen();
            }
            else
            {
                unFreeze();
            }
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
                getNoise((Vector3)location);
                break;
            case STATE.SUSPICIOUS:
                getSuspicious((Vector3)location);
                break;
            case STATE.CHASING:
                getChase();
                break;
        }
    }

    public void playSound(string clipName, int channelno, bool looptf, float vol, float pitchlocal = 1f)
    {
        //if (audioManager && AIAudio)
        //{
        //    AIAudio.loop = true;
        //    AIAudio.volume = 0.3f;
        //    AIAudio.clip = audioManager.findSound(clipName).clip;
        //    AIAudio.Play();
        //}

        if (localAudioManager)
        {
            localAudioManager.Play(name: clipName, channel: channelno, loop: looptf, volume: vol, pitch: pitchlocal);
        }
    }

    public void stopallSounds()
    {
        if (localAudioManager)
        {
             localAudioManager.StopAll();
        }
       
    }

    public void stopchannelSound(int channel)
    {
        
       if (localAudioManager)
       {
         localAudioManager.Stop(channel);
       }
    }

    public void getChase()
    {
        disableLink();
        // Play  sound 
        // FindObjectOfType<AudioManager>().Play("NPCChaseSound");
        //playSound("NPCChaseSound");
        playSound(clipName: "NPCChaseSound", channelno: 2, vol: 0.2f, looptf: true);
        playSound(clipName: "NPCFootStepsWalk", channelno: 1, vol: 0.4f, looptf: true, pitchlocal: 4f);

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
        disableLink();
        stopallSounds();
        myMesh.material.color = Color.blue;
        // agent.isStopped = true;
        targetLine.enabled = false;
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
        disableLink();
        // Play  sound 
        // FindObjectOfType<AudioManager>().Play("NPCFootSteps");
        stopallSounds();
        myMesh.material.color = Color.blue;
        //agent.isStopped = true;
        targetLine.enabled = false;
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
        disableLink();
        if (agent.enabled)
        {
            agent.isStopped = true;
        }
        
        stopallSounds();
        playSound(clipName: "NPCCollide", channelno: 3, vol: 0.2f, looptf: false);
        targetLine.enabled = false;
        conscious = false;
        FOVMesh.enabled = false;
        state = STATE.UNCONSCIOUS;
        waitTime = time;
        myMesh.material.color = new Color(145 / 255f, 145 / 255f, 145 / 255f);
    }

    public void getUnconscious()
    {
        disableLink();
        if (agent.enabled)
        {
            agent.isStopped = true;
        }
        stopallSounds();
        playSound(clipName: "NPCCollide", channelno: 3, vol: 0.2f, looptf: false);
        
        conscious = false;
        FOVMesh.enabled = false;
        targetLine.enabled = false;
        state = STATE.UNCONSCIOUS;
        waitTime = unconsciousTime;
        myMesh.material.color = new Color(145 / 255f, 145 / 255f, 145 / 255f);
        
    }

    public void getNoise(Vector3 source)
    {
        //playSound("NPCSus");
        playSound(clipName: "NPCSus", channelno: 2, vol: 0.2f, looptf: true);

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
        disableLink();
        playerVisibleTimer = Mathf.Clamp(playerVisibleTimer, timeToSuspicion, timeToSuspicion);
        getSuspicious(transform.position);
    }

    public void getSuspicious(Vector3 source)
    {
        
        disableLink();
        //playSound("NPCSus");
        playSound(clipName: "NPCSus", channelno: 2, vol: 0.2f, looptf: true);
        //playSound(clipName: "NPCFootStepsWalk", channelno: 1, vol: 0.2f, looptf: true);
        playFootSteps();

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
        //playSound("NPCSus");
        agent.isStopped = false;
        targetLine.enabled = false;
        myMesh.material.color = new Color(252 / 255f, 139 / 255f, 0f);
        assignParanoidWalk();

        returnToSuspicion();

        timeToSuspicion /= 2;
        timeToChase /= 2;
        playerVisibleTimer /= 2;

        state = STATE.PARANOID;
    }

    public void getPatrol()
    {
        disableLink();
        agent.isStopped = false;
        targetLine.enabled = false;
        agent.speed = patrolSpeed;

        // Play  sound 
        // FindObjectOfType<AudioManager>().Play("NPCFootSteps");

        //playSound(clipName: "NPCFootStepsWalk", channelno: 1, vol: 0.2f, looptf: true);

        playFootSteps();

        stopchannelSound(2);


        returnToPatrol();
        myMesh.material.color = Color.cyan;

        // playSound("NPCFootStepsWalk");

        state = STATE.PATROLLING;
    }


    //public void dieIdle()
    //{
    //    StartCoroutine("dieAsynchronous");
    //}
    public void die()
    {
        disableLink();
        // STop  sound 
        // FindObjectOfType<AudioManager>().Stop("NPCChaseSound");

        // FindObjectOfType<AudioManager>().Stop("NPCFootSteps");
    
     playSound(clipName: "NPCDeath", channelno: 1, vol: 0.3f, looptf: false);
        
        
        alive = false;
        conscious = false;
        targetLine.enabled = false;
        state = STATE.UNCONSCIOUS;
        waitTime = 10f;
        if (agent.enabled)
        {
            agent.isStopped = true;
        }
        FOVMesh.enabled = false;
        NPCManager.decrement();
        myMesh.material.color = Color.black;
        //Destroy(fov.gameObject);
        StartCoroutine("dieAsynchronous");
        // stopallSounds();
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
        /*if ((collision.gameObject.layer == LayerMask.NameToLayer("safeCloset")|| collision.gameObject.layer == LayerMask.NameToLayer("safePlayer")) && conscious && alive)
        {
            //getUnconscious();
            //state = STATE.PATROLLING;
            agent.SetDestination(patrolPoints[currentDest].position);
         
            //collision.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            //collision.gameObject.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
        }*/
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
    // Neighbors with lower weights are preferred
    Transform getRandomNeighbor(waypoint wayPoint)
    {
        Transform bestPoint = null;
        int smallestWeight = 1000000000;

        for (int i = 0; i < wayPoint.neighbors.Count; i++)
        {
            Transform currentPoint = wayPoint.neighbors[i].transform;
            int diceRoll = Random.Range(1, 50);

            int weight = paranoidPoints[currentPoint] + diceRoll;
            if (weight < smallestWeight)
            {
                smallestWeight = weight;
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

    public void stop(float seconds)
    {
        agent.isStopped = true;
        agent.enabled = false;
        myBody.isKinematic = true;
        getUnconscious(seconds);
        //myBody.angularVelocity = Vector3.zero;
        //myBody.velocity = Vector3.zero;

        //myBody.ResetInertiaTensor();
        //myBody.mass = 1000000;
        //myTransform.SetPositionAndRotation(position, myTransform.rotation);
        //myBody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX;
    }

    //public void stop()
    //{
    //    getFrozen();
    //}

    public void getFrozen()
    {
        velocityCache = myBody.velocity;
        angularVelocityCache = myBody.angularVelocity;
        inertiaCache = myBody.inertiaTensor;
        massCache = myBody.centerOfMass;
        inertiaRotationCache = myBody.inertiaTensorRotation;

        destinationCache = agent.destination;
        //pathCache = agent.path;
        //aiVelocityCache = agent.velocity;
        //positionCache = agent.nextPosition;

        //speedCache = agent.speed;
        //stopCache = agent.isStopped;
        //agent.isStopped = true;
        //agent.enabled = false;
        myBody.isKinematic = true;
        //stateCache = state;
        //myBody.constraints = RigidbodyConstraints.FreezeAll;
        

        stateCache = state;
        stopCache = agent.isStopped;
        //agent.isStopped = true;
        agent.enabled = false;


        state = STATE.FROZEN;
    }

    public void unFreeze()
    {
        state = stateCache;
        agent.enabled = true;
        agent.isStopped = stopCache;

        //state = stateCache;
        //conscious = true;
        //agent.enabled = true;


        //state = stateCache;
        //agent.isStopped = stopCache;
        agent.destination = destinationCache;
        //agent.path = pathCache;
        //agent.velocity = aiVelocityCache;
        //agent.nextPosition = positionCache;

        myBody.isKinematic = false;
        //myBody.constraints = RigidbodyConstraints.None;
        myBody.velocity = velocityCache;
        myBody.angularVelocity = angularVelocityCache;
        myBody.inertiaTensor = inertiaCache;
        myBody.centerOfMass = massCache;
        myBody.inertiaTensorRotation = inertiaRotationCache;

        //agent.speed = speedCache;

        //agent.ResetPath();
        //agent.SetDestination(destinationCache);
        //agent.path = pathCache;
        //agent.CalculatePath(agent.destination);

        //agent.Resume();


    }

    private void disableLink()
    {
        if (gameObject.GetComponent<workingOffLinkScript>())
        {
            noiseTriggered = false;
            gameObject.GetComponent<workingOffLinkScript>().enabled = false;
        }
    }

    private void playFootSteps()
    {
        switch (enemyType)
        {
            case TYPE.STANDARD:
                playSound(clipName: "NPCFootStepsWalk", channelno: 1, vol: 0.4f, looptf: true, pitchlocal: 1.5f);
                break;
            case TYPE.SPEEDBALL:
                playSound(clipName: "360BallSound", channelno: 1, vol: 0.2f, looptf: true, pitchlocal: 1.3f);
                break;
        }
    }

    //public void start()
    //{

    //}

    // public void MakeIncapacitated(float time){
    //     getUnconscious(time);
    // }
}
