using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HwanAI : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform Enemy;

    [Header("Detection")]
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float attackRange = 1f;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;

    [Header("Attack")]
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float attackCooldown = 1.5f;

    private Rigidbody2D rb;
    private float nextAttackTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (Enemy == null)
            return;

        float distance = Vector2.Distance(
            rb.position,
            (Vector2)Enemy.position
        );

        if (distance > detectionRange ||
            distance <= attackRange)
        {
            return;
        }

        Vector2 direction =
            ((Vector2)Enemy.position - rb.position).normalized;

        Vector2 nextPosition =
            rb.position + direction * moveSpeed * Time.fixedDeltaTime;

        rb.MovePosition(nextPosition);
    }

    private void Update()
    {
        if (Enemy == null)
            return;

        float distance = Vector2.Distance(
            transform.position,
            Enemy.position
        );

        if (distance <= attackRange)
        {
            AttackEnemy();
        }
    }

    private void AttackEnemy()
    {
        if (Time.time < nextAttackTime)
            return;

        EnemyHealth health =
            Enemy.GetComponent<EnemyHealth>();

        if (health == null)
        {
            health = Enemy.GetComponentInParent<EnemyHealth>();
        }

        if (health == null)
            return;

        nextAttackTime = Time.time + attackCooldown;

        health.TakeDamage(attackDamage);

        Debug.Log("Support menyerang Enemy!");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}