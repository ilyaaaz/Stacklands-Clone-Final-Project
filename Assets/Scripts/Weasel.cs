using UnityEngine;
using System.Collections;

public class WeaselController : MonoBehaviour
{
    public float foodDetectionRadius = 0.5f;
    public float eatingCooldown = 10f;
    private float eatTimer = 0f;
    public float health = 5f;
    public float moveSpeed = 12f;
    private float moveInterval = 0.8f;
    private float moveTimer = 0f;
    private bool isMoving = false;
    private bool isCombat = false;
    private Vector3 targetPosition;

    private float minY = -5f;
    private float maxY = 3f;
    private float minX = -9f;
    private float maxX = 8.5f;
    private bool hasEntered = false;
    private GameObject currentTargetFood = null;
    private Animator animator;

   void Start()
    {
        targetPosition = new Vector3(0, transform.position.y, 0);
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (health <= 0) Die();

        if (!hasEntered)
        {
            MoveToEntry();
            return;
        }

        eatTimer += Time.deltaTime;
        if (eatTimer >= eatingCooldown)
        {
            CheckForFood();
        }

        moveTimer += Time.deltaTime;
        if (moveTimer >= moveInterval && !isMoving && currentTargetFood == null && !isCombat)
        {
            PrepareRandomMovement();
            moveTimer = 0f;
        }

        if (isMoving && currentTargetFood == null && !isCombat)
        {
            MoveToTarget();
        }

        EnsureWithinBounds();
    }

    void MoveToEntry()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            hasEntered = true;
        }
    }

    void CheckForFood()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, foodDetectionRadius);
        float minDistance = float.MaxValue;

        foreach (var hit in hitColliders)
        {
            if (hit.CompareTag("Food"))
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    currentTargetFood = hit.gameObject;
                }
            }
        }

        if (currentTargetFood != null)
        {
            MoveToAndEatFood(currentTargetFood);
        }
    }

    void MoveToAndEatFood(GameObject food)
    {
        transform.position = Vector3.MoveTowards(transform.position, food.transform.position, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, food.transform.position) < 0.1f)
        {
            Destroy(food);
            GameManager.instance.foodNum -= 1;
            currentTargetFood = null;
            eatTimer = 0;
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    void EnsureWithinBounds()
    {
        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

    void PrepareRandomMovement()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized * 3.0f;
        targetPosition = transform.position + new Vector3(randomDirection.x, randomDirection.y, 0);
        targetPosition = new Vector3(
            Mathf.Clamp(targetPosition.x, minX, maxX),
            Mathf.Clamp(targetPosition.y, minY, maxY),
            targetPosition.z
        );
        isMoving = true;
    }

    void MoveToTarget()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            isMoving = false;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health > 0)
        {
            animator.SetTrigger("Hit");
        }
        else
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Villager"))
        {
            isCombat = true;
            TakeDamage(1f); 
        }
    }
}











// using UnityEngine;

// public class WeaselController : MonoBehaviour
// {
//     public float foodDetectionRadius = 0.5f; //proximity for detecting food
//     public float eatingCooldown = 10f; //time between allowed eating actions
//     private float eatTimer = 0f;
//     public float health = 5f;
//     public float moveSpeed = 12f; //speed at which the weasel moves
//     private float moveInterval = 0.8f; //time between random movements
//     private float moveTimer = 0f;
//     private bool isMoving = false;
//     private Vector3 targetPosition;

//     private float minY = -5f;
//     private float maxY = 3f;
//     private float minX = -9f;
//     private float maxX = 8.5f;
//     private bool hasEntered = false;

//     void Start()
//     {
//         targetPosition = new Vector3(0, transform.position.y, 0);
//         isMoving = true;
//     }
//     void Update()
//     {
//         if (health <= 0) Die();

//         if (!hasEntered)
//         {
//             MoveToEntry();
//             return;
//         }

//         eatTimer += Time.deltaTime;
//         if (eatTimer >= eatingCooldown)
//         {
//             CheckForFood();
//         }

//         moveTimer += Time.deltaTime;
//         if (moveTimer >= moveInterval && !isMoving)
//         {
//             PrepareRandomMovement();
//             moveTimer = 0f; //reset timer after setting up movement
//         }

//         if (isMoving)
//         {
//             MoveToTarget();
//         }

//         EnsureWithinBounds();
//     }

//     void MoveToEntry()
//     {
//         transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
//         if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
//         {
//             hasEntered = true; //confirm entry to start normal behavior
//         }
//     }

//     // void CheckForFood()
//     // {
//     //     Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, foodDetectionRadius);
//     //     foreach (var hit in hitColliders)
//     //     {
//     //         if (hit.CompareTag("Food") && Vector3.Distance(transform.position, hit.transform.position) <= foodDetectionRadius)
//     //         {
//     //             MoveToAndEatFood(hit.gameObject);
//     //             break;
//     //         }
//     //     }
//     // }

//     void CheckForFood()
//     {
//         Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, foodDetectionRadius);
//         GameObject nearestFood = null;
//         float minDistance = float.MaxValue;

//         foreach (var hit in hitColliders)
//         {
//             if (hit.CompareTag("Food"))
//             {
//                 float distance = Vector3.Distance(transform.position, hit.transform.position);
//                 if (distance < minDistance)
//                 {
//                     minDistance = distance;
//                     nearestFood = hit.gameObject;
//                 }
//             }
//         }

//         if (nearestFood != null)
//         {
//             MoveToAndEatFood(nearestFood);
//         }
//     }

//     void MoveToAndEatFood(GameObject food)
//     {
//         transform.position = Vector3.MoveTowards(transform.position, food.transform.position, moveSpeed * Time.deltaTime);
//         if (Vector3.Distance(transform.position, food.transform.position) < 0.1f)
//         {
//             Destroy(food);
//             eatTimer = 0; //reset the cooldown timer after eating
//         }
//     }

//     void Die()
//     {
//         Destroy(gameObject);
//     }

//     void EnsureWithinBounds()
//     {
//         float clampedX = Mathf.Clamp(transform.position.x, minX, maxX); 
//         float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);
//         transform.position = new Vector3(clampedX, clampedY, transform.position.z);
//     }


//     void PrepareRandomMovement()
//     {
//         Vector2 randomDirection = Random.insideUnitCircle.normalized * 3.0f; //expand the movement range
//         targetPosition = transform.position + new Vector3(randomDirection.x, randomDirection.y, 0);
//         targetPosition = new Vector3(
//             targetPosition.x, 
//             Mathf.Clamp(targetPosition.y, minY, maxY), //clamp Y within bounds
//             targetPosition.z
//         );
//         isMoving = true;
//     }

//     void MoveToTarget()
//     {
//         transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);
//         if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
//         {
//             isMoving = false; //stop moving once close to the target
//         }
//     }
// }

