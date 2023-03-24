using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class safeClosetActivatev3 : MonoBehaviour
{
    public LiveCounter NPCLiveCounter;

    // If player enters the playerCollisionBox, change the player layer from player to safePlayer
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            other.gameObject.layer = 15;
        }
        // When enemy is chasing the player into safe closet, automatically changes chase state to suspicious state
        NPCLiveCounter.stopChase();
    }

    // If player exist the playerCollisionBox, change the player layer from safePlayer to player
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer==15)
        {
            other.gameObject.layer = 3;
        }
    }
}
