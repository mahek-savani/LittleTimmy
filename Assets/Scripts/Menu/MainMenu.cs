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
}
