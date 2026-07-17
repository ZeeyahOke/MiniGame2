using System;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(int amount);
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Round Settings")]
    [SerializeField] private float roundDuration = 60f;

    public event Action<int> OnScoreChanged;
    public event Action<int, int> OnHealthChanged;
    public event Action OnRoundEnded;

    private int score;
    private float timeRemaining;
    private bool isRoundActive;

    public int Score => score;
    public float TimeRemaining => timeRemaining;
    public bool IsRoundActive => isRoundActive;

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
        Time.timeScale = 1f;
        timeRemaining = roundDuration;
        isRoundActive = true;
        score = 0;
    }

    void Update()
    {
        if (!isRoundActive) return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            EndRound();
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        OnScoreChanged?.Invoke(score);
    }

    public void ReportHealthChanged(int current, int max)
    {
        OnHealthChanged?.Invoke(current, max);
    }

    public void OnPlayerDied()
    {
        if (!isRoundActive) return;
        EndRound();
    }

    private void EndRound()
    {
        isRoundActive = false;
        Time.timeScale = 0f;
        OnRoundEnded?.Invoke();
    }
}
