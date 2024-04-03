using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public STATE currentState;
    // Start is called before the first frame update
    [SerializeField] TextMeshProUGUI titleText, detailedText;

    void Start()
    {
        instance = this;
        currentState = STATE.Normal;
    }

    public enum STATE { 
        Normal,
        Stop,
        Fast
    }

    // Update is called once per frame
    void Update()
    {
        StateUpdate();
        //MouseCheck();
    }

    void StateUpdate()
    {
        if (currentState == STATE.Normal)
        {

        }
        else if (currentState == STATE.Fast)
        {

        }
        else if (currentState == STATE.Stop)
        {

        }
    }

    void MouseCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.name == "ANewWorld")
            {
                MouseOnPack();
            } 
        } else
        {
            titleText.text = "";
            detailedText.text = "";
        }
    }

    void MouseOnPack()
    {
        titleText.text = "A NEW WORLD";
        detailedText.text = "Open this Pack to get Cards!";
        if (Input.GetMouseButtonDown(0))
        {

        }
    }
}
