using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class workingOffLinkScript : MonoBehaviour
{
    public float dieTime = 100.0f;
    public OffMeshLink link;
    public Transform myBody;
    private float floor;
    // public Transform enemyTransform;
    //public List<Transform> enemyTransforms = new List<Transform>();
    //public Transform startLink;
    //public Transform endLink;
    //public Transform floor;

    //private void Awake()
    //{
    //    link.startTransform = gameObject.AddComponent<Transform>();
    //    link.endTransform = gameObject.AddComponent<Transform>();
    //}

    void OnEnable()
    {
        link.enabled = true;
        StartCoroutine("waitToDie");
    }

    private void OnDisable()
    {
        link.enabled = false;
        StopCoroutine("waitToDie");
    }

    public void setLink(Vector3 endPos, Quaternion endRot, float floorHeight)
    {
        floor = floorHeight;
        link.startTransform.SetPositionAndRotation(new Vector3(myBody.position.x, floorHeight, myBody.position.z), myBody.rotation);
        link.endTransform.SetPositionAndRotation(endPos, endRot);
    }

    public void UpdateLink()
    {
        link.startTransform.SetPositionAndRotation(new Vector3(myBody.position.x, floor, myBody.position.z), myBody.rotation);
        link.UpdatePositions();
    }

    IEnumerator waitToDie()
    {
        yield return new WaitForSeconds(dieTime);
        gameObject.GetComponent<workingOffLinkScript>().enabled = false;
    }
}
