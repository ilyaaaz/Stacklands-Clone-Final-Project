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
        // Calculate the midpoint and offset for combat positioning
        float spaceBetween = 2.5f;
        Vector3 midPoint = (this.transform.position + mob.position) / 2;

        // Stop the villager's movement and any other scripts that shouldn't run during combat
        this.enabled = false; // Assuming the villager has a movement controller that should be disabled

        // Position the mob and villager for combat
        mob.position = new Vector3(midPoint.x, midPoint.y + spaceBetween / 2, 0); // Mob above
        this.transform.position = new Vector3(midPoint.x, midPoint.y - spaceBetween / 2, 0); // Villager below

        // Optionally start combat coroutine if the villager can fight back
        StartCoroutine(Combat(mob.gameObject));
    }

    IEnumerator Combat(GameObject mob)
    {
        Health villagerHealth = GetComponent<Health>();
        Health mobHealth = mob.GetComponent<Health>();

        while (villagerHealth.currentHealth > 0 && mobHealth.currentHealth > 0)
        {
            // Villager's turn
            AttackAnimation(this.transform);
            mobHealth.TakeDamage(1);
            yield return new WaitForSeconds(1.0f);

            // Mob's turn
            if (mobHealth.currentHealth > 0)
            {
                AttackAnimation(mob.transform);
                villagerHealth.TakeDamage(1);
                yield return new WaitForSeconds(1.0f);
            }
        }
    }

    void AttackAnimation(Transform combatant)
    {
        float originalY = combatant.position.y;
        combatant.position = new Vector3(combatant.position.x, originalY + 0.1f, combatant.position.z);
        StartCoroutine(ResetPosition(combatant, originalY));
    }

    IEnumerator ResetPosition(Transform combatant, float originalY)
    {
        yield return new WaitForSeconds(0.5f);
        combatant.position = new Vector3(combatant.position.x, originalY, combatant.position.z);
    }
}
