using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestrictedArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero; 
            rb.AddForce(Vector2.down * 15f, ForceMode2D.Impulse); 
        }
    }
}
