using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Corpse : MonoBehaviour
{
    GameCard card;
    bool hasAdd;

    // Start is called before the first frame update
    void Start()
    {
        hasAdd = false;
    }

    private void Update()
    {
        if (!GameManager.instance.duringFeed && !hasAdd)
        {
            gameObject.AddComponent<GameCard>();
            card = GetComponent<GameCard>();
            hasAdd = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            if (GameCard.mouseUp && card.simulated && !collision.CompareTag("Structure"))
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