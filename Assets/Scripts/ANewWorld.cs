using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ANewWorld : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI titleText, detailedText;
    [SerializeField] List<GameObject> list = new List<GameObject>();
    int cardIndex;
    float circleRadius = 2f;
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

            Vector3 circleCenter = transform.position; 

            float angle = cardIndex * Mathf.PI * 2f / list.Count; 
            float x = circleCenter.x + circleRadius * Mathf.Cos(angle);
            float y = circleCenter.y + circleRadius * Mathf.Sin(angle);
            Vector3 objectPosition = new Vector3(x, y, 0f);
            cardIndex++;
            StartCoroutine(lerpCard(newCard, objectPosition)); 
            
        }
    }
    IEnumerator lerpCard(GameObject newCard, Vector3 objectPosition)
    {
        while (Vector3.Distance(newCard.transform.position, objectPosition) > 0.1f)
        {
            yield return new WaitForSeconds(0.01f);
            newCard.transform.position = Vector3.Lerp(newCard.transform.position, objectPosition, 0.1f);
            if (cardIndex == list.Count)
            {
                if (Vector2.Distance(newCard.transform.position, objectPosition) < 0.1f)
                {
                    Destroy(gameObject);
                }
            }
        }

    }
}
