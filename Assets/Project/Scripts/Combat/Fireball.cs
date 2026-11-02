using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Fireball : MonoBehaviour
{
    [Header("Fireball Settings")]
    [SerializeField] private float speed = 8f;
    [SerializeField] private int damage = 15;
    [SerializeField] private float lifetime = 3f;

    private Rigidbody2D rb;
    private bool hasHit;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Vector2 direction)
    {
        rb.linearVelocity = direction.normalized * speed;

        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit)
            return;

        IDamageable target =
            other.GetComponent<IDamageable>();

        if (target == null)
        {
            target = other.GetComponentInParent<IDamageable>();
        }

        if (target != null)
        {
            hasHit = true;
            target.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}