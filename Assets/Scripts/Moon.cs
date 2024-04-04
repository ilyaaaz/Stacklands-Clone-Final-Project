using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Moon : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] TextMeshProUGUI moonText, titleText, detailedText;
    [SerializeField] Sprite normalImage, stopImage, fastImage;
    [SerializeField] Image stateImage;
    float totalTime, currentTime, timer, speed;

    // Start is called before the first frame update
    void Start()
    {
        totalTime = 120f;
        currentTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        SlideBar();

        //text.text = slider.value.ToString();
        if (currentTime >= totalTime)
        {
            currentTime = 0;
            GameManager.instance.currentState = GameManager.STATE.Stop;
        }
    }
    
    //change slideBar
    void SlideBar()
    {
        timer += Time.deltaTime;
        if (timer >= 0.1f)
        {
            currentTime += speed;
            timer = 0f;
        }

        if (GameManager.instance.currentState == GameManager.STATE.Normal)
        {
            stateImage.sprite = normalImage;
            speed = 0.1f;
        }
        else if (GameManager.instance.currentState == GameManager.STATE.Fast)
        {
            stateImage.sprite = fastImage;
            speed = 0.2f;
        }
        else if (GameManager.instance.currentState == GameManager.STATE.Stop)
        {
            stateImage.sprite = stopImage;
            speed = 0;
        }

        slider.value = currentTime / totalTime;
    }

    public void ChangeFont()
    {  
        InvokeRepeating("RealTimeFont", 0, Time.deltaTime); 
    }

    void RealTimeFont()
    {
        moonText.fontStyle = FontStyles.Bold | FontStyles.Underline;
        titleText.text = "TIME";
        detailedText.text = "The current time. There's " + (totalTime - currentTime).ToString("0.0") + "s left in this moon. \n \n" + "Use [Space] to pause or use [Tab] to toggle between game speeds" ; 
    }

    public void ChangeBack()
    {
        moonText.fontStyle = FontStyles.Bold;
        titleText.text = "";
        detailedText.text = ""; 
        CancelInvoke();
    }

    public void MouseClick()
    {
        if (GameManager.instance.currentState == GameManager.STATE.Normal)
        {
            GameManager.instance.currentState = GameManager.STATE.Fast;
        }
        else if (GameManager.instance.currentState == GameManager.STATE.Fast)
        {
            GameManager.instance.currentState = GameManager.STATE.Stop;
        }
        else if (GameManager.instance.currentState == GameManager.STATE.Stop)
        {
            GameManager.instance.currentState = GameManager.STATE.Normal;
        }
    }
}
