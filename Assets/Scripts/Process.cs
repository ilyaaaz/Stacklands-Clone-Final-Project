using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Process : MonoBehaviour
{
    [SerializeField] Slider slider;
    Product resource;
    [HideInInspector] public float totalTime = 10f;
    float currentTime, timer, speed;
    [HideInInspector] public GameObject product;
    [HideInInspector] public List<GameObject> deleteList;

    // Start is called before the first frame update
    void Start()
    {
        resource = GetComponentInParent<Product>();
        currentTime = 0f;
        speed = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        SlideBar();
        if (GetComponentInParent<GameCard>().child == null)
        {
            Destroy(gameObject);
        }
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
            SoundManager.instance.PlayProduceCard();
            if (resource != null)
            {
                if (resource.times > 0)
                {
                    resource.createMaterial();
                }
            } else
            {
                if (product != null)
                {
                    createMaterial(product);
                }
            }
            
        }
    }
    void createMaterial(GameObject product)
    {
        GameObject newCard = Instantiate(product, transform.parent.position, Quaternion.identity);
        newCard.GetComponent<GameCard>().startPos = transform.parent.position + Vector3.up * 2f;
        for (int i = 0; i < deleteList.Count; i++)
        {
            Destroy(deleteList[i]);
        }
    }

}
