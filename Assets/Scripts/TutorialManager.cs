using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject trapSketchObject;
    public GameObject endZoneSketch;
    public GameObject navSketch;

    public bool trapSketch = false;
    public bool endTrapSketch = false;
    private int popUpIndex;
    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) ){
            navSketch.SetActive(false);
        }
        // trapSketchObject.SetActive(false);
        // endZoneSketch.SetActive(false);
    }
}
