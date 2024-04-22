using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Product : MonoBehaviour
{
    public GameObject material;
    public int times;

    // Start is called before the first frame update
    void Start()
    {
        times = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (times == 0)
        {
            Destroy(gameObject);
        }
    }

    public void createMaterial()
    {
        GameObject newCard = Instantiate(material, transform.position, Quaternion.identity);
        newCard.GetComponent<GameCard>().startPos = transform.position + Vector3.up * 2f;
        times--;
    }
}
