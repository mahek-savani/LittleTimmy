using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SelectLevel : MonoBehaviour
{
    public int level = 1;
    public TextMeshProUGUI levelText;

    void Start(){
        levelText.text = level.ToString();
    }

    public void LoadLevel(){
        SceneManager.LoadScene(level);
    }
}
