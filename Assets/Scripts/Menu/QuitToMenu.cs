using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitToMenu : MonoBehaviour
{
    public void onClick()
    {
        data.gameCompleted = false;
        data.levelName = SceneManager.GetActiveScene().name;
        data.checkUserLevelCompleted();
        data.EndZoneResetData();
        SceneManager.LoadScene(0);
    }
}
