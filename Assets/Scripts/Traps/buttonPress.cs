using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonPress : MonoBehaviour
{
    public SpikeTrap spikeTrap;
    public Spike spike;
    public GameObject spikeGrid;
    public Transform orientation;
    public GameObject buttonParent;
    public GameObject spikeResetButton;
    public GameObject spikeTrapWorking;
    public GameObject smoke;
    public LocalAudioManager localAudioManager;
    public LocalAudioManager spikeslocalAudioManager;
    //backward direction
    float animDirection = -1f; 
    //forward direction
    float animDirectionFw = 1f; 
    public float movementSpeed = 30f;

    private bool switchView = false;

    public void Update(){
        if (spike.isTrapMoving){
            spikeGrid.transform.position += orientation.forward * Time.deltaTime * movementSpeed;
        }
        if(spikeTrap.trapActive){
            if(smoke) smoke.SetActive(false);
        }
    }

    public static event System.Action SpikeTrapButtonPushed;
    private Color startingMaterialColor;

    void Start(){
        spikeTrap.originalPos = spikeGrid.transform.position;
        startingMaterialColor = buttonParent.GetComponent<Renderer>().material.color;
    }

    public void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == 3)
        {
            if (spikeTrap.trapActive)
            {
                // Play Spike Release Sound
                //FindObjectOfType<AudioManager>().Play("SpikeActiveSound");

                if(localAudioManager)
                {
                    // audioManager.Play(name: "ButtonPress", loop: false);
                     localAudioManager.Play(name: "ButtonPress", channel: 3, loop: false, volume: 0.2f);
                     
                }

                if(spikeslocalAudioManager)
                {
                    spikeslocalAudioManager.Play(name: "SpikeActiveSound", channel: 1, loop: false, volume: 0.3f);
                }

                spike.isTrapMoving = true;
                //changing spike trap's trigger button's animation direction to forward
                buttonParent.GetComponent<Animation>()["buttonAnim"].speed = animDirectionFw;
                buttonParent.GetComponent<Animation>().Play("buttonAnim");

                // spikeTrap.playAnimation();
                // spikeResetButton.transform.localScale += new Vector3(0, 0.63f, 0);
                //data.trapActiveOrder.Add("spikeTrap-0");
                data.spikeTrap.Add(0);
                if (spikeResetButton)
                {
                    //changing spike trap's reset trigger button's animation direction to backward
                    spikeResetButton.GetComponent<Animation>()["buttonAnim"].speed = animDirection;
                    spikeResetButton.GetComponent<Animation>().Play("buttonAnim");
                    spikeResetButton.GetComponent<Renderer>().material.color = startingMaterialColor;
                }

                spikeTrap.trapActive = false;
                //Destroy(gameObject);
                if(SpikeTrapButtonPushed != null) SpikeTrapButtonPushed();
                buttonParent.GetComponent<Renderer>().material.color = Color.grey;
                spikeTrapWorking.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.grey;

                string sceneName = SceneManager.GetActiveScene().name;
                if(sceneName == "Level 2 Spike Trap Tutorial")
                {
                    if (!switchView)
                    {
                        switchView = true;
                        SwitchCameraView cameraView = FindObjectOfType<SwitchCameraView>();
                        if (cameraView != null)
                        {
                            cameraView.SetPanSpikeTrap(true);
                        }
                    }
                }
            }
        }
    }
}
