using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody pbody;
    public float speed = 15f;

    public bool hasTrapInInventory;
    public GameObject trapInHand;

    // UI
    public GameObject tmp_Pickup;
    TextMeshProUGUI tmp_Pickup_text;

    private float pickupDelay;

    void Start()
    {
         hasTrapInInventory = false;
         tmp_Pickup_text = tmp_Pickup.GetComponent<TextMeshProUGUI>();
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
            if(trapInHand) tmp_Pickup_text.text = "Inventory: \n 1 Trap";
        }

        data.checkGameCompleted(data.gameCompleted);
    }

    void OnTriggerEnter(Collider triggerObject){
        if(!trapInHand && triggerObject.gameObject.layer == LayerMask.NameToLayer("Pickup")){
                trapInHand = triggerObject.gameObject;
                triggerObject.gameObject.SetActive(false);
                hasTrapInInventory = true;
                tmp_Pickup_text.text = "Inventory: \n 1 Trap";
        }
    }

    void OnTriggerStay(Collider triggerObject){
        if(pickupDelay <= 0){
            if(trapInHand){
                if(Input.GetKey(KeyCode.E) && triggerObject.gameObject.layer == LayerMask.NameToLayer("Pickup")){ 
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
                }
            }
        }
        
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
