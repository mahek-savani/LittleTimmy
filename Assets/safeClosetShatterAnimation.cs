using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class safeClosetShatterAnimation : MonoBehaviour
{
    public GameObject safeCloset;
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Collapse");
        if (other.gameObject.CompareTag("Spike")){
            Debug.Log("Hi");
            safeCloset.GetComponent<Animation>().Play("safeClosetCollapse");
            safeCloset.SetActive(false);

        }
    }
}
