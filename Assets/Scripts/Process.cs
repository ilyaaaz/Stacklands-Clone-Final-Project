using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Process : MonoBehaviour
{
    [SerializeField] Slider slider;
    Resource resource;
    float totalTime, currentTime, timer, speed;

    // Start is called before the first frame update
    void Start()
    {
        resource = transform.parent.GetComponent<Resource>();
        totalTime = 10f;
        currentTime = 0f;
        speed = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        //SlideBar();
    }

    void SlideBar()
    {
        timer += Time.deltaTime;

        if (timer >= 0.1f)
        {
            currentTime += speed;
            timer = 0f;
        }
        
        slider.value = currentTime / totalTime;
        if (currentTime >= totalTime)
        {
            currentTime = 0f;
            if (resource.times > 0)
            {
                resource.createMaterial();
            }
        }
    }
}