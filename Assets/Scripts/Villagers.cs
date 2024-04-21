using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Villagers : MonoBehaviour
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (GameCard.mouseUp && card.simulated)
        {
            GameManager.instance.StackCard(gameObject, collision.gameObject);
            GameCard.mouseUp = false;
        } else
        {
            if (!card.isStack && !GameCard.mouseHold)
            {
                GameManager.instance.SeparateCard(gameObject, collision.gameObject);
            }
        }
       
        //transform.parent = collision.transform;
        //spr.sortingOrder = collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder + 1;
    }
}
