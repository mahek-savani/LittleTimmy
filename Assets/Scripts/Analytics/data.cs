using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
public class data
{
    //time class start
    public static System.DateTime startTime = System.DateTime.Now;
    public static System.DateTime endTime = System.DateTime.Now;
    public static System.DateTime ttrstart = System.DateTime.Now;
    //time class end
    //data class start
    public static string playerDeath = "no";
    public static bool gameCompleted = false;
    public static string levelName = "demo";
    //public static List<string> trapActiveOrder = new List<string>();
    public static int healthRemaining = 0;
    public static int healthLost = 0;
    public static int enemyHit = 0;
    public static int attempts=1;
    public static bool userLevelComplete = false;
    public static int NPCChase = 0;
    public static int NPCSuspicion = 0;
    //data class end
    //trapActive class start
    public static List<int> spikeTrap = new List<int>();
    public static List<int> pitTrap = new List<int>();
    public static List<int> noiseTrap = new List<int>();
    public static List<int> freezeTrap = new List<int>();
    public static List<int> spikes = new List<int>();
    public static bool uploadData = true;
    //trapActive class end
    //camera class start
    public static List<string> playerCam = new List<string>();
    public static List<string> levelCam = new List<string>();
    //camera class end
    //public static List<string> trapRedeployment = new List<string>();
    public static void checkGameCompleted(bool checkComplete)
    {
        //Debug.Log(gameCompleted);
        if (checkComplete is true)
        {
            dataRes d = new dataRes();
            trapActive trap = new trapActive();
            camera c = new camera();
            TimeCheck t = new TimeCheck();
            RestClient.Post("https://littletimmy-23966-default-rtdb.firebaseio.com/data.json", d);
            RestClient.Post("https://littletimmy-23966-default-rtdb.firebaseio.com/trapActive.json", trap);
            RestClient.Post("https://littletimmy-23966-default-rtdb.firebaseio.com/cams.json", c);
            RestClient.Post("https://littletimmy-23966-default-rtdb.firebaseio.com/time.json", t);
            //Debug.Log(d.enemyRemaining);
            //gameCompleted = false;
        }
        
    }

    public static void addTrapVal(string temp)
    {
        if (temp == "freezeTrap")
        {
            data.freezeTrap.Add(0);
        }
        else
        {
            data.noiseTrap.Add(0);
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
        //string d2 = JsonUtility.ToJson(d1);
        RestClient.Post("https://littletimmy-23966-default-rtdb.firebaseio.com/userData.json", d1);
        
    }

    public static void playerResetData()
    {
        data.startTime = System.DateTime.Now;
        data.endTime = System.DateTime.Now;
        data.ttrstart = System.DateTime.Now;

        data.playerDeath = "no";
        data.gameCompleted = false;
        data.levelName = "demo";
        data.healthRemaining = 0;
        data.healthLost = 0;
        data.enemyHit = 0;
        data.NPCChase = 0;
        data.NPCSuspicion = 0;

        data.spikeTrap = new List<int>();
        data.pitTrap = new List<int>();
        data.noiseTrap = new List<int>();
        data.freezeTrap = new List<int>();
        data.spikes = new List<int>();
        data.uploadData = true;

        data.playerCam = new List<string>();
        data.levelCam = new List<string>();
    }
    public static void EndZoneResetData()
    {
        data.startTime = System.DateTime.Now;
        data.endTime = System.DateTime.Now;
        data.ttrstart = System.DateTime.Now;
        
        data.playerDeath = "no";
        data.gameCompleted = false;
        data.levelName = "demo";
        data.healthRemaining = 0;
        data.healthLost = 0;
        data.enemyHit = 0;
        data.attempts = 1;
        data.userLevelComplete = false;        
        data.NPCChase = 0;
        data.NPCSuspicion = 0;

        data.spikeTrap = new List<int>();
        data.pitTrap = new List<int>();
        data.noiseTrap = new List<int>();
        data.freezeTrap = new List<int>();
        data.spikes = new List<int>();
        data.uploadData = true;

        data.playerCam = new List<string>();
        data.levelCam = new List<string>();
    }
    public static void checkData()
    {
        data.endTime = System.DateTime.Now;
        data.gameCompleted = true;
        //Debug.Log(data.gameCompleted);
        
        //Debug.Log(data.levelName);
        data.playerCam.Add(System.DateTime.Now.ToString());
        data.levelCam.Add(System.DateTime.Now.ToString());
        data.checkGameCompleted(data.gameCompleted);
        if(data.playerDeath == "no")
        {
            EndZoneResetData();
        }
        else
        {
            playerResetData();
        }
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

public class TimeCheck
{
    public string startTime;
    public string endTime;
    public string ttrTime;
    public TimeCheck()
    {
        startTime = data.startTime.ToString();
        endTime = data.endTime.ToString();
        ttrTime = data.ttrstart.ToString();
    }
}

public class trapActive
{
    public List<int> spikeTrap;
    public List<int> pitTrap;
    public List<int> noiseTrap;
    public List<int> freezeTrap;
    //public List<int> spikes;
    public bool uploadData;

    public trapActive()
    {
        spikeTrap = data.spikeTrap;
        pitTrap = data.pitTrap;
        noiseTrap = data.noiseTrap;
        freezeTrap = data.freezeTrap;
        uploadData = data.uploadData;
    }
}
public class camera
{
    public List<string> playerCam;
    public List<string> levelCam;

    public camera()
    {
        playerCam = data.playerCam;
        levelCam = data.levelCam;
    }
}
public class dataRes
{
    public string playerDeath;
    public string levelName;
    public List<string> trapActiveOrder;
    public int healthRemaining;
    public int healthLost;
    public int enemyHit;
    public int NPCChase;
    public int NPCSuspicion;
    public dataRes()
    {
        
        playerDeath = data.playerDeath;
        levelName = data.levelName;
        //trapActiveOrder = data.trapActiveOrder;
        healthRemaining = data.healthRemaining;
        healthLost = data.healthLost;
        enemyHit = data.enemyHit;
        TimeCheck time1 = new TimeCheck();
        //enemyRemaining = data.enemyRemaining;
        NPCChase = data.NPCChase;
        NPCSuspicion = data.NPCSuspicion;
        //camera = JsonUtility.ToJson(data.camera);
        //trapRedeployment = data.trapRedeployment;
    }

}
