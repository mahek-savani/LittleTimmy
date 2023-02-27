using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxSize : MonoBehaviour
{
    // Start is called before the first frame update4

    public Vector3 originalposition;
    public Vector3 originalscale;

    public Vector3 newscale;
    public Vector3 newposition;

    void Start()
    {
        originalposition = transform.position;
        originalscale = transform.localScale;
    }

    public void ChangeHitBoxSize()
    {
            newposition = originalposition;
            newposition.x = newposition.x -3.23f;  //Hardcorded values
            transform.position = newposition;

            newscale = originalscale;
            newscale.z = newscale.z - 5.67f;        
            transform.localScale = newscale;
    }

    public void ResetHitBoxSize()
    {   
            newposition = transform.position;
            newposition.x = newposition.x +3.23f;    //Hardcoded values
            transform.position = newposition;

            newscale = transform.localScale;
            newscale.z = newscale.z +5.67f;
            transform.localScale = newscale;
    }

    
}
