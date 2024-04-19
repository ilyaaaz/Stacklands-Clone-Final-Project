using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HideMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI hideMenuText, titleText, detailedText;
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

    public void ChangeFont()
    {  
        InvokeRepeating("TogglePannelFont", 0, Time.deltaTime); 
    }

    void TogglePannelFont()
    {
        hideMenuText.fontStyle = FontStyles.Bold | FontStyles.Underline;
        titleText.text = "TOGGLE PANNEL";
        detailedText.text = "Press this button or press [Q] to toggle the Quests and Ideas Tab"; 
    }

    public void ChangeBack()
    {
        hideMenuText.fontStyle = FontStyles.Bold;
        titleText.text = "";
        detailedText.text = ""; 
        CancelInvoke();
    }
}


