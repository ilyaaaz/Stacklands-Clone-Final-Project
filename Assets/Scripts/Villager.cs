using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Villager : MonoBehaviour
{
    GameCard card;
    [HideInInspector] public List<GameObject> stackList;
    [HideInInspector] public bool checkOnce;
    private Transform combatOpponent;
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
        combatOpponent = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //store potential combat opponent
        if (collision.CompareTag("Rabbit") || collision.CompareTag("Weasel"))
        {
            combatOpponent = collision.transform;
        }
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
                    GameManager.instance.ProcessBarCreateWithProduct(stackList[stackList.Count - 1], GameManager.instance.ideasObj[i], idea.requireTime, stackList);
                }
            }
        }
    }

    void InitiateCombat(Transform opponent)
    {
        float spaceBetween = 2.5f;
        Vector3 midPoint = (this.transform.position + opponent.position) / 2;
        Vector3 villagerPosition = new Vector3(midPoint.x, midPoint.y + spaceBetween / 2, 0);
        Vector3 opponentPosition = new Vector3(midPoint.x, midPoint.y - spaceBetween / 2, 0);

        this.transform.position = villagerPosition; // Villager moves to top position
        opponent.position = opponentPosition; // Opponent moves to bottom position

        StartCoroutine(Combat(opponent.gameObject, villagerPosition, opponentPosition));
    }

    IEnumerator Combat(GameObject opponent, Vector3 villagerPosition, Vector3 opponentPosition)
    {
        Health villagerHealth = GetComponent<Health>();
        Health opponentHealth = opponent ? opponent.GetComponent<Health>() : null;

        while (villagerHealth.currentHealth > 0 && opponentHealth && opponentHealth.currentHealth > 0)
        {
            opponentHealth.TakeDamage(1);
            yield return new WaitForSeconds(1.0f);

            if (opponentHealth.currentHealth <= 0) break;

            villagerHealth.TakeDamage(1);
            yield return new WaitForSeconds(1.0f);

            this.transform.position = villagerPosition; // Reset villager position
            if (opponent) opponent.transform.position = opponentPosition; // Reset opponent position if still exists
        }
    }

    void ClearParentChildRelations(GameCard card)
    {
        if (card != null)
        {
            card.parent = null;
            card.child = null;
            if (card.parentCard != null)
            {
                card.parentCard.child = null;
                card.parentCard.childCard = null;
                card.parentCard = null;
            }
            if (card.childCard != null)
            {
                card.childCard.parent = null;
                card.childCard.parentCard = null;
                card.childCard = null;
            }
        }
    }

    // IEnumerator Combat(GameObject mob, Vector3 villagerPosition, Vector3 mobPosition)
    // {
    //     Health villagerHealth = GetComponent<Health>();
    //     Health mobHealth = mob.GetComponent<Health>();

    //     while (villagerHealth.currentHealth > 0 && mobHealth.currentHealth > 0)
    //     {
    //         AttackAnimation(this.transform, villagerPosition, mob.transform);
    //         mobHealth.TakeDamage(1);
    //         yield return new WaitForSeconds(1.0f);

    //         this.transform.position = villagerPosition;  //reset villager pos
    //         mob.transform.position = mobPosition;        //reset mob pos

    //         if (mobHealth.currentHealth > 0)
    //         {
    //             AttackAnimation(mob.transform, mobPosition, this.transform);
    //             villagerHealth.TakeDamage(1);
    //             yield return new WaitForSeconds(1.0f);

    //             this.transform.position = villagerPosition;  //reset villager pos
    //             mob.transform.position = mobPosition;        //reset mob pos
    //         }
    //     }
    // }

    void AttackAnimation(Transform combatant, Vector3 combatPosition, Transform target)
    {
        //point slightly in front of the target to simulate an attack landing
        Vector3 attackPosition = combatPosition + (target.position - combatPosition) * 0.1f + new Vector3(0, 0.3f, 0);

        //attack move
        StartCoroutine(PerformAttack(combatant, combatPosition, attackPosition));
    }

    IEnumerator PerformAttack(Transform combatant, Vector3 originalPosition, Vector3 attackPosition)
    {
        //attack pos
        float attackTime = 0.1f;
        for (float t = 0; t < 1; t += Time.deltaTime / attackTime)
        {
            combatant.position = Vector3.Lerp(originalPosition, attackPosition, t);
            yield return null;
        }

        //attack pos pause
        yield return new WaitForSeconds(0.1f);

        //original combat pos
        for (float t = 0; t < 1; t += Time.deltaTime / attackTime)
        {
            combatant.position = Vector3.Lerp(attackPosition, originalPosition, t);
            yield return null;
        }

        //position
        combatant.position = originalPosition;
    }

    IEnumerator ResetPosition(Transform combatant, Vector3 originalPosition)
    {
        yield return new WaitForSeconds(0.5f);  //wait
        combatant.position = originalPosition;  //reset position
    }

}