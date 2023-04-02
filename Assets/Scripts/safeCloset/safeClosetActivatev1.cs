using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class safeClosetActivatev1 : MonoBehaviour
{
    public PlayerController playerController;
    public GameObject playerObject;
    public bool playerAwake = true;
    public LiveCounter NPCLiveCounter;
    
    private void Update()
    {
        // On key press I, the player enter/exits the closet safe space if playerAwake(boolean) is False/True
        if (Input.GetKeyDown(KeyCode.I))
        {
            playerObject.SetActive(playerAwake);
            if(playerAwake == false)
            {
                NPCLiveCounter.stopChase();
                playerAwake = true;    
            }
        }
    }

    // If player tries to enter the safe closet space, Display text asking to initiate key press
    private void OnTriggerStay(Collider other)
    {
        // Set playerAwake boolean to false 
        if (other.gameObject.layer == 3)
        {
            playerController.helpUI.GetComponent<TextMeshProUGUI>().text = "Press [I] to Hide!!";
            playerAwake = false;            
        }
    }
}
