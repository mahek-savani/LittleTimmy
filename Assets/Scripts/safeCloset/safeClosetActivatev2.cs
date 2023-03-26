using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class safeClosetActivatev2 : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerController player;
    public GameObject playerObject;
    public bool playerAwake = true;
    public LiveCounter enemyCounter;
    public GameObject pos;
    private bool check = false;
    private void Update()
    {
        
        if (playerAwake == false)
        {
            playerObject.SetActive(playerAwake);
            enemyCounter.stopChase();
            playerAwake = true;
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            //Debug.Log("key press");
            if (check == true)
            {
                playerObject.SetActive(playerAwake);
                playerObject.transform.position = pos.transform.position;
                check = false;

            }
            //playerObject.transform.position = pos;
            
            //Debug.Log(playerObject);
            //Debug.Log(playerAwake);

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            check = true;
            playerAwake = false;
        }
    }
}
