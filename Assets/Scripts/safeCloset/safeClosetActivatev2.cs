using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Experimental.GlobalIllumination;

public class safeClosetActivatev2 : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerController player;
    public GameObject playerObject;
    public bool playerAwake = true;
    public LiveCounter enemyCounter;
    public GameObject pos;
    private bool check = true;
    private Light pointLight;
    private float lightRange;

    private void disablePlayer()
    {
        pointLight = playerObject.gameObject.GetComponentInChildren<Light>();

        playerObject.GetComponent<MeshRenderer>().enabled = playerAwake;
        if(playerAwake == false)
        {
            lightRange = pointLight.range;
            pointLight.range = 0;
            playerObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
            player.speed = 0;
        }
        else
        {
            pointLight.range = lightRange;
            playerObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            playerObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            player.speed = 10;
        }
        playerObject.GetComponent<CapsuleCollider>().enabled = playerAwake;

    }


    // UI
    public TextMeshProUGUI helpText;

    private void Update()
    {
        
        if (playerAwake == false)
        {
            disablePlayer();
            //playerObject.SetActive(playerAwake);
            enemyCounter.stopChase();
            playerAwake = true;
            check = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Debug.Log("key press");
            if (check == false)
            {
                disablePlayer();
                //playerObject.SetActive(playerAwake);
                playerObject.transform.position = pos.transform.position;
                check = true;
                //playerAwake = false;
                if (helpText) helpText.text = "";
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
            if(helpText) helpText.text = "Press Space to exit the closet";
            check = true;
            playerAwake = false;
        }
    }
}
