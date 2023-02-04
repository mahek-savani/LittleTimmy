using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;

public class data
{
    public static System.DateTime startTime = System.DateTime.Now;
    public static System.DateTime endTime = System.DateTime.Now;
    public static string playerDeath = "no";
    public static int enemyRemaining = 2;
    public static bool gameCompleted = false;
    public static int count = 0;
    public static void checkGameCompleted(bool checkComplete)
    {
        //Debug.Log(gameCompleted);
        if (checkComplete is true && count == 0)
        {

            dataRes d = new dataRes();
            //RestClient.Post("https://littletimmy-23966-default-rtdb.firebaseio.com/data.json", d);
            Debug.Log(d.enemyRemaining);
            gameCompleted = false;
            count = 1;
        }
    }

    /*public data()
    {
        playerDeath = "yes";
        //Debug.Log(playerDeath);
        //timeToComplete = StateMachine_Robust.enemyRemaining;
        //Debug.Log(timeToComplete);
    }*/

}

public class dataRes
{
    public string startTime;
    public string endTime;
    public string playerDeath;
    public int enemyRemaining;
    public dataRes()
    {
        startTime = data.startTime.ToString();
        endTime = data.endTime.ToString();
        playerDeath = data.playerDeath;
        enemyRemaining = data.enemyRemaining;
    }

}
