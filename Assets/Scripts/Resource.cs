using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    GameCard card;
    // Start is called before the first frame update
    void Start()
    {
        card = GetComponent<GameCard>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        card.isColliding = false;
        card.currentState = GameCard.STATE.NoCard;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        /*
        if (!collision.CompareTag("Pack"))
        {
            card.isColliding = true;
            if (card.currentState == GameCard.STATE.NoCard)
            {

            }
            else if (card.currentState == GameCard.STATE.CardDrag)
            {

            }
            else if (card.currentState == GameCard.STATE.CardRelease)
            {
                if (collision.CompareTag("Resource") || collision.CompareTag("Villager"))
                {
                    GameManager.instance.StackCard(gameObject, collision.gameObject);
                    card.currentState = GameCard.STATE.NoCard;
                } else
                {
                    if (!card.isStack)
                    {
                        GameManager.instance.SeparateCard(gameObject, collision.gameObject);
                    }
                }
            }
        }
        */
        
        if (!collision.CompareTag("Pack") && !collision.CompareTag("Shop"))
        {
            if (GameCard.mouseUp && card.simulated && (collision.CompareTag("Resource") || collision.CompareTag("Villager")))
            {
                GameManager.instance.StackCard(gameObject, collision.gameObject);
                GameCard.mouseUp = false;
            }
            else
            {
                if (!card.isStack && !GameCard.mouseHold)
                {
                    GameManager.instance.SeparateCard(gameObject, collision.gameObject);
                }
            }
        }
        
    }

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if same
        if (collision.name == name && !card.isStack)
        {
            GameManager.instance.StackCard(gameObject, collision.gameObject);
        }
    }
    */
}
