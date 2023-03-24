using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    public PlayerController PC;
    private void OnEnable()
    {
        Time.timeScale = 0;
        PC.canPause = false;
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
        PC.canPause = true;
    }
}
