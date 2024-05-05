using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Moon : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] TextMeshProUGUI moonText, titleText, detailedText, feedTitleText, feedDetailedText, feedButtonText, buttonText;
    [SerializeField] Button botton;
    [SerializeField] Sprite normalImage, stopImage, fastImage;
    [SerializeField] Image stateImage;
    float totalTime, currentTime, timer, speed;
    [SerializeField] GameObject feedUI, otherUI;
    GameObject[] allObjects;

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
            moonText.text = "Moon " + GameManager.instance.moonCount;
            currentTime = 0;
            GameManager.instance.currentState = GameManager.STATE.Stop;
            SoundManager.instance.Playmoon();
            GameManager.instance.duringFeed = true;
            FreezeCards();
            feedTitleText.text = "End Of Moon " + GameManager.instance.moonCount;
            feedDetailedText.text = "Time to eat";
            feedButtonText.text = "Feed Villagers";
            InvokeRepeating("ButtonColorChange", 0, 0.3f);
            feedUI.SetActive(true);
            otherUI.SetActive(false);
        }
        else
        {
            GameManager.instance.duringFeed = false;
            UnFreezeCards();
            feedUI.SetActive(false);
            otherUI.SetActive(true);
        }
    }

    void ButtonColorChange()
    {
        Color white = new Color(1, 0.9764706f, 0.8901961f);
        Color yellow = new Color(1, 0.9607844f, 0.8117648f);
        if (botton.image.color == white)
        {
            botton.image.color = yellow;
        }
        else
        {
            botton.image.color = white;
        }
    }

    //change slideBar
    void SlideBar()
    {
        timer += Time.deltaTime;
        if (timer >= 0.1f)
        {
            currentTime += speed * GameManager.instance.gameSpeed;
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

    public void ChangeButtonFont()
    {
        buttonText.fontStyle = FontStyles.Bold | FontStyles.Underline;
    }

    public void ChangeButtonBack()
    {
        buttonText.fontStyle = FontStyles.Bold;
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

    void FreezeCards()
    {
        allObjects = FindObjectsOfType<GameObject>();

        // Iterate over each GameObject
        foreach (GameObject obj in allObjects)
        {
            if (obj.layer == 6)
            {
                obj.GetComponent<Collider2D>().enabled = false;
            }
        }
    }

    void UnFreezeCards()
    {
        // Iterate over each GameObject
        foreach (GameObject obj in allObjects)
        {
            if (obj.layer == 6)
            {
                obj.GetComponent<Collider2D>().enabled = true;
            }
        }
    }
}
