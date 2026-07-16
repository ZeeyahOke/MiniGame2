using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamageable
{
    protected enum EnemyState
    {
        Spawning,
        Moving,
        Dying
    }

    [Header("Enemy Stats")]
    [SerializeField] protected int maxHealth = 1;
    [SerializeField] protected float moveSpeed = 2f;
    [SerializeField] protected int scoreValue = 1;
    [SerializeField] protected int contactDamage = 1;

    [Header("State Timing")]
    [SerializeField] protected float spawnDuration = 0.5f;
    [SerializeField] protected float dyingDuration = 0.2f;

    protected int currentHealth;
    protected Rigidbody2D rb;
    protected Transform playerTransform;
    protected SpriteRenderer spriteRenderer;
    protected EnemyState currentState;
    protected float stateTimer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
    }

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        EnterState(EnemyState.Spawning);
    }

    void FixedUpdate()
    {
        if (!GameManager.Instance.IsRoundActive) return;
        if (playerTransform == null) return;

        UpdateState();
    }

    protected void EnterState(EnemyState newState)
    {
        currentState = newState;
        stateTimer = 0f;

        switch (newState)
        {
            case EnemyState.Spawning:
                SetAlpha(0.5f);
                rb.linearVelocity = Vector2.zero;
                break;

            case EnemyState.Moving:
                SetAlpha(1f);
                break;

            case EnemyState.Dying:
                rb.linearVelocity = Vector2.zero;
                SetAlpha(0.3f);
                break;
        }
    }

    protected void UpdateState()
    {
        stateTimer += Time.fixedDeltaTime;

        switch (currentState)
        {
            case EnemyState.Spawning:
                if (stateTimer >= spawnDuration)
                {
                    EnterState(EnemyState.Moving);
                }
                break;

            case EnemyState.Moving:
                Move();
                break;

            case EnemyState.Dying:
                if (stateTimer >= dyingDuration)
                {
                    GameManager.Instance.AddScore(scoreValue);
                    Destroy(gameObject);
                }
                break;
        }
    }

    protected virtual void Move()
    {
        Vector2 direction = ((Vector2)playerTransform.position - (Vector2)transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
    }

    public virtual void TakeDamage(int amount)
    {
        if (currentState == EnemyState.Dying) return;

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            EnterState(EnemyState.Dying);
        }
    }

    private void SetAlpha(float alpha)
    {
    spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (currentState != EnemyState.Moving) return;

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
