using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }

    [Header("Enemy Prefabs")]
    [SerializeField] private Enemy easyEnemyPrefab;
    [SerializeField] private Enemy mediumEnemyPrefab;
    [SerializeField] private Enemy hardEnemyPrefab;

    [Header("Enemy Spawn Settings")]
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float spawnX = -8.5f;
    [SerializeField] private float minY = -5f;
    [SerializeField] private float maxY = 5f;

    [Header("Spawn Weights")]
    [SerializeField] private int easyWeight = 5;
    [SerializeField] private int mediumWeight = 3;
    [SerializeField] private int hardWeight = 2;

    [Header("Pickup Settings")]
    [SerializeField] private HealthPickup healthPickupPrefab;
    [SerializeField] private float pickupSpawnInterval = 10f;
    [SerializeField] private float pickupMinX = -6f;
    [SerializeField] private float pickupMaxX = 6f;

    private float spawnTimer;
    private float pickupTimer;
    private List<Enemy> activeEnemies = new List<Enemy>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        pickupTimer = pickupSpawnInterval;
    }

    void Update()
    {
        if (!GameManager.Instance.IsRoundActive) return;

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            SpawnEnemy();
            spawnTimer = spawnInterval;
        }

        pickupTimer -= Time.deltaTime;
        if (pickupTimer <= 0f)
        {
            SpawnPickup();
            pickupTimer = pickupSpawnInterval;
        }
    }

    private void SpawnEnemy()
    {
        Enemy prefabToSpawn = PickWeightedEnemy();
        Vector2 spawnPosition = new Vector2(spawnX, Random.Range(minY, maxY));

        Enemy newEnemy = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
        activeEnemies.Add(newEnemy);
    }

    private void SpawnPickup()
    {
        if (healthPickupPrefab == null) return;

        Vector2 spawnPosition = new Vector2(
            Random.Range(pickupMinX, pickupMaxX),
            Random.Range(minY, maxY)
        );

        Instantiate(healthPickupPrefab, spawnPosition, Quaternion.identity);
    }

    private Enemy PickWeightedEnemy()
    {
        int totalWeight = easyWeight + mediumWeight + hardWeight;
        int roll = Random.Range(0, totalWeight);

        if (roll < easyWeight) return easyEnemyPrefab;
        if (roll < easyWeight + mediumWeight) return mediumEnemyPrefab;
        return hardEnemyPrefab;
    }

    public Enemy FindNearestEnemy(Vector2 fromPosition)
    {
        Enemy nearest = null;
        float shortestDistanceSquared = float.MaxValue;

        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            if (activeEnemies[i] == null)
            {
                activeEnemies.RemoveAt(i);
                continue;
            }

            float distanceSquared = ((Vector2)activeEnemies[i].transform.position - fromPosition).sqrMagnitude;

            if (distanceSquared < shortestDistanceSquared)
            {
                shortestDistanceSquared = distanceSquared;
                nearest = activeEnemies[i];
            }
        }

        return nearest;
    }

    public float GetDistanceToNearestEnemy(Vector2 fromPosition)
    {
        Enemy nearest = FindNearestEnemy(fromPosition);
        if (nearest == null) return -1f;

        return Vector2.Distance(fromPosition, nearest.transform.position);
    }
}
