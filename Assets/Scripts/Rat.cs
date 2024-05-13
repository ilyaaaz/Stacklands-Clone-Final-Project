using System.Collections;
using UnityEngine;

public class Rat : MonoBehaviour
{
    public float moveSpeed = 10f;  //speed of the rat
    public float moveInterval = 3f;  //how often the rat moves towards the villager
    private Transform targetVillager;  //the current target for the rat
    private float moveTimer = 0;  //timer to manage movement intervals
    private Vector3 nextPosition;  //next position to move towards

    void Update()
    {
        moveTimer -= Time.deltaTime;

        if (targetVillager == null)
        {
            targetVillager = FindNearestVillager();
        }

        if (targetVillager != null && moveTimer <= 0)
        {
            nextPosition = Vector3.Lerp(transform.position, targetVillager.position, 0.5f); 
            moveTimer = moveInterval;  //reset timer
        }

        //move towards calculated position
        if (moveTimer > 0 && moveTimer < moveInterval)
        {
            MoveTowardsTarget(nextPosition);
        }

        if (targetVillager != null && Vector3.Distance(transform.position, targetVillager.position) < 1f)
        {
            if (CombatController.instance != null)
            {
                CombatController.instance.StartCoroutine(
                    CombatController.instance.HandleCombat(
                        gameObject, 
                        targetVillager.gameObject, 
                        OnCombatEnd
                    )
                );
                targetVillager = null;  //reset target after combat
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

    void MoveTowardsTarget(Vector3 targetPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    private void OnCombatEnd()
    {
        Debug.Log("Combat has ended");
    }
}
