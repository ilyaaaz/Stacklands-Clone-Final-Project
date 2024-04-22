using UnityEngine;

public class SellArea : MonoBehaviour
{
    [SerializeField] GameObject Coin;
    Collider2D cld;

    private void Awake()
    {
        cld = GetComponent<Collider2D>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            cld.enabled = false;
        }
        if (Input.GetMouseButtonUp(0))
        {
            cld.enabled = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameCard card = collision.GetComponent<GameCard>();
        if (card.value >= 0)
        {
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
