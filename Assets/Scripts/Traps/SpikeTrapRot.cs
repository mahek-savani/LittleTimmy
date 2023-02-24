using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrapRot : MonoBehaviour
{
    public bool trapActive = true;
    // Start is called before the first frame update
    public void playAnimation()
    {
        Debug.Log("inside rot, anim called");
        gameObject.GetComponent<Animation>().Play("SpikeAnim2");
    }
}
