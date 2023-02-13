using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitTrapResetButton : MonoBehaviour
{
    public PitTrap pitTrap;
    public GameObject buttonParent;
    public MeshCollider door;
    public GameObject trapDoor;
    public void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == 3)
        {
            if (!pitTrap.trapActive)
            {
                buttonParent.GetComponent<Animation>().Play("buttonAnim");
                pitTrap.trapActive = true;
                door.enabled = true;
                trapDoor.transform.Rotate(0f, 0f, -85f, Space.Self);
            }
        }
    }
}
