using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class workingOffLinkScript : MonoBehaviour
{
    public float dieTime = 2.0f;
    // public Transform enemyTransform;
    public List<Transform> enemyTransforms = new List<Transform>();
    public Transform startLink;
    public Transform endLink;
    public Transform floor;
    void OnEnable()
    {
        StartCoroutine("waitToDie");
    }

    private void Update()
    {
        for(int i=0; i<enemyTransforms.Count; i++)
        {
            if(enemyTransforms[i] && startLink)
            {
                startLink.SetPositionAndRotation(new Vector3(enemyTransforms[i].position.x, floor.position.y,
enemyTransforms[i].position.z), enemyTransforms[i].rotation);
            }
        }
    }

    IEnumerator waitToDie()
    {
        yield return new WaitForSeconds(dieTime);
        OffMeshLink[] offMeshLinks = GetComponents<OffMeshLink>();
        // Debug.Log(offMeshLinks.Length);
        foreach (OffMeshLink offMeshLink in offMeshLinks)
        {
            Destroy(offMeshLink);
        }
        gameObject.GetComponent<workingOffLinkScript>().enabled = false;
    }
}
