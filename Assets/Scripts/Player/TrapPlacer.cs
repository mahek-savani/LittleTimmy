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
    public Transform target;
    public LayerMask canBeTrapped;

    // 1 means TrapPlacer is the owner, 2 means PlayerController is
    public int eLocked = 2;

    public bool placedTrapBefore = false;

    // Trap pickup analytics logic; integrate after merge
    /*                 data.addTrapVal(player.trapInHand.gameObject.tag.ToString());
                trapTemp = true;*/

    // Update is called once per frame
    void LateUpdate()
    {
        if (player.hasTrapInInventory && (player.pickupDelay == 0 && !player.inSwapCommand))
        {
            trapInInventory = player.trapInHand;

            if (!placedTrapBefore)
            {
                player.helpUI.GetComponent<TextMeshProUGUI>().text = "[E] to place your trap on the floor!";
                placedTrapBefore = true;
                
            }

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
                Vector3 trapPosition = player.transform.position;
                //GameObject trapPlaced = Instantiate(trapInInventory, trapPosition, Quaternion.identity) as GameObject;
                GameObject trapPlaced = trapInInventory;
                //trapPlaced.layer = 8;
                trapPlaced.transform.position = trapPosition;

                trapPlaced.SetActive(true);
                trapPlaced.GetComponentInChildren<BaseTrapClass>().isTriggered = false;

                NoiseTrapActivation cloud = trapPlaced.GetComponentInChildren<NoiseTrapActivation>();
                //data.trapActiveOrder.Add("noiseTrap-0");
                //Debug.Log(trapPlaced);
                //Debug.Log(trapInInventory.gameObject.tag);
                //data.noiseTrap.Add(0);
                if (cloud != null)
                {
                    cloud.visible();
                }
                
                print("inside the function");

                // MeshRenderer[] kids = trapPlaced.GetComponentsInChildren<MeshRenderer>();
                // if (kids.Length == 2)
                // {
                //     kids[1].enabled = true;
                // }

                player.hasTrapInInventory = false;
                player.pickupDelay = 0f;

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
