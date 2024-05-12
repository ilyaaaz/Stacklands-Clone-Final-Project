using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using TMPro;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


public class GameCard : MonoBehaviour
{
    public int value = 0; //default value
    TextMeshProUGUI titleText, detailedText, valueText;
    Zoom gameCam;
    
    Collider2D cld;
    Rigidbody2D rb;
    SpriteRenderer spr;
    [HideInInspector] public bool isStack, simulated;
    [HideInInspector] public static bool mouseUp, mouseHold;
    [HideInInspector] public Vector3 startPos, parentOrigin;
    public GameObject child, parent;
    [HideInInspector] public GameCard parentCard, childCard;

    [Header("Idea Product")]
    public List<GameObject> materials = new List<GameObject>();
    public List<int> matchingNum = new List<int>();
    public int materialSize = 0;
    public float requireTime;

    public bool firstCheck;

    Vector3 offSet;

    //public STATE currentState;

    /*
    public enum STATE
    {
        NoCard,
        CardDrag,
        CardRelease
    }
    */
    private void Awake()
    {
        gameCam = GameObject.Find("Main Camera").GetComponent<Zoom>();
        startPos = new Vector3(-7.85f, 2.85f, 0); //default value
        cld = GetComponent<Collider2D>();
        spr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        titleText = GameObject.Find("TitleText (TMP)").GetComponent<TextMeshProUGUI>();
        detailedText = GameObject.Find("DetailedText (TMP)").GetComponent<TextMeshProUGUI>();
        valueText = GameObject.Find("ValueText (TMP)").GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        firstCheck = false;
        rb.mass = 0.4f;
        //currentState = STATE.NoCard;
        parent = null;
        child = null;
        //isColliding = false;
        cld.enabled = false;
        //cld.isTrigger = true;
        StartCoroutine(lerpCard(gameObject, startPos));
        ItemsUpdate();
    }

    private void Update()
    {
        
        if (GameManager.instance.currentCard == gameObject)
        {
            simulated = true;
        } else
        {
            simulated = false;
        }
        if (cld.enabled)
        {
            FollowParent();
        }
        if (child == null && parent == null)
        {
            isStack = false;
        }
        ColliderChange();
    }

    void ColliderChange()
    {
        if(child != null)
        {
            ((BoxCollider2D)cld).size = new Vector2(4.34f, 0.89f);
            ((BoxCollider2D)cld).offset = new Vector2(0f, 2.09f);
        }
        else
        {
             ((BoxCollider2D)cld).size = new Vector2(4.34f, 5.24f);
             ((BoxCollider2D)cld).offset = new Vector2(0f, 0f);
        }
    }

    void ItemsUpdate()
    {
        if (CompareTag("Coin"))
        {
            GameManager.instance.coinNum++;
            GameManager.instance.CoinUpdate();
        }
        else if (CompareTag("Villager"))
        {
            GameManager.instance.people.Add(gameObject);
            GameManager.instance.FoodUpdate();
            GameManager.instance.cardNum++;
            GameManager.instance.StorageUpdate();
        }
        else
        {
            GameManager.instance.cardNum++;
            GameManager.instance.StorageUpdate();
        }
    }

    private void OnMouseDown()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offSet = transform.position - mousePos;

