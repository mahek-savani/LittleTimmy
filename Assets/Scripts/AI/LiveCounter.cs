using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveCounter : MonoBehaviour
{
    private int totalLives;
    private int liveCounter;
    public bool paranoidMode = false;

    private void Start()
    {
        totalLives = transform.childCount;
        liveCounter = totalLives;
    }

    private void Update()
    {
        if (paranoidMode)
        {
            if (liveCounter <= totalLives / 2)
                {
                    StateMachine_Robust[] children = gameObject.GetComponentsInChildren<StateMachine_Robust>();

                    for (int i = 0; i < children.Length; i++)
                    {
                        if (children[i].alive)
                        {
                            if (children[i].state == children[i].defaultState)
                            {
                                children[i].getParanoid();
                            }

                            children[i].defaultState = StateMachine_Robust.STATE.PARANOID;
                        }
                    }

                    liveCounter = totalLives * 2;
                }
        }

    }

    public void decrement()
    {
        --liveCounter;
    }

    public int getNumLiving()
    {
        int numLiving = 0;
        StateMachine_Robust[] children = gameObject.GetComponentsInChildren<StateMachine_Robust>();

        for (int i = 0; i < children.Length; i++)
        {
            if (children[i].alive)
            {
                numLiving++;
            }
        }

        return numLiving;
    }

    public void cease()
    {
        StateMachine_Robust[] children = gameObject.GetComponentsInChildren<StateMachine_Robust>();

        for (int i = 0; i < children.Length; i++)
        {
            children[i].enabled = false;
        }
    }
}
