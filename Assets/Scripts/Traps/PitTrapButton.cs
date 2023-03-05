using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitTrapButton : MonoBehaviour
{
    public PitTrap pitTrap;
    public GameObject buttonParent;
    public GameObject resetButton;
    public MeshCollider door;
    //backward direction
    float animDirection = -1f; 
    //forward direction
    float animDirectionFw = 1f; 

    public void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == 3)
        {
            if (pitTrap.trapActive)
            {
                //changing pit trap's trigger button's animation direction to forward
                buttonParent.GetComponent<Animation>()["buttonAnim"].speed = animDirectionFw;
                buttonParent.GetComponent<Animation>().Play("buttonAnim");

                pitTrap.playAnimation();
                pitTrap.trapActive = false;
                door.enabled = false;
                data.pitTrap.Add(0);

                // resetButton.transform.localScale += new Vector3(0, 0.63f, 0);
                //changing pit trap's reset trigger button's animation direction to backward

                if (resetButton)
                {
                    resetButton.GetComponent<Animation>()["buttonAnim"].speed = animDirection;
                    resetButton.GetComponent<Animation>().Play("buttonAnim");
                }


                //Destroy(gameObject);
            }
        }
    }
}