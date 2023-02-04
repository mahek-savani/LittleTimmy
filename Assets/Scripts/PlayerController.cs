using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody pbody;
    public float speed = 15f;

    public bool hasTrapInInventory;
    public GameObject trapInHand;

    void Start()
    {
         hasTrapInInventory = false;
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
    }

        void OnTriggerEnter(Collider triggerObject){
        if(!hasTrapInInventory && triggerObject.gameObject.layer == LayerMask.NameToLayer("Pickup")){
                trapInHand = triggerObject.gameObject;
                triggerObject.gameObject.SetActive(false);
                hasTrapInInventory = true;
        }
    }

    void OnTriggerStay(Collider triggerObject){

        if(hasTrapInInventory){
            if(Input.GetKeyDown("e") && triggerObject.gameObject.layer == LayerMask.NameToLayer("Pickup")){ 
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
            }
        }
    }
}
