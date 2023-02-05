using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPlacer : MonoBehaviour
{
    public GameObject dropTrap;
    public GameObject player;
    public Transform cams;
    public LayerMask canBeTrapped;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Wall traps
        if(Input.GetKeyDown(KeyCode.W)){
            RaycastHit Hit;
            if(Physics.Raycast(cams.position, cams.forward, out Hit, 1000f, canBeTrapped))
            {
                GameObject trapPlaced = Instantiate(dropTrap, Hit.point + Hit.normal * .001f, Quaternion.identity) as GameObject;
                trapPlaced.transform.LookAt(Hit.point + Hit.normal);
            }
        }

        //Floor traps
        if(Input.GetKeyDown(KeyCode.F)){
            Vector3 trapPosition = player.transform.position;
            GameObject trapPlaced = Instantiate(dropTrap, trapPosition, Quaternion.identity) as GameObject;
            trapPlaced.transform.LookAt(trapPosition);
        }
    }
}
