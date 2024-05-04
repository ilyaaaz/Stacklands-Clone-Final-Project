using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class HumbleBeginnings : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI titleText, detailedText;
    [SerializeField] List<GameObject> list = new List<GameObject>();
    [SerializeField] List<GameObject> idea = new List<GameObject>();
    [SerializeField] List<float> percent = new List<float>();
    int cardIndex, count;
    float circleRadius = 2.5f;
    int ideaIndex, replaceCount;

    private void Awake()
    {
        titleText = GameObject.Find("TitleText (TMP)").GetComponent<TextMeshProUGUI>();
        detailedText = GameObject.Find("DetailedText (TMP)").GetComponent<TextMeshProUGUI>();
    }
    // Start is called before the first frame update
    void Start()
    {
        cardIndex = 0;
        count = 0;
        if (idea.Count > 0)
        {
            ideaIndex = Random.Range(0, idea.Count);
            replaceCount = Random.Range(0, 3);
        }
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
        if (Input.GetMouseButtonUp(0))
        {
            mouseClick();
            SoundManager.instance.PlayOpenPack();
        }
    }
    public void mouseClick()
    {
        cardIndex = randomIndex();
        
        if (count <= 2)
        {
            GameObject newCard;
            if (ideaIndex >= 0 && count == replaceCount)
            {
                newCard = Instantiate(idea[ideaIndex], transform.position, Quaternion.identity);
                idea.Remove(idea[ideaIndex]);
                GameManager.instance.ideasFound.Add(newCard.name);
                GameManager.instance.ideasFoundCheck();
            } else
            {
                newCard = Instantiate(list[cardIndex], transform.position, Quaternion.identity);
            }
        
            Vector3 circleCenter = transform.position;

            float angle = count * Mathf.PI * 2f / 3;
            float x = circleCenter.x + circleRadius * Mathf.Cos(angle);
            float y = circleCenter.y + circleRadius * Mathf.Sin(angle);
            Vector3 objectPosition = new Vector3(x, y, 0f);
            newCard.GetComponent<GameCard>().startPos = objectPosition;
            count++;
            if (count == 3)
            {
                titleText.text = "";
                detailedText.text = "";
                Destroy(gameObject);
            }
        }
    }
    int randomIndex()
    {
        int result = 0;
        // Create cumulative distribution array
        float[] cumulative = new float[percent.Count];
        float total = 0;

        for (int i = 0; i < percent.Count; i++)
        {
            total += percent[i];
            cumulative[i] = total;
        }

        // Generate random number between 0 and total of all percentages
        float randomPoint = Random.Range(0, total);

        // Determine which card corresponds to randomPoint
        for (int i = 0; i < cumulative.Length; i++)
        {
            if (randomPoint <= cumulative[i])
            {
                result = i; 
                break;
            }
        }
        return result;
    }
}
