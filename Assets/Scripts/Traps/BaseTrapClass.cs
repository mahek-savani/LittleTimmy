using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTrapClass : MonoBehaviour
{
    public string trapName;             // Used in UI to denote trap type
    public bool isTriggered = false;    // Used for all pick-ups to decide if they're triggered
}
