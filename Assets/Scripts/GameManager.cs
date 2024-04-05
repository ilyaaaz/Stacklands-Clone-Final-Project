using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public STATE currentState;
    [SerializeField] TextMeshProUGUI titleText, detailedText, foodText, storageText, coinsText;
    public List<GameObject> people = new List<GameObject>();
    public int cardNum, coinNum;
    int foodNum, maxStorage;
    RaycastHit2D hit;

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
        MouseCheck();
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

    void MouseCheck()
    {
        if (!Input.GetMouseButton(0))
        {
             hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        }
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Card"))
            {
                MouseOnCard(hit.collider);
            } 
        }
    }

    void MouseOnCard(Collider2D hit)
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            hit.gameObject.transform.position = new Vector3(mousePos.x, mousePos.y, 0);
            hit.isTrigger = true;
        } else
        {
            hit.isTrigger = false;
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
}
