using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrapPlacer : MonoBehaviour
{
    //public GameObject dropTrap;
    public GameObject noiseTrap;
    public GameObject trapInInventory;
    public PlayerController player;
    public Transform cams;
    // public Transform target;
    public LayerMask canBeTrapped;

    // 1 means TrapPlacer is the owner, 2 means PlayerController is
    public int eLocked = 2;

    public bool placedTrapBefore = false;

    public LocalAudioManager localAudioManager;

    // Trap pickup analytics logic; integrate after merge
    /*                 data.addTrapVal(player.trapInHand.gameObject.tag.ToString());
                trapTemp = true;*/

    // Update is called once per frame
    void LateUpdate()
    {
        if (player.hasTrapInInventory && (player.pickupDelay == 0 && !player.inSwapCommand))
        {
            trapInInventory = player.trapInHand;

            // Update the HelpUI if you have an item in your inventory + are navigating level
            // Should NOT update if the helpUI is already occupied with the closet interaction text
            if (player.helpUI.GetComponent<TextMeshProUGUI>().text == "") player.helpUI.GetComponent<TextMeshProUGUI>().text = "[E] to place your trap on the floor!";                

            // placedTrapBefore is used in PlayerController.OnTriggerStay to help trigger tutorials in Level 4 (Noise Trap Tutorial)
            // and Level 5 (Speedball/Freeze Trap Tutorial). Remove at your own risk.
            if(!placedTrapBefore) placedTrapBefore = true;

            //Wall traps
            if (Input.GetKeyDown(KeyCode.R))
            {
                RaycastHit Hit;
                if (Physics.Raycast(cams.position, cams.forward, out Hit, 1000f, canBeTrapped))
                {
                    //GameObject trapPlaced = Instantiate(trapInInventory, Hit.point + Hit.normal * .001f, Quaternion.identity) as GameObject;
                    GameObject trapPlaced = trapInInventory;
                    trapPlaced.transform.position = (Hit.point + Hit.normal * .001f);
                    trapPlaced.transform.LookAt(Hit.point + Hit.normal);
                    trapPlaced.layer = 8;

                    trapPlaced.SetActive(true);
                    player.hasTrapInInventory = false;
                    player.pickupDelay = 0f;
                }
            }

            //Floor traps
            if (Input.GetKeyDown(KeyCode.E) && eLocked == 1)
            {
                RaycastHit Hit;
                if (Physics.Raycast(cams.position, -cams.up, out Hit, 1000f, canBeTrapped))
                {
                    //GameObject trapPlaced = Instantiate(trapInInventory, Hit.point + Hit.normal * .001f, Quaternion.identity) as GameObject;
                    GameObject trapPlaced = trapInInventory;
                    float height = 0;
                    //Debug.Log(trapPlaced.ToString());
                    if(trapPlaced.CompareTag("noiseTrap"))
                    {
                        height = transform.position.y - Hit.point.y + 0.3f;
                    }
                    
                    trapPlaced.transform.position =  new Vector3(Hit.point.x + Hit.normal.x * .001f, Hit.point.y + Hit.normal.y * .001f + height, Hit.point.z + Hit.normal.z * .001f); 
                    
                    trapPlaced.transform.LookAt(cams.up);
                    // trapPlaced.transform.rotation = Quaternion.identity;
                    trapPlaced.transform.rotation = Hit.transform.rotation;
                    trapPlaced.layer = 8;

                    trapPlaced.SetActive(true);
                    trapPlaced.GetComponentInChildren<BaseTrapClass>().isTriggered = false;
                    player.hasTrapInInventory = false;
                    player.pickupDelay = 0f;
                    NoiseTrapActivation cloud = trapPlaced.GetComponentInChildren<NoiseTrapActivation>();
                    if (cloud != null)
                    {
                        cloud.visible();
                    }

                    if(localAudioManager)
                    {
                         localAudioManager.Play(name: "TrapPlace", channel: 3, loop: false, volume: 0.3f);
                    }
                }

                // Vector3 trapPosition = player.transform.position;
                //GameObject trapPlaced = Instantiate(trapInInventory, trapPosition, Quaternion.identity) as GameObject;
                // GameObject trapPlaced = trapInInventory;
                //trapPlaced.layer = 8;
                // trapPlaced.transform.position = trapPosition;

                // // trapPlaced.SetActive(true);
                // trapPlaced.GetComponentInChildren<BaseTrapClass>().isTriggered = false;

                // NoiseTrapActivation cloud = trapPlaced.GetComponentInChildren<NoiseTrapActivation>();
                //data.trapActiveOrder.Add("noiseTrap-0");
                //Debug.Log(trapPlaced);
                //Debug.Log(trapInInventory.gameObject.tag);
                // //data.noiseTrap.Add(0);
                // if (cloud != null)
                // {
                //     cloud.visible();
                // }

                // MeshRenderer[] kids = trapPlaced.GetComponentsInChildren<MeshRenderer>();
                // if (kids.Length == 2)
                // {
                //     kids[1].enabled = true;
                // }

                // player.hasTrapInInventory = false;
                // player.pickupDelay = 0f;

                data.addTrapVal(player.trapInHand.gameObject.tag.ToString());

                // trapPlaced.transform.LookAt(target.right);

                //StartCoroutine(unLockE());

                eLocked = 2;
            }

            //if (Input.GetKeyUp(KeyCode.E) && eLocked == 1)
            //{
            //    StartCoroutine(unLockE());
            //}

            // Noise traps
            // if(Input.GetKeyDown(KeyCode.E)){
            //     Vector3 trapPosition = player.transform.position;
            //     GameObject trapPlaced = Instantiate(trapInInventory, trapPosition, Quaternion.identity) as GameObject;
            //     trapPlaced.layer = 8;

            //     trapPlaced.SetActive(true);
            //     //trapPlaced.GetComponentInChildren<MeshRenderer>().enabled = true;

            //     //trapPlaced.GetComponent<MeshRenderer>().enabled = true;
            //     player.hasTrapInInventory = false;
            //     player.pickupDelay = 1f;
            // }
        }
        else if (player.hasTrapInInventory && (player.pickupDelay == 0 && player.inSwapCommand))
        {
            if (Input.GetKeyDown(KeyCode.E) && eLocked == 1)
            {
                player.swapTraps();
            }
        }


        //IEnumerator unLockE()
        //{
        //    yield return new WaitForEndOfFrame();
        //    eLocked = 0;
        //}
    }
}
