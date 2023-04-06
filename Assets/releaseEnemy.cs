using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class releaseEnemy : MonoBehaviour
{
    public GameObject gate;
    //public GameObject npcFake;
    //public GameObject npcReal;
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == 3)
        {
            gate.SetActive(false);
            //npcFake.SetActive(false);
            //npcReal.SetActive(true);
        }
    }
}
