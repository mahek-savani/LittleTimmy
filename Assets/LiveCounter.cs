using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveCounter : MonoBehaviour
{
    private int totalLives;
    private int liveCounter;

    private void Start()
    {
        totalLives = transform.childCount;
        liveCounter = totalLives;
    }

    private void Update()
    {
        if (liveCounter <= totalLives / 2)
        {
            StateMachine_Robust[] children = gameObject.GetComponentsInChildren<StateMachine_Robust>();

            for (int i = 0; i < children.Length; i++)
            {
                if (children[i].alive)
                {
                    children[i].getParanoid();
                }
            }

            liveCounter = totalLives * 2;
        }
    }

    public void decrement()
    {
        --liveCounter;
    }
}
