using System.Collections;
using UnityEngine;

public class RabbitController : MonoBehaviour
{
    public float moveInterval = 15f;
    public float moveDuration = 2f;
    public float moveSpeed = 2f;
    private Vector2 targetPosition;
    private float timer = 0f;
    private bool isMoving = false;
    private bool isCombat = false; 

    public float health = 5f;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        ChooseNewTargetPosition();
    }

    private void Update()
    {
        HandleMovement();
        CheckHealth();
    }

    private void HandleMovement()
    {
        timer += Time.deltaTime;
        if (timer >= moveInterval && !isMoving && !isCombat)
        {
            StartCoroutine(MoveToPosition());
            timer = 0f;
        }
    }

    private IEnumerator MoveToPosition()
    {
        isMoving = true;
        float startTime = Time.time;
        Vector2 startPosition = transform.position;

        while (Time.time < startTime + moveDuration)
        {
            transform.position = Vector2.Lerp(startPosition, targetPosition, (Time.time - startTime) / moveDuration);
            yield return null;
        }

        transform.position = targetPosition;
        ChooseNewTargetPosition();
        isMoving = false;
    }

    private void ChooseNewTargetPosition()
    {
        float randomX = Random.Range(-3f, 3f);
        float randomY = Random.Range(-3f, 3f);
        targetPosition = new Vector2(transform.position.x + randomX, transform.position.y + randomY);
    }

    private void CheckHealth()
    {
        if (health <= 0)
        {
            Die();
            isCombat = false; 
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health > 0)
        {
            animator.SetTrigger("Hit");
            RespondToAttack();
        }
    }

    private void RespondToAttack()
    {
        //attack back if attacked
        animator.SetTrigger("Attack");
    }

    private void Die()
    {
        animator.SetTrigger("Die");
        this.enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isCombat = true;
        if (collision.gameObject.CompareTag("Villager"))
        {
            TakeDamage(1f);  
        }
    }
}