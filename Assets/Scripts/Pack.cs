using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pack : MonoBehaviour
{
    [SerializeField] GameObject pack;
    Collider2D cld;
    [SerializeField] int price;
    public int coinNeed;

    private void Awake()
    {
        cld = GetComponent<Collider2D>();
    }

    private void Start()
    {
        coinNeed = price;
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
        if (coinNeed <= 0)
        {
            Instantiate(pack, transform.position + Vector3.down * 3, Quaternion.identity);
            coinNeed += price;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Coin"))
        {
            coinNeed--;
            SoundManager.instance.PlayBuyPack();
            collision.gameObject.GetComponent<GameCard>().CardDestroy();
        } else
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.down * 15f);
        }
    }
}
