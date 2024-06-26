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
        if (card != null)
        {
            if (card.value >= 0)
            {
                for (int i = 0; i < card.value; i++)
                {
                    GameObject coin = Instantiate(Coin, transform.position, Quaternion.identity);
                    coin.GetComponent<GameCard>().startPos = gameObject.transform.position + Vector3.down * 3f;
                }

                SoundManager.instance.PlayCardSell();
                //destroy sold card.
                card.CardDestroy();
            }
            else
            {
                print("a");
                StartCoroutine(card.lerpCard(collision.gameObject, gameObject.transform.position + Vector3.down * 3f));
            }
        }
    }
}
