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

    public bool placedTrapBefore = false;

    // Update is called once per frame
    void Update()
    {
        if(player.hasTrapInInventory && (player.pickupDelay == 0 && !player.inSwapCommand)){
            trapInInventory = player.trapInHand;

            if(!placedTrapBefore){
                player.helpUI.GetComponent<TextMeshProUGUI>().text = "[E] to place your trap on the floor!";
                placedTrapBefore = true;
            }

            //Wall traps
            if(Input.GetKeyDown(KeyCode.R)){
                RaycastHit Hit;
                if(Physics.Raycast(cams.position, cams.forward, out Hit, 1000f, canBeTrapped))
                {
                    //GameObject trapPlaced = Instantiate(trapInInventory, Hit.point + Hit.normal * .001f, Quaternion.identity) as GameObject;
                    GameObject trapPlaced = trapInInventory;
                    trapPlaced.transform.position = (Hit.point + Hit.normal * .001f);
                    trapPlaced.transform.LookAt(Hit.point + Hit.normal);
                    trapPlaced.layer = 8;

                    trapPlaced.SetActive(true);
                    player.hasTrapInInventory = false;
                    player.pickupDelay = 1f;
                }
            }

            //Floor traps
            if(Input.GetKeyDown(KeyCode.E)){
                Vector3 trapPosition = player.transform.position;
                //GameObject trapPlaced = Instantiate(trapInInventory, trapPosition, Quaternion.identity) as GameObject;
                GameObject trapPlaced = trapInInventory;
                //trapPlaced.layer = 8;
                trapPlaced.transform.position = trapPosition;

                trapPlaced.SetActive(true);
                trapPlaced.GetComponentInChildren<BaseTrapClass>().isTriggered = false;

                NoiseTrapActivation cloud = trapPlaced.GetComponentInChildren<NoiseTrapActivation>();

                if (cloud != null)
                {
                    cloud.visible();
                }

                // MeshRenderer[] kids = trapPlaced.GetComponentsInChildren<MeshRenderer>();
                // if (kids.Length == 2)
                // {
                //     kids[1].enabled = true;
                // }

                player.hasTrapInInventory = false;
                player.pickupDelay = 1f;

                // trapPlaced.transform.LookAt(target.right);
            }

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
        
    }
}
