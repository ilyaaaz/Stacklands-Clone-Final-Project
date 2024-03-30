using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideMenu : MonoBehaviour
{
    bool closed;
    float hide_posX;
    RectTransform myRT;
    // Start is called before the first frame update
    void Start()
    {
        closed = false;
        hide_posX = -440f;
        myRT = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MenuButton()
    {
        if (!closed)
        {
            myRT.anchoredPosition = new Vector2(hide_posX, 0);
            closed = true;
        } else
        {
            myRT.anchoredPosition = Vector2.zero;
            closed = false;
        }
    }
}
