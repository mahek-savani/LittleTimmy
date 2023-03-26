using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseTrap_Collision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionStay(Collision other)
    {
        if (other.transform.root.CompareTag("Enemies"))
        {
            transform.GetComponent<Renderer>().material.color = Color.grey;
        }
    }
}
