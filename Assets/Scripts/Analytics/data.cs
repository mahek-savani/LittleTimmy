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
    public static int healthRemaining = 0;
    public static int enemyHit = 0;
    public static System.DateTime ttrstart = System.DateTime.Now;
    public static int attempts=1;
    public static bool userLevelComplete = false;
    public static int NPCChase = 0;
    public static int NPCSuspicion = 0;
    //public static List<string> trapRedeployment = new List<string>();
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

    public static void checkUserLevelCompleted()
    {
        /*
        if (data.userLevelComplete)
        {
            dataPerUser d1 = new dataPerUser();
            RestClient.Post("https://littletimmy-23966-default-rtdb.firebaseio.com/userData.json", d1);
        }
        */

        dataPerUser d1 = new dataPerUser();
        RestClient.Post("https://littletimmy-23966-default-rtdb.firebaseio.com/userData.json", d1);
    }

}

public class dataPerUser
{
    public int attempts;
    public string levelName;
    public bool userLevelComplete;
    public dataPerUser()
    {
        attempts = data.attempts;
        levelName = data.levelName;
        userLevelComplete = data.userLevelComplete;
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
    public int healthRemaining;
    public int enemyHit;
    public string ttrstart;
    public int NPCChase;
    public int NPCSuspicion;
    public List<string> trapRedeployment;
    public dataRes()
    {
        startTime = data.startTime.ToString();
        endTime = data.endTime.ToString();
        playerDeath = data.playerDeath;
        levelName = data.levelName;
        trapActiveOrder = data.trapActiveOrder;
        healthRemaining = data.healthRemaining;
        enemyHit = data.enemyHit;
        ttrstart = data.ttrstart.ToString();
        //enemyRemaining = data.enemyRemaining;
        NPCChase = data.NPCChase;
        NPCSuspicion = data.NPCSuspicion;
        //trapRedeployment = data.trapRedeployment;
    }

}
