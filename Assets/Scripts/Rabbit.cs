using System.Collections;
using UnityEngine;

public class RabbitController : MonoBehaviour
{
    public float moveInterval = 20f;
    public float moveDuration = 2f;
    public float moveSpeed = 2f;
    private Vector2 targetPosition;
    private float timer = 0f;
    private bool isMoving = false;
    private bool isCombat = false;
    private bool isDragging = false;

    public float health = 5f;
    private Animator animator;

    private float minX = -9f;
    private float maxX = 8.5f;
    private float minY = -5f;
    private float maxY = 3f;
    private Vector3 dragOffset;
    
    private Zoom gameCam; //reference to the camera script that controls the camera movement

    private void Start()
    {
        animator = GetComponent<Animator>();
        ChooseNewTargetPosition();
        gameCam = Camera.main.GetComponent<Zoom>(); //assuming your main camera has the Zoom script
    }

    private void Update()
    {
        if (!isCombat && !isDragging) //only handle movement if not in combat or being dragged
        {
            HandleMovement();
        }
        CheckHealth();
    }

    private void HandleMovement()
    {
        timer += Time.deltaTime;
        if (timer >= moveInterval && !isMoving)
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
            float progress = (Time.time - startTime) / moveDuration;
            transform.position = Vector2.Lerp(startPosition, targetPosition, progress);
            yield return null;
        }

        transform.position = targetPosition;
        ChooseNewTargetPosition();
        isMoving = false;
    }

    private void ChooseNewTargetPosition()
    {
        float randomX = Random.Range(-2f, 2f);
        float randomY = Random.Range(-2f, 2f);
        targetPosition = new Vector2(
            Mathf.Clamp(transform.position.x + randomX, minX, maxX),
            Mathf.Clamp(transform.position.y + randomY, minY, maxY)
        );
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
        }
    }

    private void Die()
    {
        animator.SetTrigger("Die");
        this.enabled = false;
    }

    private void OnMouseDown()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z));
        dragOffset = transform.position - mousePos;
        isDragging = true;
        if (gameCam != null)
        {
            gameCam.enableDrag = false; //disable camera movement while dragging
        }
    }

    private void OnMouseDrag()
    {
        Vector3 newPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z)) + dragOffset;
        transform.position = new Vector3(Mathf.Clamp(newPos.x, minX, maxX), Mathf.Clamp(newPos.y, minY, maxY), newPos.z);
    }

    private void OnMouseUp()
    {
        isDragging = false;
        if (gameCam != null)
        {
            gameCam.enableDrag = true;
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
