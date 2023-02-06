using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPlacer : MonoBehaviour
{
    public GameObject dropTrap;
    public GameObject noiseTrap;
    public PlayerController player;
    public Transform cams;
    public Transform target;
    public LayerMask canBeTrapped;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player.hasTrapInInventory){
            dropTrap = player.trapInHand;
            //Wall traps
            if(Input.GetKeyDown(KeyCode.R)){
                RaycastHit Hit;
                if(Physics.Raycast(cams.position, cams.forward, out Hit, 1000f, canBeTrapped))
                {
                    GameObject trapPlaced = Instantiate(dropTrap, Hit.point + Hit.normal * .001f, Quaternion.identity) as GameObject;
                    trapPlaced.transform.LookAt(Hit.point + Hit.normal);
                    trapPlaced.layer = 8;

                    trapPlaced.SetActive(true);
                    player.hasTrapInInventory = false;
                    player.pickupDelay = 1f;
                }
            }

            //Floor traps
            if(Input.GetKeyDown(KeyCode.F)){
                Vector3 trapPosition = player.transform.position;
                GameObject trapPlaced = Instantiate(dropTrap, trapPosition, Quaternion.identity) as GameObject;
                trapPlaced.layer = 8;

                trapPlaced.SetActive(true);
                player.hasTrapInInventory = false;
                player.pickupDelay = 1f;
                // trapPlaced.transform.LookAt(target.right);
            }

            //Noise traps
            if(Input.GetKeyDown(KeyCode.N)){
                Vector3 trapPosition = player.transform.position;
                GameObject trapPlaced = Instantiate(noiseTrap, trapPosition, Quaternion.identity) as GameObject;
                trapPlaced.layer = 8;

                trapPlaced.SetActive(true);
                player.hasTrapInInventory = false;
                player.pickupDelay = 1f;
            }
        }
        
    }
}
