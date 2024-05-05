using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DottedOutline : MonoBehaviour
{
    public GameObject dotPrefab; 
    public float width = 1.4f; 
    public float height = 1.65f; 
    public int numberOfDots = 20;

    void Start()
    {
        CreateDottedOutline();
    }

    void CreateDottedOutline()
    {
        float perimeter = 2 * (width + height);
        int dotsWidth = Mathf.RoundToInt(numberOfDots * (width / perimeter));
        int dotsHeight = (numberOfDots - 2 * dotsWidth) / 2;

        for (int i = 0; i <= dotsWidth; i++)
        {
            Vector3 topPosition = new Vector3(-width / 2 + i * (width / dotsWidth), height / 2, 0) + transform.position;
            Instantiate(dotPrefab, topPosition, Quaternion.identity, transform);

            if (i < dotsWidth) 
            {
                Vector3 bottomPosition = new Vector3(-width / 2 + i * (width / dotsWidth), -height / 2, 0) + transform.position;
                Instantiate(dotPrefab, bottomPosition, Quaternion.identity, transform);
            }
        }

        for (int i = 1; i < dotsHeight; i++) 
        {
            Vector3 rightPosition = new Vector3(width / 2, -height / 2 + i * (height / dotsHeight), 0) + transform.position;
            Instantiate(dotPrefab, rightPosition, Quaternion.identity, transform);

            Vector3 leftPosition = new Vector3(-width / 2, -height / 2 + i * (height / dotsHeight), 0) + transform.position;
            Instantiate(dotPrefab, leftPosition, Quaternion.identity, transform);
        }
    }
}
