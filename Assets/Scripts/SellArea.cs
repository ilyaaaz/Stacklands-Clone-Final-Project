using UnityEngine;

public class SellArea : MonoBehaviour
{
    [SerializeField]
    private GameObject Coin;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameCard card = collision.GetComponent<GameCard>();
        if (card != null)
        {
            //generate coins equivalent to card's value.
            for (int i = 0; i < card.value; i++)
            {
                Instantiate(Coin, transform.position, Quaternion.identity);
            }
            
            GameManager.instance.coinNum += card.value;
            GameManager.instance.CoinUpdate();

            //destroy sold card.
            Destroy(collision.gameObject);
        }
    }
}
