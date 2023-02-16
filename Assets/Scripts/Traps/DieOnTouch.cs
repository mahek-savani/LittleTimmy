using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DieOnTouch : MonoBehaviour
{
    public PitTrap pitTrap;

    void OnTriggerStay(Collider c)
    {
        //Debug.Log("It's activating");
        if (!pitTrap.trapActive && c.gameObject.layer == 7)
        {
            //Debug.Log("Enenmy on pit");
            //Scene scene = SceneManager.GetActiveScene();
            c.GetComponent<StateMachine_Robust>().die();

            // if (scene.name == "tutorialPitTrap")
            // {
            //     c.GetComponent<StateMachine_Robust>().dieIdle();
            // }
            // else
            // {
            //     c.GetComponent<StateMachine_Robust>().die();
            // }
        }
    }
    
}
