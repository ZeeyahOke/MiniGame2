using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamageable
{
    [Header("Enemy Stats")]
    [SerializeField] protected int maxHealth = 1;
    [SerializeField] protected float moveSpeed = 2f;
    [SerializeField] protected int scoreValue = 1;
    [SerializeField] protected int contactDamage = 1;

    protected int currentHealth;
    protected Rigidbody2D rb;
    protected Transform playerTransform;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void FixedUpdate()
    {
        if (!GameManager.Instance.IsRoundActive) return;
        if (playerTransform == null) return;

        Move();
    }

    protected virtual void Move()
    {
        Vector2 direction = ((Vector2)playerTransform.position - (Vector2)transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
    }

    public virtual void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        GameManager.Instance.AddScore(scoreValue);
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IDamageable player = collision.gameObject.GetComponent<IDamageable>();
            if (player != null)
            {
                player.TakeDamage(contactDamage);
            }
        }
    }
}
