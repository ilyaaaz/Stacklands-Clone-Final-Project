using System.Collections;
using UnityEngine;

public class WeaselController : MonoBehaviour
{
    public float moveSpeed = 8f;
    public float entrySpeed = 15f;
    public float foodDetectionRadius = 5f; //radius to detect food
    public float eatingCooldown = 10f; //time between eating actions
    private float eatTimer = 0f;
    private bool isMoving = false;
    public float health = 5f;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void InitializeMovement()
    {
        StartCoroutine(EnterScreen());
    }

    void Update()
    {
        if (health <= 0) Die();
        
        eatTimer += Time.deltaTime;
        if (eatTimer >= eatingCooldown)
        {
            CheckForFood();
        }
    }
    private IEnumerator EnterScreen()
    {
        // Move horizontally into the screen
        float targetX = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0, 0)).x;
        Vector3 targetPosition = new Vector3(targetX, transform.position.y, 0);
        float startTime = Time.time;
        float journeyLength = Mathf.Abs(targetPosition.x - transform.position.x);
        float fracJourney = 0;

        while (fracJourney < 1)
        {
            float distCovered = (Time.time - startTime) * entrySpeed;
            fracJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(transform.position, targetPosition, fracJourney);
            yield return null;
        }

        // After entering the screen, allow random movement
        InvokeRepeating("MoveRandomly", 2.0f, 1.0f);
    }

    void MoveRandomly()
    {
        if (isMoving) return;
        StartCoroutine(PerformRandomMovement());
    }

    IEnumerator PerformRandomMovement()
    {
        isMoving = true;
        Vector3 startPosition = transform.position;
        Vector3 moveDirection = new Vector2(Random.Range(-1, 2), Random.Range(-1, 2)).normalized;
        float startTime = Time.time;

        while (Time.time - startTime < 0.5f)
        {
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            yield return null;
        }

        isMoving = false;
    }

    void CheckForFood()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, foodDetectionRadius);
        foreach (var hit in hitColliders)
        {
            if (hit.CompareTag("Food"))
            {
                Destroy(hit.gameObject);
                eatTimer = 0; //reset eating cooldown timer
                break;
            }
        }
    }

    void Die()
    {
        animator.SetTrigger("Die");
        Destroy(gameObject, 1f); //allow time for animation
    }
}