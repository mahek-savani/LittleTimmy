using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DieOnTouch : MonoBehaviour
{
    public PitTrap pitTrap;
    public PlayerController playercontroller;
    private int check = 0;

    void OnTriggerStay(Collider c)
    {
        //Debug.Log("It's activating");
        if (!pitTrap.trapActive && c.gameObject.layer == 7)
        {
            //Debug.Log("Enenmy on pit");
            //Scene scene = SceneManager.GetActiveScene();
            
            c.GetComponent<StateMachine_Robust>().die();
            if (check == 0)
            {
                data.pitTrap.Add(1);
                check = 1;
            }
            // if (scene.name == "tutorialPitTrap")
            // {
            //     c.GetComponent<StateMachine_Robust>().dieIdle();
            // }
            // else
            // {
            //     c.GetComponent<StateMachine_Robust>().die();
            // }
        }
        
        if (!pitTrap.trapActive && c.gameObject.layer == 3)
        {
            PlayerDamage damageInterface = c.gameObject.GetComponent<PlayerDamage>();

            damageInterface.TakeDamage(1);
            
            playercontroller.RespawnPlayer();
            
        }
    }
    
}
