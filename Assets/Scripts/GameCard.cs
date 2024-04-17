using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;


public class GameCard : MonoBehaviour

{
    public int value = 0; //default value
    [HideInInspector] public Vector3 startPos;
    [SerializeField] TextMeshProUGUI titleText, detailedText, valueText;
    
    Collider2D cld;
    Rigidbody2D rb;
    SpriteRenderer spr;
    bool mouseUp;
    

    private void Awake()
    {
        startPos = new Vector3(-7.85f, 2.85f, 0); //default value
        cld = GetComponent<Collider2D>();
        //cld.isTrigger = true;
        spr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        titleText = GameObject.Find("TitleText (TMP)").GetComponent<TextMeshProUGUI>();
        detailedText = GameObject.Find("DetailedText (TMP)").GetComponent<TextMeshProUGUI>();
        valueText = GameObject.Find("ValueText (TMP)").GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        StartCoroutine(lerpCard(startPos));
        ItemsUpdate();
    }

    private void Update()
    {
        //if (Input.GetMouseButtonUp(0))
        //{
        //    Collider2D[] colliders = Physics2D.OverlapCollider(GetComponent<Collider2D>(),);
        //}
        
    }

    void ItemsUpdate()
    {
        if (CompareTag("Coin"))
        {
            GameManager.instance.coinNum++;
            GameManager.instance.CoinUpdate();
        }
        else if (CompareTag("Human"))
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

    private void OnMouseDrag()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePos.x, mousePos.y, 0);
        spr.sortingOrder = 100;
        GameManager.instance.currentCard = gameObject;
        mouseUp = false;
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
    }

    private void OnMouseExit()
    {
        titleText.text = "";
        detailedText.text = "";
    }

    
    private void OnMouseUp()
    {
        mouseUp = true;
        //cld.isTrigger = false;
        spr.sortingOrder = 0;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (mouseUp)
        {
            transform.position = collision.transform.position + Vector3.down * 0.3f;
            transform.parent = collision.transform;
            spr.sortingOrder = collision.transform.childCount;
            if (collision.gameObject.layer == 6)
            {
                GameObject newBar = Instantiate(GameManager.instance.processBar);
                newBar.transform.parent = collision.transform;
                newBar.GetComponent<RectTransform>().anchoredPosition = new Vector2(transform.position.x, collision.transform.position.y + 0.5f);
            }
            mouseUp = false;
        }
        //transform.parent = collision.transform;
        //spr.sortingOrder = collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder + 1;
    }


    IEnumerator lerpCard(Vector3 targetPos)
    {
        while (Vector3.Distance(transform.position, targetPos) > 0.1f)
        {
            yield return new WaitForSeconds(0.01f);
            transform.position = Vector3.Lerp(transform.position, targetPos, 0.1f);
        }
    }
}