using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    GameCard card;
    // Start is called before the first frame update
    void Start()
    {
        card = GetComponent<GameCard>();
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (collision.gameObject.layer == 6)
        {
            if (GameCard.mouseUp && card.simulated && (collision.CompareTag("Resource") || collision.CompareTag("Villager") || collision.CompareTag("Coin")) && collision.gameObject != card.child && collision.gameObject != card.parent)
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
