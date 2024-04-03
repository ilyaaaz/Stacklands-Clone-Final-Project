using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ANewWorld : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI titleText, detailedText;
    [SerializeField] List<GameObject> list = new List<GameObject>();
    int cardIndex;
    // Start is called before the first frame update
    void Start()
    {
        cardIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseExit()
    {
        titleText.text = "";
        detailedText.text = "";
    }

    private void OnMouseOver()
    {
        titleText.text = "A NEW WORLD";
        detailedText.text = "Open this Pack to get Cards!";
        if (Input.GetMouseButtonDown(0))
        {
            mouseClick();
        }
    }
    public void mouseClick()
    {
        if (cardIndex < list.Count)
        {
            GameObject newCard = Instantiate(list[cardIndex]);
            newCard.transform.position = transform.position + (cardIndex+1) * Vector3.right;
            cardIndex++;
            if (cardIndex == list.Count)
            {
                Destroy(gameObject);
            }
        }
    }
}
