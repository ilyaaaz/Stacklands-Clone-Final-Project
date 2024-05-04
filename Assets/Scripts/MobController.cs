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
        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, target.position, step / Vector3.Distance(transform.position, target.position));
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Villager"))
        {
            //calculate the positions for combat 
            Vector3 midPoint = (transform.position + collision.transform.position) / 2;
            float spaceBetween = 2.5f; 
            Vector3 mobPosition = new Vector3(midPoint.x, midPoint.y + spaceBetween / 2, 0); // Mob above
            Vector3 villagerPosition = new Vector3(midPoint.x, midPoint.y - spaceBetween / 2, 0); // Villager below

            //stop both movement and start combat
            this.enabled = false;
            if (collision.gameObject.GetComponent<MobController>())
            {
                collision.gameObject.GetComponent<MobController>().enabled = false;
            }

            this.transform.position = mobPosition;  //update this mob's pos
            collision.transform.position = villagerPosition;  //update colliding villager's pos

            StartCoroutine(Combat(collision.gameObject, mobPosition, villagerPosition)); //new positions
        }
    }


    void PositionForCombat(Transform villager)
    {
        //stop the mob's and villager's movement
        this.enabled = false; //disabling the mob's script to stop further updates
        if (villager.GetComponent<MobController>()) //check and disable the villager's movement if it has the same script
        {
            villager.GetComponent<MobController>().enabled = false;
        }

        //calculate new positions
        float spaceBetween = 2.5f;
        Vector3 midPoint = (this.transform.position + villager.position) / 2;
        Vector3 mobPosition = new Vector3(midPoint.x, midPoint.y + spaceBetween / 2, 0); //mob above
        Vector3 villagerPosition = new Vector3(midPoint.x, midPoint.y - spaceBetween / 2, 0); //villager below
        this.transform.position = mobPosition;
        villager.position = villagerPosition;

        //start combat
        StartCoroutine(Combat(villager.gameObject, mobPosition, villagerPosition));
    }


    IEnumerator Combat(GameObject villager, Vector3 mobPosition, Vector3 villagerPosition)
    {
        Health mobHealth = GetComponent<Health>();
        Health villagerHealth = villager.GetComponent<Health>();
        Transform villagerTransform = villager.transform;  // We need this for directional attacks

        while (mobHealth.currentHealth > 0 && villagerHealth.currentHealth > 0)
        {
            //mob attacks villager
            AttackAnimation(this.transform, mobPosition, villagerTransform);
            villagerHealth.TakeDamage(1);
            yield return new WaitForSeconds(1.0f);

            //villager attacks mob
            if (villagerHealth.currentHealth > 0)
            {
                AttackAnimation(villagerTransform, villagerPosition, this.transform);
                mobHealth.TakeDamage(1);
                yield return new WaitForSeconds(1.0f);
            }
        }
    }


    void AttackAnimation(Transform combatant, Vector3 combatPosition, Transform target)
    {
        //point slightly in front of the target to simulate an attack landing
        Vector3 attackPosition = combatPosition + (target.position - combatPosition) * 0.1f + new Vector3(0, 0.1f, 0);

        //attack move
        StartCoroutine(PerformAttack(combatant, combatPosition, attackPosition));
    }

    IEnumerator PerformAttack(Transform combatant, Vector3 originalPosition, Vector3 attackPosition)
    {
        // Move towards the attack position
        float attackTime = 0.1f;
        for (float t = 0; t < 1; t += Time.deltaTime / attackTime)
        {
            combatant.position = Vector3.Lerp(originalPosition, attackPosition, t);
            yield return null;
        }

        //attack position pause
        yield return new WaitForSeconds(0.1f);

        //original combat position
        for (float t = 0; t < 1; t += Time.deltaTime / attackTime)
        {
            combatant.position = Vector3.Lerp(attackPosition, originalPosition, t);
            yield return null;
        }

        //position
        combatant.position = originalPosition;
    }
}