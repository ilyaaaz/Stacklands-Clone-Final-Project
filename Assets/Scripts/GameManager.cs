using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public STATE currentState;
    [SerializeField] TextMeshProUGUI titleText, detailedText, foodText, storageText, coinsText, ideaText, feedTitleText, feedDetailedText, feedButtonText;
    [SerializeField] GameObject corpse, otherUI;
    public GameObject processBar;
    [HideInInspector] public List<GameObject> people, foods;
    public int cardNum, coinNum, foodNum;
    public float gameSpeed;
    public PolygonCollider2D leftEdgeCollider, rightEdgeCollider, topEdgeCollider, bottomEdgeCollider;

    int buttonState, deathIndex; //0 feed Villager, 1 uh oh, 2 start new Moon, 3 gameover.

    int maxStorage;

    public GameObject currentCard;

    public List<GameObject> ideasObj;
    public List<GameCard> ideas;

    [HideInInspector] public int moonCount;

    [HideInInspector] public List<string> ideasFound;

    [HideInInspector] public bool duringFeed;
    void Start()
    {
        duringFeed = false;
        moonCount = 1;
        buttonState = 0;
        people = new List<GameObject>();
        coinNum = 0;
        foodNum = 0;
        cardNum = 0;
        maxStorage = 20;
        instance = this;
        currentState = STATE.Normal;
        currentCard = null;
        for (int i = 0; i < ideasObj.Count; i++)
        {
            ideas.Add(ideasObj[i].GetComponent<GameCard>());
        }
    }

    public enum STATE
    {
        Normal,
        Stop,
        Fast,
    }

    // Update is called once per frame
    void Update()
    {
        //print(currentCard);
        StateUpdate();
        //StorageUpdate();
        if (currentCard != null)
        {
            currentCard.transform.position = GetClampedPosition(currentCard.transform.position);
        }
    }

    void StateUpdate()
    {
        if (currentState == STATE.Normal)
        {
            otherUI.SetActive(true);
            gameSpeed = 1f;
        }
        else if (currentState == STATE.Fast)
        {
            gameSpeed = 2f;
        }
        else if (currentState == STATE.Stop)
        {
            gameSpeed = 0;
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
            }
            else
            {
                currentState = STATE.Normal;
            }
        }
    }
    //gameboard edge detection
    public Vector3 GetClampedPosition(Vector3 position)
    {
        float minX = leftEdgeCollider.bounds.max.x;
        float maxX = rightEdgeCollider.bounds.min.x;
        float minY = bottomEdgeCollider.bounds.max.y;
        float maxY = topEdgeCollider.bounds.min.y;

        float clampedX = Mathf.Clamp(position.x, minX, maxX);
        float clampedY = Mathf.Clamp(position.y, minY, maxY);

        return new Vector3(clampedX, clampedY, position.z);
    }
    
    //update food item
    public void FoodUpdate()
    {
        foodText.text = foodNum + "/" + people.Count * 2;
        if (foodNum < people.Count * 2)
        {
            InvokeRepeating("FoodTextColorChange", 0, 0.5f);
        }
        else
        {
            CancelInvoke("FoodTextColorChange");
            foodText.color = Color.black;
        }
    }

    //change food text each 0.5s
    void FoodTextColorChange()
    {
        if (foodText.color == Color.black)
        {
            foodText.color = Color.red;
        }
        else
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
        topCard.GetComponent<Collider2D>().enabled = false;
        //top.transform.position = bot.transform.position + Vector3.down * 0.3f;
        top.GetComponent<SpriteRenderer>().sortingOrder = bot.GetComponent<SpriteRenderer>().sortingOrder + 1;
        topCard.isStack = true;
        botCard.isStack = true;
        if (botCard.child == null && botCard.parent != top)
        {
            botCard.child = top;
            botCard.childCard = top.GetComponent<GameCard>();
        }

        if (topCard.parent == null && topCard.child != bot)
        {
            topCard.parent = bot;
            topCard.parentCard = botCard.GetComponent<GameCard>();
        }
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

    void ProcessBarCheck(GameObject bot)
    {
        if (bot.CompareTag("Structure") && bot.GetComponent<Product>().times > 0)
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

    public void ProcessBarCreateWithProduct(GameObject bot, GameObject product, float time, List<GameObject> stack)
    {
        GameObject newBar = Instantiate(processBar);
        newBar.transform.SetParent(bot.transform);
        newBar.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        Process barProcess = newBar.GetComponent<Process>();
        barProcess.totalTime = time;
        barProcess.product = product;
        for (int i = 0; i < stack.Count; i++)
        {
            barProcess.deleteList.Add(stack[i]);
        }
    }

    public void ProcessBarCreateWithProduct(GameObject bot, GameObject product, float time)
    {
        GameObject newBar = Instantiate(processBar);
        newBar.transform.SetParent(bot.transform);
        newBar.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        Process barProcess = newBar.GetComponent<Process>();
        barProcess.totalTime = time;
        barProcess.product = product;
        barProcess.deleteList.Add(bot.GetComponent<GameCard>().child);
    }

    public void ideasFoundCheck()
    {
        string sentence = "";
        for (int i = 0; i < ideasFound.Count; i++)
        {
            sentence += "*" + ideasFound[i] + "\n";
        }
        ideaText.text = sentence;
    }

    public void moonButtonClick()
    {
        if (buttonState == 0)
        {
            FeedVillager();
        }
        else if (buttonState == 1)
        {
            VillagerStarve();
        } else if (buttonState == 2)
        {
            currentState = STATE.Normal;
        } else if (buttonState == 3)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void FeedVillager()
    {
        feedDetailedText.text = "";
        feedButtonText.text = "Eating...";
        bool everyoneFeed = true;
        //for each villager
        for (int i = 0; i < people.Count; i++)
        {
            print(foods.Count);
            int eatNum = 0;
            //if no food left
            if (foods.Count == 0)
            {
                everyoneFeed = false;
                VillagerStarveText();
                deathIndex = i;
                break;
            }
            else
            {
                for (int j = 0; j < foods.Count; j++)
                {
                    int foodNum = foods[j].GetComponent<Food>().foodPoint;
                    if (foodNum > 0)
                    {
                        FoodUpdate();
                        //if food is large than 2
                        if (foodNum > 2)
                        {
                            foods[j].GetComponent<Food>().foodPoint -= 2;
                            break;
                        }
                        //if food is less or equal to 2
                        if (foodNum <= 2)
                        {
                            Destroy(foods[j]);
                            cardNum--;
                            eatNum += foodNum;
                            if (eatNum == 2)
                            {
                                break;
                            }
                        }
                    }
                }
                if (eatNum < 2)
                {
                    everyoneFeed = false;
                    VillagerStarveText();
                    deathIndex = i;
                    break;
                }
            }
        }

        if (everyoneFeed)
        {
            print("aaaaa");
            StartNewMoonText();
        }
    }

    void VillagerStarveText()
    {
        feedDetailedText.text = "There is not enough Food.. " + (people.Count - deathIndex) + " Villager will starve of Hunger";
        feedButtonText.text = "Uh oh";
        buttonState = 1;
    }

    void VillagerStarve()
    {
        
        while (deathIndex != people.Count)
        {
            GameObject deadBody = Instantiate(corpse);
            deadBody.transform.position = people[deathIndex].transform.position;
            Destroy(people[deathIndex]);
            cardNum--;
            people.RemoveAt(deathIndex);
        }
        if (people.Count != 0)
        {
            StartNewMoonText();
        } else
        {
            GameEnd();
        }
    }

    void StartNewMoonText()
    {
        moonCount++;
        feedTitleText.text = "START OF MOON " + moonCount;
        feedDetailedText.text = "";
        feedButtonText.text = "Start New Moon";
        buttonState = 2;
    }

    void GameEnd()
    {
        feedDetailedText.text = "Everyone starved to death!";
        feedButtonText.text = "Game Over (Restart)";
        buttonState = 3;
    }
}
    
