using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise_Collision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        print("collision");
        if (collision.transform.root.CompareTag("Enemies"))
        {
            print("collision");
        }
    }
}
