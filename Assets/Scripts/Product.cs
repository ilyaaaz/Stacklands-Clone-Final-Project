using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Product : MonoBehaviour
{
    public List<GameObject> material;
    public int times;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (times == 0)
        {
            GameManager.instance.cardNum--;
            Destroy(gameObject);
        }
    }

    public void createMaterial()
    {
        // GameObject newCard = Instantiate(material, transform.position, Quaternion.identity);

        int index = Random.Range(0, material.Count);
        GameObject selectedMaterial = material[index];
        GameObject newCard = Instantiate(selectedMaterial, transform.position, Quaternion.identity);

        newCard.GetComponent<GameCard>().startPos = transform.position + Vector3.up * 2f;
        times--;
    }
}
