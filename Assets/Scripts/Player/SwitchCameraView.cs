using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class SwitchCameraView : MonoBehaviour
{
    public CinemachineVirtualCamera playerCamera;
    public CinemachineVirtualCamera endZoneCamera;
    public CinemachineVirtualCamera spikeTrapCamera;
    public CinemachineVirtualCamera pitTrapCamera;
    public CinemachineVirtualCamera resetButtonCamera;
    public CinemachineVirtualCamera levelCamera;
    public Vector3 playerCameraOffset = new Vector3(0f, 50f, -30f);
    public float switchTime = 5f;
    public Transform playerTransform;
    private bool isPlayerCamera = false;
    public GameObject SwitchCamFill;
    public bool isPlayerInCloset = false;

    private Quaternion playerCamRotation;
    private bool panEndZone = false;
    private bool panSpikeTrap = false;
    private bool panPitTrap = false;
    private bool panResetButton = false;

    private void Start()
    {
        playerCamRotation = new Quaternion(playerCamera.transform.rotation.x, playerCamera.transform.rotation.y,
            playerCamera.transform.rotation.z, playerCamera.transform.rotation.w);
        // Debug.Log(playerCamera.transform.rotation);
        levelCamera.gameObject.SetActive(true);
        endZoneCamera.gameObject.SetActive(false);
        string sceneName = SceneManager.GetActiveScene().name;
        if(sceneName == "Level 1 Pit Trap Tutorial")
        {
            pitTrapCamera.gameObject.SetActive(false);
        }
        if(sceneName == "Level 2 Spike Trap Tutorial")
        {
            spikeTrapCamera.gameObject.SetActive(false);
        }
        if(sceneName == "Level 3 Trap Resets")
        {
            resetButtonCamera.gameObject.SetActive(false);
        }
        
        playerCamera.gameObject.SetActive(false);
        
        data.levelCam.Add(System.DateTime.Now.ToString());
    }
    private void Update()
    {
        playerCamera.transform.position = playerTransform.position + playerCameraOffset;
        playerCamera.transform.rotation = playerCamRotation;
        // Debug.Log(playerCamera.transform.rotation);
        if (Input.GetKeyDown("c"))
        {
            if (isPlayerCamera)
            {
                data.levelCam.Add(System.DateTime.Now.ToString());
                data.playerCam.Add(System.DateTime.Now.ToString());
                
                SwitchCamFill.GetComponent<CameraIconColor>().SetColor(1,1,1,1); //Change the color of icon to show that it is inactive
               
                playerCamera.gameObject.SetActive(false);
                levelCamera.gameObject.SetActive(true);

                isPlayerCamera = false;
            }
            else
            {   
                SwitchCamFill.GetComponent<CameraIconColor>().SetColor(0,1,0,1); //Change the color of icon to show that it is active

                playerCamera.gameObject.SetActive(true);
                levelCamera.gameObject.SetActive(false);
                data.levelCam.Add(System.DateTime.Now.ToString());
                data.playerCam.Add(System.DateTime.Now.ToString());
                isPlayerCamera = true;
            }
        }
        if(panEndZone)
        {
            panEndZone = false;
            playerCamera.gameObject.SetActive(false);
            levelCamera.gameObject.SetActive(false);
            string sceneName = SceneManager.GetActiveScene().name;
            if(sceneName == "Level 1 Pit Trap Tutorial")
            {
                StartCoroutine(WaitForPitTrap());
            }
            else
            {
                endZoneCamera.gameObject.SetActive(true);
            }
            playerTransform.gameObject.GetComponent<PlayerController>().pausePlayer();
            StartCoroutine(EndZoneView());
        }
        if(panSpikeTrap)
        {
            panSpikeTrap = false;
            playerCamera.gameObject.SetActive(false);
            levelCamera.gameObject.SetActive(false);
            spikeTrapCamera.gameObject.SetActive(true);
            playerTransform.gameObject.GetComponent<PlayerController>().pausePlayer();
            StartCoroutine(SpikeTrapView());
        }
        if(panResetButton)
        {
            panResetButton = false;
            playerCamera.gameObject.SetActive(false);
            levelCamera.gameObject.SetActive(false);
            resetButtonCamera.gameObject.SetActive(true);
            playerTransform.gameObject.GetComponent<PlayerController>().pausePlayer();
            StartCoroutine(ResetButtonButton());
        }
        if(panPitTrap)
        {
            panPitTrap = false;
            playerCamera.gameObject.SetActive(false);
            levelCamera.gameObject.SetActive(false);
            pitTrapCamera.gameObject.SetActive(true);
            playerTransform.gameObject.GetComponent<PlayerController>().pausePlayer();
            StartCoroutine(PitTrapView());
        }
    }

    public void SetPanEndZone(bool value)
    {
        panEndZone = value;
    }

    public void SetPanSpikeTrap(bool value)
    {
        panSpikeTrap = value;
    }

    public void SetPanResetButton(bool value)
    {
        panResetButton = value;
    }

    public void SetPanPitTrap(bool value)
    {
        panPitTrap = value;
    }

    private IEnumerator WaitForPitTrap()
    {
        yield return new WaitForSecondsRealtime(1f);
        endZoneCamera.gameObject.SetActive(true);
    }

    private IEnumerator EndZoneView()
    {
        yield return new WaitForSecondsRealtime(3f);
        endZoneCamera.gameObject.SetActive(false);
        //playerCamera.gameObject.SetActive(true);
        if(isPlayerCamera)
        {
            playerCamera.gameObject.SetActive(true);
        }
        else
        {
            levelCamera.gameObject.SetActive(true);
        }
        yield return new WaitForSecondsRealtime(2f);
        playerTransform.gameObject.GetComponent<PlayerController>().unPausePlayer();

    }

    private IEnumerator PitTrapView()
    {
        yield return new WaitForSecondsRealtime(3f);
        pitTrapCamera.gameObject.SetActive(false);
        //playerCamera.gameObject.SetActive(true);
        if(isPlayerCamera)
        {
            playerCamera.gameObject.SetActive(true);
        }
        else
        {
            levelCamera.gameObject.SetActive(true);
        }
        yield return new WaitForSecondsRealtime(2f);
        playerTransform.gameObject.GetComponent<PlayerController>().unPausePlayer();
    }

    private IEnumerator SpikeTrapView()
    {
        yield return new WaitForSecondsRealtime(5f);
        spikeTrapCamera.gameObject.SetActive(false);
        //playerCamera.gameObject.SetActive(true);
        if(isPlayerCamera)
        {
            playerCamera.gameObject.SetActive(true);
        }
        else
        {
            levelCamera.gameObject.SetActive(true);
        }
        yield return new WaitForSecondsRealtime(2f);
        playerTransform.gameObject.GetComponent<PlayerController>().unPausePlayer();
    }
    private IEnumerator ResetButtonButton()
    {
        yield return new WaitForSecondsRealtime(3f);
        //spikeTrapCamera.gameObject.SetActive(false);
        resetButtonCamera.gameObject.SetActive(false);
        //playerCamera.gameObject.SetActive(true);
        if(isPlayerCamera)
        {
            playerCamera.gameObject.SetActive(true);
        }
        else
        {
            levelCamera.gameObject.SetActive(true);
        }
        yield return new WaitForSecondsRealtime(2f);
        playerTransform.gameObject.GetComponent<PlayerController>().unPausePlayer();

    }
}