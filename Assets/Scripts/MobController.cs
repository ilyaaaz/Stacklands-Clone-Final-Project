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
        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Villager"))
        {
            //position
            PositionForCombat(collision.gameObject.transform);

            //combat
            StartCoroutine(Combat(collision.gameObject));
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
        Vector3 midPoint = (this.transform.position + villager.position) / 2; //midpoint between the two
        this.transform.position = new Vector3(midPoint.x, midPoint.y + spaceBetween / 2, 0); //mob above
        villager.position = new Vector3(midPoint.x, midPoint.y - spaceBetween / 2, 0); //villager below
    }



    IEnumerator Combat(GameObject villager)
    {
        Health mobHealth = GetComponent<Health>();
        Health villagerHealth = villager.GetComponent<Health>();

        while (mobHealth.currentHealth > 0 && villagerHealth.currentHealth > 0)
        {
            //mob's turn
            AttackAnimation(this.transform);  //visual indication of attack
            villagerHealth.TakeDamage(1);
            yield return new WaitForSeconds(1.0f);

            //villager's turn
            if (villagerHealth.currentHealth > 0)
            {
                AttackAnimation(villager.transform);  //visual indication of attack
                mobHealth.TakeDamage(1);
                yield return new WaitForSeconds(1.0f);
            }
        }
    }

    void AttackAnimation(Transform combatant)
    {
        float originalY = combatant.position.y;
        combatant.position = new Vector3(combatant.position.x, originalY + 0.1f, combatant.position.z); //move up a little
        StartCoroutine(ResetPosition(combatant, originalY));
    }

    IEnumerator ResetPosition(Transform combatant, float originalY)
    {
        yield return new WaitForSeconds(0.5f); //wait
        combatant.position = new Vector3(combatant.position.x, originalY, combatant.position.z); //reset position
    }

}
