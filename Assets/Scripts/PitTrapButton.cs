using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitTrapButton : MonoBehaviour
{
    public PitTrap pitTrap;
    public GameObject buttonParent;
    public MeshCollider door;
    public void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == 3)
        {
            if (pitTrap.trapActive)
            {
                buttonParent.GetComponent<Animation>().Play("buttonAnim");
                pitTrap.playAnimation();
                pitTrap.trapActive = false;
                door.enabled = false;
                //Destroy(gameObject);
            }
        }
    }
}
