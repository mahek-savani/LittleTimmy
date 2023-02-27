using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitToMenu : MonoBehaviour
{
    void resetData()
    {
        data.startTime = System.DateTime.Now;
        data.endTime = System.DateTime.Now;
        data.playerDeath = "no";
        data.levelName = "demo";
        data.gameCompleted = false;
        data.trapActiveOrder = new List<string>();
        data.healthRemaining = 0;
        data.enemyHit = 0;
        data.ttrstart = System.DateTime.Now;
        data.userLevelComplete = false;
        data.attempts = 1;
        data.NPCChase = 0;
        data.NPCSuspicion = 0;
    }
    public void onClick()
    {
        data.gameCompleted = false;
        data.levelName = SceneManager.GetActiveScene().name;
        data.checkUserLevelCompleted();
        resetData();
        SceneManager.LoadScene(0);
    }
}
