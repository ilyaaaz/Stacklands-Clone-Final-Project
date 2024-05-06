using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Soil : MonoBehaviour
{
    GameCard card;
    public List<GameObject> foodList = new List<GameObject>();
    public List<float> growTime = new List<float>();
    // Start is called before the first frame update

    private void Awake()
    {
        card = gameObject.GetComponent<GameCard>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        checkFood();
    }

    void checkFood()
    {
        if (card.child != null)
        {
            for (int i = 0; i < foodList.Count; i++)
            {
                if (card.child.name.Contains(foodList[i].name))
                {
                    GameManager.instance.ProcessBarCreateWithProduct(gameObject, foodList[i].GetComponent<Food>().foodSource, growTime[i]);
                }
            }
        }

    }
}
