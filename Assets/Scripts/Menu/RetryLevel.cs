using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryLevel : MonoBehaviour
{
    public void onClick()
    {
        data.attempts = data.attempts + 1;
        foreach (GameObject obj in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            Destroy(obj);
        }
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }
}
