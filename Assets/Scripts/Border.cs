using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border : MonoBehaviour
{
    public int dir;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Card"))
        {
            Rigidbody2D cld = collision.gameObject.GetComponent<Rigidbody2D>();
            if (dir == 0)
            {
                cld.AddForce(Vector3.right * 4);
            }
            else if (dir == 90)
            {
                cld.AddForce(Vector3.up * 4);
            }
            else if (dir == 180)
            {
                cld.AddForce(Vector3.left * 4);
            }
            else if (dir == 270)
            {
                cld.AddForce(Vector3.down * 4);
            }

        }
    }
}
