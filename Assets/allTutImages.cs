using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class allTutImages : MonoBehaviour
{
    public Sprite sprite;
    private PopUpSystem pop;
    public PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        pop = this.GetComponent<PopUpSystem>();
    }

    void OnTriggerEnter(Collider triggerObject){
        if(triggerObject.gameObject.layer == LayerMask.NameToLayer("Player") ){
            playerController.canPause = false;
            if (pop) pop.PopUpImage(sprite);
        }
    }
}
