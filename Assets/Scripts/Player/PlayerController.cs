using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;
using System;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody pbody;
    public float speed = 15f;

    public bool hasTrapInInventory; // Check if a trap already exists in inv
    public bool inSwapCommand;      // This prevents player from swapping and dropping trap at the same time
    public GameObject trapInHand;   // Set trap that we need to spawn

    // UI
    public GameObject tmp_Pickup;
    public GameObject helpUI;
    TextMeshProUGUI tmp_Pickup_text;
    TextMeshProUGUI helpText;

    public float pickupDelay = 0f;

    public Vector3 spawnPosition;  //To get this position in the respawn script
    public Quaternion spawnRotation;

    public GameObject gameOverPanel;

    public LiveCounter npcManager;

    public TrapPlacer TP;

    public bool canPause = true;
    public GameObject pauseScreen;

    private bool eDown;

    private Collider myTrigger;

    private PopUpSystem pop;

    private AudioSource playerFootsteps;

    void Start()
    {
        //inSwapCommand = false;
        hasTrapInInventory = false;
        tmp_Pickup_text = tmp_Pickup.GetComponent<TextMeshProUGUI>();
        helpText = helpUI.GetComponent<TextMeshProUGUI>();

        spawnPosition = transform.position;  //To get these coordinates in the respawn
        spawnRotation = transform.rotation;

        pop = this.GetComponent<PopUpSystem>();
        playerFootsteps = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //float horizontal = Input.GetAxisRaw("Horizontal");
        //float vertical = Input.GetAxisRaw("Vertical");

        ////Debug.Log(vertical);

        //Vector3 mag = new Vector3(horizontal, 0f, vertical).normalized;

        //Vector3 velocity = mag * movementSpeed * Time.deltaTime;

        //pbody.MovePosition(transform.position + velocity);


        //float horVal = Input.GetAxis("Horizontal");
        //float vertVal = Input.GetAxis("Vertical");

        ////Vector3 newPos = new Vector3(horVal * Time.deltaTime * speed, 0, vertVal * Time.deltaTime * speed);
        //Vector3 newPos = new Vector3(horVal, 0, vertVal);
        //newPos *= Time.deltaTime * speed;
        ////newPos.Normalize();
        ////Debug.Log("Translation vector: " + newPos);
        ////Debug.Log("Vector magnitude: " + newPos.magnitude);


        //pbody.velocity = newPos * 100;
        //pbody.AddForce(newPos, ForceMode.VelocityChange);
        //pbody.MovePosition(transform.position + newPos);
        //transform.Translate(newPos);
         


        // if(pickupDelay > 0) pickupDelay -= 1f * Time.deltaTime;
        // else {
        //     pickupDelay = 0;
            if (hasTrapInInventory) tmp_Pickup_text.text = trapInHand.GetComponentInChildren<BaseTrapClass>().trapName + " Trap";
            else tmp_Pickup_text.text = "Empty";
        //}

        if (Input.GetKeyDown(KeyCode.E) && TP.eLocked == 2)
        {
            eDown = true;
        }
        else if (TP.eLocked == 1 || Input.GetKeyUp(KeyCode.E))
        {
            eDown = false;
        }

        //if (Input.GetKeyDown(KeyCode.E) && TP.eLocked == 1)
        //{
        //    TP.eLocked = 2;
        //    //StartCoroutine(unregisterE());
        //}
        //else if (Input.GetKeyUp(KeyCode.E) && TP.eLocked == 2)
        //{
        //    StartCoroutine(unLockE());
        //}

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (canPause)
            {
                pauseGame();
            } else
            {
                if (pop) pop.PopDown();
                canPause = true;
                Time.timeScale = 1;
            }
        }
    


        // Footsteps Sound Code
        if (Input.GetKeyDown(KeyCode.W) | Input.GetKeyDown(KeyCode.A) | Input.GetKeyDown(KeyCode.S) |Input.GetKeyDown(KeyCode.D))
        {
            if (playerFootsteps && !playerFootsteps.isPlaying)
            {
                playerFootsteps.Play();
            }
            // FindObjectOfType<AudioManager>().Play("PlayerFootSteps");
        }

        else if(Input.GetKeyUp(KeyCode.W) | Input.GetKeyUp(KeyCode.A) | Input.GetKeyUp(KeyCode.S) | Input.GetKeyUp(KeyCode.D))
        {
            if (!(Input.GetKey(KeyCode.W) | Input.GetKey(KeyCode.A) | Input.GetKey(KeyCode.S) |Input.GetKey(KeyCode.D)))
            {
                if (playerFootsteps)
                {
                    playerFootsteps.Stop();
                }
                // FindObjectOfType<AudioManager>().Stop("PlayerFootSteps");
            }
        }
    }

    private void FixedUpdate()
    {
        inSwapCommand = false;
        myTrigger = null;

        float horVal = Input.GetAxis("Horizontal");
        float vertVal = Input.GetAxis("Vertical");

        //Vector3 newPos = new Vector3(horVal * Time.deltaTime * speed, 0, vertVal * Time.deltaTime * speed);
        Vector3 newPos = new Vector3(horVal, 0, vertVal);
        newPos *= Time.deltaTime * speed;
        //newPos.Normalize();
        //Debug.Log("Translation vector: " + newPos);
        //Debug.Log("Vector magnitude: " + newPos.magnitude);
        newPos *= 40;
        //Vector3 temp = pbody.velocity;
        //temp.x = newPos.x;
        //temp.z = newPos.z;
        //temp.y = pbody.velocity.y;
        //pbody.velocity = temp;

        pbody.velocity = new Vector3(newPos.x, pbody.velocity.y, newPos.z);
        pbody.AddForce(Physics.gravity * 2, ForceMode.Acceleration);
    }

    void OnTriggerStay(Collider triggerObject){
        int a;

        
        // Setting a pickup delay so that the text doesn't flash rapidly
        if(pickupDelay <= 0){
            a = 4;

            // We need to know if the trigger box we've entered is a pickup object AND
            // if the object has been triggered before or not
            if(triggerObject.gameObject.layer == LayerMask.NameToLayer("Pickup")) {
                //&& !triggerObject.gameObject.GetComponent<BaseTrapClass>().isTriggered ){
                a = 2;
                // Different logic depending on whether or not there is currently a trap
                // in inventory
                if(hasTrapInInventory && triggerObject.gameObject.GetComponentInChildren<BaseTrapClass>()){
                    inSwapCommand = true;
                    myTrigger = triggerObject;
                    //StartCoroutine(swapOff());

                    if(triggerObject.gameObject.GetComponentInChildren<BaseTrapClass>()){
                        helpText.text = "[E] SWAP to " + triggerObject.gameObject.GetComponentInChildren<BaseTrapClass>().trapName + " Trap!";
                    }
                    
                    //if(eDown){
                    //    // Swap object positions  
                    //    Vector3 newObjectPos = triggerObject.gameObject.transform.position;             
                    //    trapInHand.transform.position = newObjectPos;

                    //    // Swap object On/Off
                    //    trapInHand.SetActive(true);
                    //    triggerObject.gameObject.SetActive(false);

                    //    // Move the current object we are over into inventory
                    //    // and ensure that hasTrapInInventory is true.
                    //    trapInHand = triggerObject.gameObject;
                    //    hasTrapInInventory = true; 

                    //    tmp_Pickup_text.text = "Trap Swapped!";
                    //    //pickupDelay = 1f;

                    //    helpText.text = "";
                    //    inSwapCommand = false;

                    //    //eDown = false;
                    //    TP.eLocked = 1;
                    //}
                } else {

                    //inSwapCommand = false;
                    //myTrigger = null;

                    if (triggerObject.gameObject.GetComponentInChildren<BaseTrapClass>()){
                        helpText.text = "[E] PICK UP the " + triggerObject.gameObject.GetComponentInChildren<BaseTrapClass>().trapName + " Trap!";
                        
                        if (eDown)
                        {
                            trapInHand = triggerObject.gameObject;
                            triggerObject.gameObject.SetActive(false);
                            hasTrapInInventory = true;

                            string temp = triggerObject.gameObject.tag;
                            data.addTrapVal(temp);

                            helpText.text = "";
                            //pickupDelay = 1f;

                            //eDown = false;

                            //TP.eLocked = 1;

                            StartCoroutine(transferControl());

                            if(triggerObject.gameObject.GetComponentInChildren<BaseTrapClass>().trapName == "Noise" && 
                                SceneManager.GetActiveScene().name == "Level 4 Noise Trap Tutorial" && !TP.placedTrapBefore)
                            {
                                string popupText = "This is a Noise Trap! Use it to lure in enemies!";
                                canPause = false;
                                if (pop) pop.PopUp(popupText);
                            } else if(triggerObject.gameObject.GetComponentInChildren<BaseTrapClass>().trapName == "Freeze" &&
                                SceneManager.GetActiveScene().name == "Level 5 Speedball" && !TP.placedTrapBefore)
                            {
                                string popupText = "This is a Freeze Trap! Use it to stun enemies in place!";
                                canPause = false;
                                if (pop) pop.PopUp(popupText);
                            }
                        }
                    } else if(gameObject.GetComponent<PlayerDamage>().currentHealth != gameObject.GetComponent<PlayerDamage>().maxHealth){
                        gameObject.GetComponent<PlayerDamage>().HealDamage();
                        Destroy(triggerObject.gameObject);
                    }     
                }
            }
        } 
    }

    void OnTriggerExit(Collider triggerObject){
        // This is to ensure we clean the helpText and inSwapCommand bools
        // in case we leave a trigger box without picking an object up
        helpText.text = "";
        //inSwapCommand = false;

        if(triggerObject.gameObject.layer == LayerMask.NameToLayer("pitTrapTutorial") &&
            SceneManager.GetActiveScene().name == "Level 1 Pit Trap Tutorial")
        {
            Sprite image = triggerObject.gameObject.GetComponent<Image>().sprite;
            canPause = false;
            if (pop) pop.PopUpImage(image);
        }else if(triggerObject.gameObject.layer == LayerMask.NameToLayer("endZoneTutorial") &&
            SceneManager.GetActiveScene().name == "Level 1 Pit Trap Tutorial")
        {
            Sprite image = triggerObject.gameObject.GetComponent<Image>().sprite;
            canPause = false;
            if (pop) pop.PopUpImage(image);
        }else if(triggerObject.gameObject.layer == LayerMask.NameToLayer("navTutorial") &&
            SceneManager.GetActiveScene().name == "Level 1 Pit Trap Tutorial")
        {
            Sprite image = triggerObject.gameObject.GetComponent<Image>().sprite;
            canPause = false;
            if (pop) pop.PopUpImage(image);
        }

    }

    public void swapTraps()
    {
        //    // Swap object positions  
        Vector3 newObjectPos = myTrigger.gameObject.transform.position;
        trapInHand.transform.position = newObjectPos;

        // Swap object On/Off
        trapInHand.SetActive(true);
        myTrigger.gameObject.SetActive(false);

        // Move the current object we are over into inventory
        // and ensure that hasTrapInInventory is true.
        trapInHand = myTrigger.gameObject;
        hasTrapInInventory = true;

        tmp_Pickup_text.text = "Trap Swap!";
        //pickupDelay = 1f;

        helpText.text = "";
        inSwapCommand = false;
        myTrigger = null;
    }

    // void resetData()
    // {
    //     data.startTime = System.DateTime.Now;
    //     data.endTime = System.DateTime.Now;
    //     data.playerDeath = "no";
    //     data.levelName = "demo";
    //     data.gameCompleted = false;
    //     //data.trapActiveOrder = new List<string>();
    //     data.healthRemaining = 0;
    //     data.enemyHit = 0;
    //     data.ttrstart = System.DateTime.Now;
    //     data.NPCChase = 0;
    //     data.NPCSuspicion = 0;
    // }
    public IEnumerator playerDie(float delay)
    {
        canPause = false;
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
        data.playerDeath = "yes";
        data.levelName = SceneManager.GetActiveScene().name;
        data.checkData();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameOverPanel.SetActive(true);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void playerDie()
    {
        canPause = false;
        npcManager.cease();
        StartCoroutine(playerDie(0f));
        
    }

      public void RespawnPlayer()
    {
        transform.position = spawnPosition;
        transform.rotation = spawnRotation;
        npcManager.stopChase();
    }

    IEnumerator transferControl()
    {
        yield return new WaitForEndOfFrame();
        TP.eLocked = 1;
    }

    void pauseGame()
    {
        pauseScreen.SetActive(true);
        //Time.timeScale = 0;
    }

    public void pausePlayer()
    {
        speed = 0;
        GetComponent<PlayerDamage>().invincible = true;
    }

    public void unPausePlayer()
    {
        speed = 15f;
        GetComponent<PlayerDamage>().invincible = false;
    }

    //IEnumerator swapOff()
    //{
    //    yield return new WaitForEndOfFrame();
    //    inSwapCommand = false;
    //    myTrigger = null;
    //}

    // Unlocks the semaphore for the E key
    //IEnumerator unLockE()
    //{
    //    yield return new WaitForEndOfFrame();
    //    TP.eLocked = 0;
    //}

    //// Tells the system to stop registering E after this frame
    //IEnumerator unregisterE()
    //{
    //    yield return new WaitForEndOfFrame();
    //    eDown = false;
    //}
}
