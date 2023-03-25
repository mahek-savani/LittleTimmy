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
    public CinemachineVirtualCamera resetButtonCamera;
    public CinemachineVirtualCamera[] enemyCameras;
    public float switchTime = 5f;
    public Transform playerTransform;
    private bool isPlayerCamera = true;
    public GameObject SwitchCamFill;

    private bool panEndZone = false;
    private bool panSpikeTrap = false;
    private bool panResetButton = false;

    private void Start()
    {
        playerCamera.gameObject.SetActive(true);
        endZoneCamera.gameObject.SetActive(false);
        string sceneName = SceneManager.GetActiveScene().name;
        if(sceneName == "Level 2 Spike Trap Tutorial")
        {
            spikeTrapCamera.gameObject.SetActive(false);
        }
        if(sceneName == "Level 3 Trap Resets")
        {
            spikeTrapCamera.gameObject.SetActive(false);
            resetButtonCamera.gameObject.SetActive(false);
        }
        for (int i = 0; i < enemyCameras.Length; i++)
        {
            enemyCameras[i].gameObject.SetActive(false);
        }
        
        data.levelCam.Add(System.DateTime.Now.ToString());
    }
    private void Update()
    {
        if (Input.GetKeyDown("c"))
        {
            if (isPlayerCamera)
            {
                float closestDistance = float.MaxValue;
                int closestEnemyIndex = 0;
                for (int i = 0; i < enemyCameras.Length; i++)
                {
                    if (enemyCameras[i] == null)
                    {
                        continue;
                    }
                    float distance = Vector3.Distance(playerTransform.position, enemyCameras[i].transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestEnemyIndex = i;
                    }
                }
                //Debug.Log(closestEnemyIndex);
                data.levelCam.Add(System.DateTime.Now.ToString());
                data.playerCam.Add(System.DateTime.Now.ToString());
                
                SwitchCamFill.GetComponent<CameraIconColor>().SetColor(0,1,0,1); //Change the color of icon to show that it is active
               
                playerCamera.gameObject.SetActive(false);
                enemyCameras[closestEnemyIndex].gameObject.SetActive(true);

                isPlayerCamera = false;
            }
            else
            {   
                SwitchCamFill.GetComponent<CameraIconColor>().SetColor(1,1,1,1); //Change the color of icon to show that it is inactive

                playerCamera.gameObject.SetActive(true);
                for (int i = 0; i < enemyCameras.Length; i++)
                {
                    if (enemyCameras[i] == null)
                    {
                        continue;
                    }
                    enemyCameras[i].gameObject.SetActive(false);
                }
                data.levelCam.Add(System.DateTime.Now.ToString());
                data.playerCam.Add(System.DateTime.Now.ToString());
                isPlayerCamera = true;
            }
        }
        if(panEndZone)
        {
            panEndZone = false;
            playerCamera.gameObject.SetActive(false);
            endZoneCamera.gameObject.SetActive(true);
            StartCoroutine(EndZoneView());
        }
        if(panSpikeTrap)
        {
            panSpikeTrap = false;
            playerCamera.gameObject.SetActive(false);
            spikeTrapCamera.gameObject.SetActive(true);
            resetButtonCamera.gameObject.SetActive(false);
            StartCoroutine(SpikeTrapView());
        }
        if(panResetButton)
        {
            panResetButton = false;
            playerCamera.gameObject.SetActive(false);
            spikeTrapCamera.gameObject.SetActive(false);
            resetButtonCamera.gameObject.SetActive(true);
            StartCoroutine(ResetButtonButton());
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

    private IEnumerator EndZoneView()
    {
        yield return new WaitForSeconds(3f);
        endZoneCamera.gameObject.SetActive(false);
        playerCamera.gameObject.SetActive(true);
    }
    private IEnumerator SpikeTrapView()
    {
        yield return new WaitForSeconds(5f);
        spikeTrapCamera.gameObject.SetActive(false);
        resetButtonCamera.gameObject.SetActive(false);
        playerCamera.gameObject.SetActive(true);
    }
    private IEnumerator ResetButtonButton()
    {
        yield return new WaitForSeconds(3f);
        spikeTrapCamera.gameObject.SetActive(false);
        resetButtonCamera.gameObject.SetActive(false);
        playerCamera.gameObject.SetActive(true);
    }
}