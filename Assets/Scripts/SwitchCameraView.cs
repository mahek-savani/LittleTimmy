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
            Debug.Log(closestEnemyIndex);

            playerCamera.gameObject.SetActive(false);
            enemyCameras[closestEnemyIndex].gameObject.SetActive(true);
            
            Invoke("SwitchToPlayerCamera", switchTime);
        }
    }

    private void SwitchToPlayerCamera()
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
    }
}

