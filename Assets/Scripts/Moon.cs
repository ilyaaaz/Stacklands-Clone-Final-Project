using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Moon : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] TextMeshProUGUI text;
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
        slideBar();
        //mouseCheck();


        //text.text = slider.value.ToString();
        if (currentTime >= totalTime)
        {
            currentTime = 0;
            GameManager.instance.currentState = GameManager.STATE.Stop;
        }
    }
    
    //change slideBar
    void slideBar()
    {
        timer += Time.deltaTime;
        if (timer >= 0.1f)
        {
            currentTime += speed;
            timer = 0f;
        }

        if (GameManager.instance.currentState == GameManager.STATE.Normal)
        {
            speed = 0.1f;
        }
        else if (GameManager.instance.currentState == GameManager.STATE.Fast)
        {
            speed = 0.2f;
        }
        else if (GameManager.instance.currentState == GameManager.STATE.Stop)
        {
            speed = 0;
        }

        slider.value = currentTime / totalTime;
    }

    //check mouse ********** maychange
    void mouseCheck()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // Check if the collider belongs to the object you're interested in
            if (hit.collider.gameObject == slider.gameObject)
            {
                text.fontStyle = FontStyles.Underline;
            } else
            {
                text.fontStyle = FontStyles.Bold;
            }
        } 
    }
}
