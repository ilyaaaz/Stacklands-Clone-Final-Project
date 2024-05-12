using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    GameCard card;
    public int foodPoint;
    public GameObject foodSource;
    // Start is called before the first frame update
    void Start()
    {
        card = GetComponent<GameCard>();
        GameManager.instance.foodNum += foodPoint;
        GameManager.instance.FoodUpdate();
        GameManager.instance.foods.Add(gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (collision.gameObject.layer == 6)
        {
            if (GameCard.mouseUp && card.simulated && (collision.CompareTag("Food") || collision.CompareTag("Resource") || collision.CompareTag("Coin") || collision.name == "Soil(Clone)") && collision.gameObject != card.child && collision.gameObject != card.parent)
            {
                GameManager.instance.StackCard(gameObject, collision.gameObject);
                GameCard.mouseUp = false;
            }
            else if (!card.firstCheck && name == collision.name)
            {
                GameManager.instance.StackCard(gameObject, collision.gameObject);
            }
            else
            {
                if (!card.isStack && !GameCard.mouseHold)
                {
                    GameManager.instance.SeparateCard(gameObject, collision.gameObject);
                }
            }
        }
        card.firstCheck = true;
    }

}
