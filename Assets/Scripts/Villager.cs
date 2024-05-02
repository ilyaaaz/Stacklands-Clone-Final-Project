using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Villager : MonoBehaviour
{
    GameCard card;
    [HideInInspector] public List<GameObject> stackList;
    [HideInInspector] public bool checkOnce;
    // Start is called before the first frame update
    void Start()
    {
        card = GetComponent<GameCard>();
        checkOnce = false;
    }

    private void Update()
    {
        MakeStackList();
    }

    private void OnMouseDown()
    {
        checkOnce = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            if (GameCard.mouseUp && card.simulated)
            {
                GameManager.instance.StackCard(gameObject, collision.gameObject);
                GameCard.mouseUp = false;
            }
            else
            {
                if (!card.isStack && !GameCard.mouseHold)
                {
                    GameManager.instance.SeparateCard(gameObject, collision.gameObject);
                }
            }
        }
    }

    void MakeStackList()
    {
        if (!checkOnce)
        {
            stackList = new List<GameObject>();
            if (card.parent != null)
            {
                int size = 0;
                GameCard curCard = card.parentCard;
                while (curCard != null)
                {
                    stackList.Add(curCard.gameObject);
                    curCard = curCard.parentCard;
                    size++;
                }
                CheckStackProduct(size);
            }
        }
    }

    void CheckStackProduct(int size)
    {
        checkOnce = true;
        for (int i = 0; i < GameManager.instance.ideas.Count; i++)
        {
            GameCard idea = GameManager.instance.ideas[i];
            if (idea.materialSize == size)
            {
                bool result = true;
                for (int j = 0; j < idea.materials.Count; j++)
                {
                    GameObject material = idea.materials[j];
                    int materialNum = idea.matchingNum[j];
                    int count = stackList.FindAll(obj => obj.name.Contains(material.name)).Count;
                    if (materialNum != count)
                    {
                        result = false;
                        break;
                    }
                }
                if (result)
                {
                   GameManager.instance.ProcessBarCreateWithProduct(stackList[stackList.Count-1], GameManager.instance.ideasObj[i] ,idea.requireTime, stackList);
                }
            }
        }
    }
}
