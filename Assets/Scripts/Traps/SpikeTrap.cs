using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public bool trapActive = true;
    // Start is called before the first frame update
    public void playAnimation()
    {
        gameObject.GetComponent<Animation>().Play();
    }
}
