using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
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
        Time.timeScale = 0;
        //AudioListener.pause = true;
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }
}
