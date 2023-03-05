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

    private void Start()
    {
        playerCamera.gameObject.SetActive(true);
        for (int i = 0; i < enemyCameras.Length; i++)
        {
            enemyCameras[i].gameObject.SetActive(false);
        }
        
        data.playerCam.Add(System.DateTime.Now.ToString());
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
                data.playerCam.Add(System.DateTime.Now.ToString());
                data.levelCam.Add(System.DateTime.Now.ToString());
                playerCamera.gameObject.SetActive(false);
                enemyCameras[closestEnemyIndex].gameObject.SetActive(true);

                isPlayerCamera = false;
            }
            else
            {
                playerCamera.gameObject.SetActive(true);
                for (int i = 0; i < enemyCameras.Length; i++)
                {
                    if (enemyCameras[i] == null)
                    {
                        continue;
                    }
                    enemyCameras[i].gameObject.SetActive(false);
                }
                data.playerCam.Add(System.DateTime.Now.ToString());
                data.levelCam.Add(System.DateTime.Now.ToString());
                isPlayerCamera = true;
            }
        }
    }
}