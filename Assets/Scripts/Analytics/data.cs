using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;

public class data
{
    public static System.DateTime startTime = System.DateTime.Now;
    public static System.DateTime endTime = System.DateTime.Now;
    public static string playerDeath = "no";
    public static bool gameCompleted = false;
    public static string levelName = "demo";
    public static List<string> trapActiveOrder = new List<string>();
    public static void checkGameCompleted(bool checkComplete)
    {
        //Debug.Log(gameCompleted);
        if (checkComplete is true)
        {
            dataRes d = new dataRes();
            RestClient.Post("https://littletimmy-23966-default-rtdb.firebaseio.com/data.json", d);
            //Debug.Log(d.enemyRemaining);
            //gameCompleted = false;
        }
    }

}

public class dataRes
{
    public string startTime;
    public string endTime;
    public string playerDeath;
    //public int enemyRemaining;
    public string levelName;
    public List<string> trapActiveOrder;
    public dataRes()
    {
        startTime = data.startTime.ToString();
        endTime = data.endTime.ToString();
        playerDeath = data.playerDeath;
        levelName = data.levelName;
        trapActiveOrder = data.trapActiveOrder;
        //enemyRemaining = data.enemyRemaining;
    }

}