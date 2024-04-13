using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;


public class GameCard : MonoBehaviour

{
    public int value = 0; //default value
    [HideInInspector] public Vector3 startPos;
    Collider2D cld;
    Rigidbody2D rb;
    SpriteRenderer spr;

    private void Awake()
    {
        startPos = new Vector3(-7.85f, 2.85f, 0); //default value
        cld = GetComponent<Collider2D>();
        spr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        print(startPos);
        StartCoroutine(lerpCard(startPos));
        ItemsUpdate();
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
        cld.isTrigger = true;
        spr.sortingOrder = 1;
        
    }

    private void OnMouseUp()
    {
        cld.isTrigger = false;
        spr.sortingOrder = 0;
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