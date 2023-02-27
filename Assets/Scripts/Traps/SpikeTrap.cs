using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public bool trapActive = true;
    public HitBoxSize hitboxsize;
    public GameObject hitBox;
    //forward direction
    float animDirection = 1.0f;
    // Start is called before the first frame update
    // public Vector3 originalscale = HitBox.transform.localScale;
    // public Vector3 newscale;

    // public Vector3 originalpos = HitBox.transform.position;
    // public Vector3 newpos;

    private void Update()
    {
        if (gameObject.GetComponent<Animation>().isPlaying)
        {
            hitBox.SetActive(true);
        }
        else
        {
            hitBox.SetActive(false);
        }
    }

    public void playAnimation()
    {
        //changing spike trap's animation direction to forward
        gameObject.GetComponent<Animation>()["Spike Tutorial Hallway Anim"].speed = animDirection;
        gameObject.GetComponent<Animation>().Play("Spike Tutorial Hallway Anim");
        //hitboxsize.ChangeHitBoxSize();

        //BUGFIX: Change the hitbox size so that player doesnt take damage from behind the spikes


    }


}
