using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int healAmount = 1;
    [SerializeField] private float lifetime = 8f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            player.Heal(healAmount);
            Destroy(gameObject);
        }
    }
}
