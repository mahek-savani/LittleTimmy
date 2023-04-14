using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FlipOnBlackBG : MonoBehaviour
{
    public TextMeshProUGUI HelpText;

    // Update is called once per frame
    void Update()
    {
        Color ColorChange = new Color(0, 0, 0);
        if(HelpText.text == "") {
            ColorChange.a = 0f;
        }
        else {
            ColorChange.a = 0.51f;
        }
        this.gameObject.GetComponent<Image>().color = ColorChange;
    }
}
