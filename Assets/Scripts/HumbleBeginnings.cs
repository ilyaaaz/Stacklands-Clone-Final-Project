using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class HumbleBeginnings : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI titleText, detailedText;
    [SerializeField] List<GameObject> list = new List<GameObject>();
    int cardIndex;
    float circleRadius = 2.5f;

    private void Awake()
    {
        titleText = GameObject.Find("TitleText (TMP)").GetComponent<TextMeshProUGUI>();
        detailedText = GameObject.Find("DetailedText (TMP)").GetComponent<TextMeshProUGUI>();
    }
    // Start is called before the first frame update
    void Start()
    {
        cardIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnMouseDrag()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePos.x, mousePos.y, 0);
    }

    private void OnMouseExit()
    {
        titleText.text = "";
        detailedText.text = "";
    }

    private void OnMouseOver()
    {
        titleText.text = "Humble Beginnings";
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
            GameObject newCard = Instantiate(list[cardIndex], transform.position, Quaternion.identity);

            Vector3 circleCenter = transform.position;

            float angle = cardIndex * Mathf.PI * 2f / list.Count;
            float x = circleCenter.x + circleRadius * Mathf.Cos(angle);
            float y = circleCenter.y + circleRadius * Mathf.Sin(angle);
            Vector3 objectPosition = new Vector3(x, y, 0f);
            cardIndex++;
            newCard.GetComponent<GameCard>().startPos = objectPosition;
            if (cardIndex == list.Count)
            {
                titleText.text = "";
                detailedText.text = "";
                Destroy(gameObject);
            }
        }
    }
}
