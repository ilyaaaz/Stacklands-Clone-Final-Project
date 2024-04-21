using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public STATE currentState;
    [SerializeField] TextMeshProUGUI titleText, detailedText, foodText, storageText, coinsText;
    public GameObject processBar;
    public List<GameObject> people = new List<GameObject>();
    public int cardNum, coinNum;
    int foodNum, maxStorage;
    RaycastHit2D hit;

    public GameObject currentCard;

    void Start()
    {
        coinNum = 0;
        foodNum = 0;
        cardNum = 0;
        maxStorage = 20;
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
        //MouseCheck();
        //FoodUpdate();
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentState == STATE.Stop)
            {
                currentState = STATE.Normal;
            }
            else
            {
                currentState = STATE.Stop;
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (currentState == STATE.Normal)
            {
                currentState = STATE.Fast;
            } else
            {
                currentState = STATE.Normal;
            }
        }
    }

    //update food item
    public void FoodUpdate()
    {
        foodText.text = foodNum + "/" + people.Count * 2;
        if (foodNum < people.Count*2)
        {
            InvokeRepeating("FoodTextColorChange", 0, 0.5f);
        }
    }

    //change food text each 0.5s
    void FoodTextColorChange()
    {
        if (foodText.color == Color.black)
        {
            foodText.color = Color.red;
        } else
        {
            foodText.color = Color.black;
        }
    }

    //update storage item
    public void StorageUpdate()
    {
        storageText.text = cardNum + "/" + maxStorage;
    }

    //update coins item
    public void CoinUpdate()
    {
        coinsText.text = coinNum.ToString();
    }

    //stack card
    public void StackCard(GameObject top, GameObject bot)
    {
        GameCard topCard = top.GetComponent<GameCard>();
        GameCard botCard = bot.GetComponent<GameCard>();
        /*
        if (bot.layer == 6)
        {
            GameObject newBar = Instantiate(processBar);
            newBar.transform.parent = bot.transform;
            newBar.GetComponent<RectTransform>().anchoredPosition = new Vector2(bot.transform.position.x, bot.transform.position.y + 0.5f);
        }
        */
        top.transform.position = bot.transform.position + Vector3.down * 0.3f;
        //spr.sortingOrder = collision.transform.childCount;
        topCard.isStack = true;
        botCard.isStack = true;
        botCard.child = top;
        botCard.childCard = top.GetComponent<GameCard>();
    }
    
    //separate card
    public void SeparateCard(GameObject top, GameObject bot)
    {
        Vector3 dir = top.transform.position - bot.transform.position;
        top.GetComponent<Rigidbody2D>().AddForce(dir * 5);
        bot.GetComponent<Rigidbody2D>().AddForce(-dir * 5);
    }
}
    
