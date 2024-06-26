using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ANewWorld : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI titleText, detailedText;
    [SerializeField] List<GameObject> list = new List<GameObject>();
    int cardIndex;
    float circleRadius = 2.5f;
    [SerializeField] SpriteRenderer remain;
    [SerializeField] List<Sprite> remainUI = new List<Sprite>();
    int remainNum;

    // Start is called before the first frame update
    void Start()
    {
        remainNum = 4;
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
            if (remainNum > 0)
            {
                remainNum--;
                remain.sprite = remainUI[remainNum];
            }
            SoundManager.instance.PlayOpenPack();
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
