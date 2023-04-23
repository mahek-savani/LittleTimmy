using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PressedPlay(){
        //Debug.Log("Pressed Play");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PressedLevelSelect(){
        SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 1);
    }

    public void PressedCredits()
    {
        SceneManager.LoadScene(10);
    }

    public void PressedBack(){
        SceneManager.LoadScene(0);
    }
}
