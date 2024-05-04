using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
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
        //print("structure " + gameObject.name + " " +  card.currentState.ToString());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //card.isColliding = false;
        //card.currentState = GameCard.STATE.NoCard;
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
                //only same name could stack
                if (collision.name == gameObject.name)
                {
                    GameManager.instance.StackCard(gameObject, collision.gameObject);
                   
                } else
                {
                    if (!card.isStack)
                    {
                        GameManager.instance.SeparateCard(gameObject, collision.gameObject);
                    }
                }
                card.currentState = GameCard.STATE.NoCard;
            }
        }
        */
        
        if (collision.gameObject.layer == 6)
        {
            if (GameCard.mouseUp && (collision.name == gameObject.name || collision.CompareTag("Resource")) && card.simulated && collision.gameObject != card.child && collision.gameObject != card.parent)
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
}
