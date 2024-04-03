using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public STATE currentState;
    // Start is called before the first frame update
    [SerializeField] TextMeshProUGUI text;

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
        MouseCheck();
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

    void MouseCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            
            if (hit.collider != null)
            {
                Debug.Log("Hit " + hit.collider.gameObject.name);
                if (hit.collider.gameObject.tag == "Moon")
                {
                    text.fontStyle = FontStyles.Bold | FontStyles.Underline;
                    
                }
            }
    }
}
