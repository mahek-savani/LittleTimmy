using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DieOnTouch : MonoBehaviour
{
    public PitTrap pitTrap;

    void OnTriggerEnter(Collider c)
    {
        //Debug.Log("It's activating");
        if (!pitTrap.trapActive && c.gameObject.layer == 7)
        {
            Debug.Log("Enenmy on pit");
            Scene scene = SceneManager.GetActiveScene();
            if (scene.name == "tutorialPitTrap")
            {
                c.GetComponent<StateMachine_Robust>().dieIdle();
            }
            else
            {
                c.GetComponent<StateMachine_Robust>().die();
            }
        }
    }
    
}