        if (child == null && parent == null)
        {
            if (gameObject.CompareTag("Villager"))
            {
                SoundManager.instance.PlayVillagerCard();
            }
            else if (gameObject.CompareTag("Coin"))
            {
                SoundManager.instance.PlayCoinCard();
            }
            else
            {
                SoundManager.instance.PlayCardPickUp();
            }
        }
        else
        {
            SoundManager.instance.PlaystackCard();
        }

    }

    private void OnMouseDrag()
    {
        firstCheck = true;
        gameCam.enableDrag = false;
        //currentState = STATE.CardDrag;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //transform.position = new Vector3(mousePos.x, mousePos.y, 0);
        transform.position = new Vector3(mousePos.x + offSet.x, mousePos.y + offSet.y, 0);
        spr.sortingOrder = 100;
        GameManager.instance.currentCard = gameObject;
        mouseHold = true;
        mouseUp = false;
        //ChildFollow();
        if (parent != null)
        {
            parentCard.child = null;
            parentCard.childCard = null;
            parent = null;
        }
        //cld.isTrigger = true;
    }

    private void OnMouseOver()
    {
        
        if (gameObject.name == "Coin(Clone)")
        {
            titleText.text = "COIN";
            detailedText.text = "Humanity's best friend";
            valueText.text = "Can't be sold";
        }

        if (gameObject.name == "Villager(Clone)")
        {
            titleText.text = "VILLAGER";
            detailedText.text = "A hard-working pioneer";
            valueText.text = "Can't be sold";
        }

        if (gameObject.name == "Berry_Bush(Clone)")
        {
            titleText.text = "BERRY BUSH";
            detailedText.text = "A bush with delicious berries it";
            valueText.text = "1";
        }

        if (gameObject.name == "Rock(Clone)")
        {
            titleText.text = "ROCK";
            detailedText.text = "It looks sturdy, yet punchable";
            valueText.text = "";
        }

        if (gameObject.name == "Wood(Clone)")
        {
            titleText.text = "WOOD";
            detailedText.text = "A simple resource";
            valueText.text = "1";
        }
    }

    private void OnMouseExit()
    {
        titleText.text = "";
        detailedText.text = "";
        valueText.text = "";
        
    }


    private void OnMouseUp()
    {
        //cld.isTrigger = false;
        spr.sortingOrder = 0;
        if (gameObject.CompareTag("Villager"))
        {
            SoundManager.instance.PlayVillagerCard();
        }
        else if (gameObject.CompareTag("Coin"))
        {
            SoundManager.instance.PlayCoinCard();
        }
        else
        {
            SoundManager.instance.PlayCardDrop();
        }
        mouseUp = true;
        mouseHold = false;
        SortLayer();
        gameCam.enableDrag = true;
    }
    void FollowParent()
    {
        if (parent != null)
        {
            if (parentOrigin != parent.transform.position)
            {
                transform.position = parent.transform.position + Vector3.down * 0.3f;
                parentOrigin = parent.transform.position;
            }
            
            //childCard.ChildFollow();
            
            /*
            if (transform.position != originalPos)
            {
                StartCoroutine(lerpCard(child, transform.position + Vector3.down * 0.3f));
                childCard.ChildFollow();
            }
            */
        }
        if (child != null)
        {
            SortLayer();
        }
    }

    void SortLayer()
    {
        GameCard tempChild = childCard;

        while (tempChild != null)
        {
            tempChild.spr.sortingOrder = spr.sortingOrder + 1;
            
            tempChild = tempChild.childCard;
        }
    }

    public IEnumerator lerpCard(GameObject card, Vector3 targetPos)
    {
        while (Vector3.Distance(card.transform.position, targetPos) > 0.01f)
        {
            yield return new WaitForSeconds(0.01f);
            card.transform.position = Vector3.Lerp(card.transform.position, targetPos, 0.1f);
        }
        if (Vector3.Distance(card.transform.position, targetPos) <= 0.01f)
        {
            card.transform.position = targetPos;
            StopCoroutine(lerpCard(card, startPos));
            SetDefault();
        }
    }

    void SetDefault()
    {
        //cld.isTrigger = false;
        cld.enabled = true;
        rb.drag = 8;
        cld.isTrigger = true;
    }

    public void CardDestroy()
    {
        if (CompareTag("Coin"))
        {
            GameManager.instance.coinNum--;
            GameManager.instance.CoinUpdate();
        }
        else if (CompareTag("Villager"))
        {
            GameManager.instance.people.Remove(gameObject);
            GameManager.instance.FoodUpdate();
            GameManager.instance.cardNum--;
            GameManager.instance.StorageUpdate();
        }
        else if (CompareTag("Food"))
        {
            GameManager.instance.foodNum--;
            GameManager.instance.foods.Remove(gameObject);
            GameManager.instance.cardNum--;
            GameManager.instance.StorageUpdate();
        }
        else
        {
            GameManager.instance.cardNum--;
            GameManager.instance.StorageUpdate();
        }
        Destroy(gameObject);
    }
}