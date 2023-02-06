using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTrapViewpoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.RightArrow))
        {
            Vector3 newRotation = new Vector3(0, 90, 0);
            transform.eulerAngles = newRotation;
        }
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            Vector3 newRotation = new Vector3(0, 270, 0);
            transform.eulerAngles = newRotation;
        }
        if(Input.GetKey(KeyCode.DownArrow))
        {
            Vector3 newRotation = new Vector3(0, 180, 0);
            transform.eulerAngles = newRotation;
        }
        if(Input.GetKey(KeyCode.UpArrow))
        {
            Vector3 newRotation = new Vector3(0, 0, 0);
            transform.eulerAngles = newRotation;
        }
    }

}
