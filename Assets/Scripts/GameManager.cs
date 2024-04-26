using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public STATE currentState;
    [SerializeField] TextMeshProUGUI titleText, detailedText, foodText, storageText, coinsText;
    public GameObject processBar;
    public List<GameObject> people;
    public int cardNum, coinNum;
    int foodNum, maxStorage;
    RaycastHit2D hit;

    public GameObject currentCard;

    void Start()
    {
        people = new List<GameObject>();
        coinNum = 0;
        foodNum = 0;
        cardNum = 0;
        maxStorage = 20;
        instance = this;
        currentState = STATE.Normal;
        currentCard = null;
    }

    public enum STATE { 
        Normal,
        Stop,
        Fast
    }

    // Update is called once per frame
    void Update()
    {
        //print(currentCard);
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
        while (botCard.child != null)
        {
            bot = botCard.child;
            botCard = bot.GetComponent<GameCard>();
        }
        ProcessBarCheck(bot);
        top.transform.position = bot.transform.position + Vector3.down * 0.3f;
        top.GetComponent<SpriteRenderer>().sortingOrder = bot.GetComponent<SpriteRenderer>().sortingOrder + 1;
        topCard.isStack = true;
        botCard.isStack = true;
        botCard.child = top;
        botCard.childCard = top.GetComponent<GameCard>();
        //currentCard = null;
    }
    
    //separate card
    public void SeparateCard(GameObject top, GameObject bot)
    {
        Vector3 dir = top.transform.position - bot.transform.position;
        top.GetComponent<Rigidbody2D>().AddForce(dir * 5);
        bot.GetComponent<Rigidbody2D>().AddForce(-dir * 5);
        //currentCard = null;
    }

    void ProcessBarCheck (GameObject bot) {
        if (bot.layer == 6)
        {
            GameObject newBar = Instantiate(processBar);
            newBar.transform.SetParent(bot.transform);
            newBar.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
    }
}
    
