using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardStacker : MonoBehaviour
{
    public float stackOffset = 0.1f; // The vertical offset to stack the card on top of another

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Card"))
        {
            Vector3 contactPoint = collision.contacts[0].point;
            Vector3 newCardPosition = new Vector3(transform.position.x, contactPoint.y + stackOffset, transform.position.z);

            transform.position = newCardPosition;

            transform.SetParent(collision.transform);

            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.isKinematic = true;
            rb.velocity = Vector2.zero;
        }
    }
}
