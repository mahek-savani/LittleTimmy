using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrapRot : MonoBehaviour
{
    public bool trapActive = true;
    //forward direction
    float animDirection = 1.0f; 
    // Start is called before the first frame update
    public void playAnimation()
    {
        //changing spike trap's animation direction to forward
        gameObject.GetComponent<Animation>()["SpikeAnim2"].speed = animDirection;
        gameObject.GetComponent<Animation>().Play("SpikeAnim2");
    }
}
