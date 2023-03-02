using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class workingOffLinkScript : MonoBehaviour
{
    void OnEnable()
    {
        StartCoroutine("waitToDie");
    }

    IEnumerator waitToDie()
    {
        yield return new WaitForSeconds(2.0f);
        gameObject.GetComponent<OffMeshLink>().enabled = false;
        gameObject.GetComponent<workingOffLinkScript>().enabled = false;
    }
}
