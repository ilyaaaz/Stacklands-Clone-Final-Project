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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Rabbit") || collision.CompareTag("Weasel") || collision.CompareTag("Mob"))
        {
            combatOpponent = collision.transform;
        }
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.CompareTag("Rabbit") || collision.CompareTag("Weasel") || collision.CompareTag("Mob")) && 
            GameManager.instance.currentCard == gameObject && GameCard.mouseUp)
        {
            if (combatOpponent != null)
            {
                InitiateCombat(combatOpponent);
                GameCard.mouseUp = false; 
            }
        }
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

    void HandleCardInteraction(Collider2D collision)
    {
        if (GameCard.mouseUp && card.simulated)
        {
            GameManager.instance.StackCard(gameObject, collision.gameObject);
            GameCard.mouseUp = false;
        }
        else if (!card.isStack && !GameCard.mouseHold)
        {
            GameManager.instance.SeparateCard(gameObject, collision.gameObject);
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
        if (opponent != null)
        {
            StartCoroutine(Combat(opponent.gameObject, transform.position, opponent.position));
        }
    }

    IEnumerator Combat(GameObject opponent, Vector3 villagerPosition, Vector3 opponentPosition)
    {
        Health villagerHealth = GetComponent<Health>();
        Health opponentHealth = opponent.GetComponent<Health>();

        // Move to attack positions
        yield return MoveToPosition(transform, (villagerPosition + opponentPosition) / 2 + new Vector3(0, 1, 0), 0.5f);
        yield return MoveToPosition(opponent.transform, (villagerPosition + opponentPosition) / 2 - new Vector3(0, 1, 0), 0.5f);

        // Simulate attacks
        while (villagerHealth.currentHealth > 0 && opponentHealth.currentHealth > 0)
        {
            // Villager attacks
            opponentHealth.TakeDamage(1);
            yield return new WaitForSeconds(0.5f); // Pause for attack animation

            if (opponentHealth.currentHealth <= 0) break;

            // Opponent attacks
            villagerHealth.TakeDamage(1);
            yield return new WaitForSeconds(0.5f); // Pause for attack animation

            // Return to original positions for a brief moment
            yield return MoveToPosition(transform, villagerPosition, 0.25f);
            if (opponent) yield return MoveToPosition(opponent.transform, opponentPosition, 0.25f);
        }
    }

    IEnumerator MoveToPosition(Transform entity, Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = entity.position;
        float time = 0;
        while (time < duration)
        {
            entity.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        entity.position = targetPosition;
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

    // void AttackAnimation(Transform combatant, Vector3 combatPosition, Transform target)
    // {
    //     Vector3 attackPosition = combatPosition + (target.position - combatPosition) * 0.1f + new Vector3(0, 0.3f, 0);
    //     StartCoroutine(PerformAttack(combatant, combatPosition, attackPosition));
    // }

    // IEnumerator PerformAttack(Transform combatant, Vector3 originalPosition, Vector3 attackPosition)
    // {
    //     float attackTime = 0.1f;
    //     for (float t = 0; t < 1; t += Time.deltaTime / attackTime)
    //     {
    //         combatant.position = Vector3.Lerp(originalPosition, attackPosition, t);
    //         yield return null;
    //     }
    //     yield return new WaitForSeconds(0.1f);
    //     for (float t = 0; t < 1; t += Time.deltaTime / attackTime)
    //     {
    //         combatant.position = Vector3.Lerp(attackPosition, originalPosition, t);
    //         yield return null;
    //     }
    // }

    // void ClearParentChildRelations(GameCard card)
    // {
    //     if (card != null)
    //     {
    //         card.parent = null;
    //         card.child = null;
    //         if (card.parentCard != null)
    //         {
    //             card.parentCard.child = null;
    //             card.parentCard.childCard = null;
    //             card.parentCard = null;
    //         }
    //         if (card.childCard != null)
    //         {
    //             card.childCard.parent = null;
    //             card.childCard.parentCard = null;
    //             card.childCard = null;
    //         }
    //     }
    // }

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

    // void AttackAnimation(Transform combatant, Vector3 combatPosition, Transform target)
    // {
    //     //point slightly in front of the target to simulate an attack landing
    //     Vector3 attackPosition = combatPosition + (target.position - combatPosition) * 0.1f + new Vector3(0, 0.3f, 0);

    //     //attack move
    //     StartCoroutine(PerformAttack(combatant, combatPosition, attackPosition));
    // }

    // IEnumerator PerformAttack(Transform combatant, Vector3 originalPosition, Vector3 attackPosition)
    // {
    //     //attack pos
    //     float attackTime = 0.1f;
    //     for (float t = 0; t < 1; t += Time.deltaTime / attackTime)
    //     {
    //         combatant.position = Vector3.Lerp(originalPosition, attackPosition, t);
    //         yield return null;
    //     }

    //     //attack pos pause
    //     yield return new WaitForSeconds(0.1f);

    //     //original combat pos
    //     for (float t = 0; t < 1; t += Time.deltaTime / attackTime)
    //     {
    //         combatant.position = Vector3.Lerp(attackPosition, originalPosition, t);
    //         yield return null;
    //     }

    //     //position
    //     combatant.position = originalPosition;
    // }

    // IEnumerator ResetPosition(Transform combatant, Vector3 originalPosition)
    // {
    //     yield return new WaitForSeconds(0.5f);  //wait
    //     combatant.position = originalPosition;  //reset position
    // }

}