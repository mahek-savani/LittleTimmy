using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.UI;

public class CameraIconColor : MonoBehaviour
{

  public void SetColor(int r, int g, int b, int a)
  {
    GetComponent<Image>().color = new Color(r,g,b,a);
  }
  
}
