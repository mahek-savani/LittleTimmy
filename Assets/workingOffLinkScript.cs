using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class workingOffLinkScript : MonoBehaviour
{
    public float dieTime = 2.0f;
    public Transform enemyTransform;
    public Transform startLink;
    public Transform endLink;
    public Transform floor;
    void OnEnable()
    {
        StartCoroutine("waitToDie");
    }

    private void Update()
    {
        startLink.SetPositionAndRotation(new Vector3(enemyTransform.position.x, floor.position.y,
enemyTransform.position.z), enemyTransform.rotation);
    }

    IEnumerator waitToDie()
    {
        yield return new WaitForSeconds(dieTime);
        gameObject.GetComponent<OffMeshLink>().enabled = false;
        gameObject.GetComponent<workingOffLinkScript>().enabled = false;
    }
}
