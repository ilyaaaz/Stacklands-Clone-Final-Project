using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

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

    public List<GameCard> ideas;
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
        StartCoroutine(topCard.lerpCard(top, bot.transform.position + Vector3.down * 0.3f));
        //top.transform.position = bot.transform.position + Vector3.down * 0.3f;
        top.GetComponent<SpriteRenderer>().sortingOrder = bot.GetComponent<SpriteRenderer>().sortingOrder + 1;
        topCard.isStack = true;
        botCard.isStack = true;
        botCard.child = top;
        botCard.childCard = top.GetComponent<GameCard>();
        topCard.parent = bot;
        topCard.parentCard = botCard.GetComponent<GameCard>();
        currentCard = null;
    }
    
    //separate card
    public void SeparateCard(GameObject top, GameObject bot)
    {
        Vector3 dir = top.transform.position - bot.transform.position;
        dir.Normalize();
        top.GetComponent<Rigidbody2D>().AddForce(dir * 4);
        bot.GetComponent<Rigidbody2D>().AddForce(-dir * 4);
        currentCard = null;
    }

    void ProcessBarCheck (GameObject bot) {
        if (bot.gameObject.layer == 6)
        {
            ProcessBarCreate(bot, 10f);
        }
    }

    public void ProcessBarCreate(GameObject bot, float time)
    {
        GameObject newBar = Instantiate(processBar);
        newBar.transform.SetParent(bot.transform);
        newBar.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        newBar.GetComponent<Process>().totalTime = time;
    }
}
    
