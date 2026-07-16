using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Health")]
    [SerializeField] private int maxHealth = 5;

    [Header("Attack")]
    [SerializeField] private PlayerAttack attackPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float attackCooldown = 0.25f;

    private int currentHealth;
    private Rigidbody2D rb;
    private Vector2 movement;
    private float attackTimer;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (!GameManager.Instance.IsRoundActive) return;

        HandleMovementInput();
        HandleAttackInput();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement * moveSpeed;
    }

    private void HandleMovementInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        movement = new Vector2(horizontal, vertical).normalized;
    }

    private void HandleAttackInput()
    {
        attackTimer -= Time.deltaTime;

        if (Input.GetKey(KeyCode.Space) && attackTimer <= 0f)
        {
            Attack();
            attackTimer = attackCooldown;
        }
    }

    private void Attack()
    {
        PlayerAttack attack = Instantiate(attackPrefab, firePoint.position, Quaternion.identity);
        attack.Launch(Vector2.left);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    private void Die()
    {
        GameManager.Instance.OnPlayerDied();
        gameObject.SetActive(false);
    }
}
