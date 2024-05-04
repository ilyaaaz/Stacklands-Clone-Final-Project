using UnityEngine;
using System.Collections;

public class MobController : MonoBehaviour
{
    public float moveSpeed = 25f;
    public float cooldownTime = 2f; 
    private Transform target;
    private float cooldownTimer = 0;

    void Update()
    {
        cooldownTimer -= Time.deltaTime; 

        //check if it's time to move
        if (cooldownTimer <= 0)
        {
            if (target == null || target.gameObject.activeInHierarchy == false)
            {
                target = FindNearestVillager();  //find a new target if there isn't one or it's inactive
            }
            if (target != null)
            {
                MoveTowardsTarget();  //move towards the target if it exists
                cooldownTimer = cooldownTime;  //reset the cooldown timer
            }
        }
    }

    Transform FindNearestVillager()
    {
        GameObject[] villagers = GameObject.FindGameObjectsWithTag("Villager");
        Transform nearest = null;
        float minDistance = float.MaxValue;

        foreach (GameObject villager in villagers)
        {
            float distance = Vector3.Distance(transform.position, villager.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = villager.transform;
            }
        }
        return nearest;
    }

    void MoveTowardsTarget()
    {
        //fixed interpolation factor
        float lerpFactor = 0.2f;  //step size

        //current position to target position
        transform.position = Vector3.Lerp(transform.position, target.position, lerpFactor);
    }

    void InitiateCombat(Transform villager)
    {
        float spaceBetween = 2.5f;
        Vector3 midPoint = (this.transform.position + villager.position) / 2;
        this.enabled = false;
        GameCard card = GetComponent<GameCard>();

        Vector3 mobPosition = new Vector3(midPoint.x, midPoint.y + spaceBetween / 2, 0);
        Vector3 villagerPosition = new Vector3(midPoint.x, midPoint.y - spaceBetween / 2, 0);
        villager.position = mobPosition;
        this.transform.position = villagerPosition;

        StartCoroutine(Combat(villager.gameObject, villagerPosition, mobPosition));
    }

    IEnumerator Combat(GameObject mob, Vector3 villagerPosition, Vector3 mobPosition)
    {
        Health villagerHealth = GetComponent<Health>();
        Health mobHealth = mob.GetComponent<Health>();

        while (villagerHealth.currentHealth > 0 && mobHealth.currentHealth > 0)
        {
            AttackAnimation(this.transform, villagerPosition, mob.transform);
            mobHealth.TakeDamage(1);
            yield return new WaitForSeconds(1.0f);

            this.transform.position = villagerPosition;  //reset villager pos
            mob.transform.position = mobPosition;        //reset mob pos

            if (mobHealth.currentHealth > 0)
            {
                AttackAnimation(mob.transform, mobPosition, this.transform);
                villagerHealth.TakeDamage(1);
                yield return new WaitForSeconds(1.0f);

                this.transform.position = villagerPosition;  //reset villager pos
                mob.transform.position = mobPosition;        //reset mob pos
            }
        }
    }

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