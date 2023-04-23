using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public AudioManager audioManager;
    //private void Start()
    //{
    //    audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    //}
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            unPauseGame();
        }
    }

    void unPauseGame()
    {
        gameObject.SetActive(false);
        //AudioListener.pause = false;
    }

    private void OnEnable()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        Time.timeScale = 0;
        audioManager.Pause(3);
        //AudioListener.pause = true;
    }

    private void OnDisable()
    {
        audioManager.Resume(3);
        Time.timeScale = 1;
    }
}
