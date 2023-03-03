using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SwitchCameraView : MonoBehaviour
{
    public CinemachineVirtualCamera playerCamera;
    public CinemachineVirtualCamera[] enemyCameras;
    public float switchTime = 5f;
    public Transform playerTransform;
    private bool isPlayerCamera = true;
    public GameObject SwitchCamFill;

    private void Start()
    {
        playerCamera.gameObject.SetActive(true);
        for (int i = 0; i < enemyCameras.Length; i++)
        {
            enemyCameras[i].gameObject.SetActive(false);
        }
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

                isPlayerCamera = true;
            }
        }
    }
}