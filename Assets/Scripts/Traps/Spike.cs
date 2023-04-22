using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;


public class Spike : MonoBehaviour
{
    public bool isTrapMoving = false;
    public bool isTrapMovingBack = false;
    private bool switchView = false;
    public NavMeshSurface navMesh;
    public SpikeTrap spikeTrap;

    void OnTriggerEnter(Collider c)
    {
        if(c.gameObject.layer == LayerMask.NameToLayer("breakableWall"))
        {
            // Debug.Log("collide");
            c.gameObject.SetActive(false);
            navMesh.BuildNavMesh();
        }

        if (c.gameObject.layer == LayerMask.NameToLayer("Enemies") && spikeTrap.trapActive == true)
        {
            //data.trapActiveOrder.Add("spikeTrap-1");
            data.spikeTrap.Add(1);
            StateMachine_Robust SM = c.gameObject.GetComponent<StateMachine_Robust>();

            SM.die();
        }

        if (c.gameObject.layer == LayerMask.NameToLayer("Player") && isTrapMoving)
        {
            PlayerDamage damageInterface = c.gameObject.GetComponent<PlayerDamage>();
            damageInterface.TakeDamage(1);
        }

        if (c.gameObject.layer == LayerMask.NameToLayer("spikeObstacle"))
        {
            isTrapMoving = false;
            //isTrapMovingBack = false;
            string sceneName = SceneManager.GetActiveScene().name;
            if(sceneName == "Level 3 Trap Resets")
            {
                if (!switchView)
                {
                    switchView = true;
                    SwitchCameraView cameraView = FindObjectOfType<SwitchCameraView>();
                    if (cameraView != null)
                    {
                        cameraView.SetPanResetButton(true);
                    }
                }
            }
        }
    }
}
