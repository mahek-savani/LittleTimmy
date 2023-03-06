using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class workingOffLinkScript : MonoBehaviour
{
    public float dieTime = 2.0f;
    void OnEnable()
    {
        StartCoroutine("waitToDie");
    }

    IEnumerator waitToDie()
    {
        yield return new WaitForSeconds(dieTime);
        gameObject.GetComponent<OffMeshLink>().enabled = false;
        gameObject.GetComponent<workingOffLinkScript>().enabled = false;
    }
}
