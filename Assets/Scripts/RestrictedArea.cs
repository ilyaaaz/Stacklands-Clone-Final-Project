using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestrictedArea : MonoBehaviour
{
    Collider2D cld;

    private void Start()
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            //rb.velocity = Vector2.zero; 
            rb.AddForce(Vector2.down * 15f); 
        }
    }
}
