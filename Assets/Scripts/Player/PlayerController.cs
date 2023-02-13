using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    public float pickupDelay;

    void Start()
    {
        inSwapCommand = false;
        hasTrapInInventory = false;
        tmp_Pickup_text = tmp_Pickup.GetComponent<TextMeshProUGUI>();
        helpText = helpUI.GetComponent<TextMeshProUGUI>();
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


        float horVal = Input.GetAxis("Horizontal");
        float vertVal = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(horVal * Time.deltaTime * speed, 0, vertVal * Time.deltaTime * speed));

        if(pickupDelay > 0) pickupDelay -= 1f * Time.deltaTime;
        else {
            pickupDelay = 0;
            if(hasTrapInInventory) tmp_Pickup_text.text = "Inventory:\n1 " + trapInHand.GetComponentInChildren<BaseTrapClass>().trapName + " Trap";
            else tmp_Pickup_text.text = "Inventory: Empty";
        }

        data.checkGameCompleted(data.gameCompleted);
    }

    void OnTriggerStay(Collider triggerObject){

        // Setting a pickup delay so that the text doesn't flash rapidly
        if(pickupDelay <= 0){

            // We need to know if the trigger box we've entered is a pickup object AND
            // if the object has been triggered before or not
            if(triggerObject.gameObject.layer == LayerMask.NameToLayer("Pickup")) {
            //&& !triggerObject.gameObject.GetComponent<BaseTrapClass>().isTriggered ){

                // Different logic depending on whether or not there is currently a trap
                // in inventory
                if(hasTrapInInventory){
                    inSwapCommand = true;

                    if(triggerObject.gameObject.GetComponentInChildren<BaseTrapClass>()){
                        helpText.text = "[E] SWAP to " + triggerObject.gameObject.GetComponentInChildren<BaseTrapClass>().trapName + " Trap!";
                    }
                    
                    if(Input.GetKey(KeyCode.E)){ 
                        // Swap object positions  
                        Vector3 newObjectPos = triggerObject.gameObject.transform.position;             
                        trapInHand.transform.position = newObjectPos;

                        // Swap object On/Off
                        trapInHand.SetActive(true);
                        triggerObject.gameObject.SetActive(false);

                        // Move the current object we are over into inventory
                        // and ensure that hasTrapInInventory is true.
                        trapInHand = triggerObject.gameObject;
                        hasTrapInInventory = true; 

                        tmp_Pickup_text.text = "Trap Swapped!";
                        pickupDelay = 1f;

                        helpText.text = "";
                        inSwapCommand = false;
                    }
                } else {

                    if(triggerObject.gameObject.GetComponentInChildren<BaseTrapClass>()){
                        helpText.text = "[E] PICK UP the " + triggerObject.gameObject.GetComponentInChildren<BaseTrapClass>().trapName + " Trap!";
                    }

                    if(Input.GetKey(KeyCode.E) && triggerObject.gameObject.layer == LayerMask.NameToLayer("Pickup")){
                        trapInHand = triggerObject.gameObject;
                        triggerObject.gameObject.SetActive(false);
                        hasTrapInInventory = true;

                        helpText.text = "";
                        pickupDelay = 1f;
                    }
                }
            }
        } 
    }

    void OnTriggerExit(){
        // This is to ensure we clean the helpText and inSwapCommand bools
        // in case we leave a trigger box without picking an object up
        helpText.text = "";
        inSwapCommand = false;
    }

    public IEnumerator playerDie(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject, 0.5f);
        data.playerDeath = "yes";
        data.endTime = System.DateTime.Now;
        data.gameCompleted = true;
        Debug.Log(data.gameCompleted);
    }

    public void playerDie()
    {
        StartCoroutine(playerDie(0f));
    }

}
