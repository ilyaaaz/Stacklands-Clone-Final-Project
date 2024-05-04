using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Villager : MonoBehaviour
{
    GameCard card;
    List<GameObject> stackList = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        card = GetComponent<GameCard>();
    }

    private void Update()
    {
        //MakeStackList();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MobController mob = collision.GetComponent<MobController>();
        if (mob != null)
        {
            InitiateCombat(collision.transform);
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
        if (card.parent != null)
        {
            int size = 0;
            GameCard curCard = card.parentCard;
            while (curCard != null)
            {
                stackList.Add(card.parent);
                curCard = curCard.parentCard;
                size++;
            }
            CheckStackProduct(size);
        }
    }

    void CheckStackProduct(int size)
    {
        for (int i = 0; i < GameManager.instance.ideas.Count; i++)
        {
            GameCard idea = GameManager.instance.ideas[i];
            if (idea.materialSize == size)
            {
                bool result = true;
                for (int j = 0; j < idea.materials.Count; i++)
                {
                    GameObject material = idea.materials[j];
                    int materialNum = idea.matchingNum[j];
                    int count = stackList.FindAll(obj => obj.name == material.name).Count;
                    if (materialNum != count)
                    {
                        result = false;
                        break;
                    }
                }
                if (result)
                {
                    GameManager.instance.ProcessBarCreate(stackList[stackList.Count-1], idea.requireTime);
                }
            }
        }
    }

    void InitiateCombat(Transform mob)
    {
        //calculate the midpoint and offset for combat positioning
        float spaceBetween = 2.5f;
        Vector3 midPoint = (this.transform.position + mob.position) / 2;
        this.enabled = false; 
        GameCard card = GetComponent<GameCard>();
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
        
        mob.position = new Vector3(midPoint.x, midPoint.y + spaceBetween / 2, 0); // Mob above
        this.transform.position = new Vector3(midPoint.x, midPoint.y - spaceBetween / 2, 0); // Villager below

        StartCoroutine(Combat(mob.gameObject));
    }


    IEnumerator Combat(GameObject mob)
    {
        Health villagerHealth = GetComponent<Health>();
        Health mobHealth = mob.GetComponent<Health>();
        Vector3 originalVillagerPosition = this.transform.position; 
        Vector3 originalMobPosition = mob.transform.position;  

        while (villagerHealth.currentHealth > 0 && mobHealth.currentHealth > 0)
        {
            //villager's turn
            AttackAnimation(this.transform);
            mobHealth.TakeDamage(1);
            yield return new WaitForSeconds(1.0f);

            //reset positions to ensure they haven't drifted
            this.transform.position = originalVillagerPosition;
            mob.transform.position = originalMobPosition;

            //mob's turn
            if (mobHealth.currentHealth > 0)
            {
                AttackAnimation(mob.transform);
                villagerHealth.TakeDamage(1);
                yield return new WaitForSeconds(1.0f);

                //reset positions again
                this.transform.position = originalVillagerPosition;
                mob.transform.position = originalMobPosition;
            }
        }
    }


    void AttackAnimation(Transform combatant)
    {
        Vector3 originalPosition = combatant.position;  //full original position
        combatant.position = new Vector3(combatant.position.x, combatant.position.y + 0.1f, combatant.position.z);  // Move up a little
        StartCoroutine(ResetPosition(combatant, originalPosition));  //reset to the original position
    }

    IEnumerator ResetPosition(Transform combatant, Vector3 originalPosition)
    {
        yield return new WaitForSeconds(0.5f);  //wait
        combatant.position = originalPosition;  //reset position
    }

}
