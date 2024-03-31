using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public STATE currentState;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        currentState = STATE.Normal;
    }

    public enum STATE { 
        Normal,
        Stop,
        Fast
    }

    // Update is called once per frame
    void Update()
    {
        StateUpdate();
    }

    void StateUpdate()
    {
        if (currentState == STATE.Normal)
        {

        }
        else if (currentState == STATE.Fast)
        {

        }
        else if (currentState == STATE.Stop)
        {

        }
    }
}
