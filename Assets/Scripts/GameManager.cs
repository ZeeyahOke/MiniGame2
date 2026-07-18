using System;
using System.Collections.Generic;
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

    [Header("Leaderboard")]
    [SerializeField] private int leaderboardSize = 5;

    public event Action<int> OnScoreChanged;
    public event Action<int, int> OnHealthChanged;
    public event Action OnRoundEnded;

    private int score;
    private float timeRemaining;
    private bool isRoundActive;
    private List<int> topScores = new List<int>();

    public int Score => score;
    public float TimeRemaining => timeRemaining;
    public bool IsRoundActive => isRoundActive;
    public IReadOnlyList<int> TopScores => topScores;

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
        LoadLeaderboard();
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
        InsertScoreIntoLeaderboard(score);
        SaveLeaderboard();
        OnRoundEnded?.Invoke();
    }

    private void InsertScoreIntoLeaderboard(int newScore)
    {
        topScores.Add(newScore);

        for (int i = topScores.Count - 1; i > 0; i--)
        {
            if (topScores[i] > topScores[i - 1])
            {
                int temp = topScores[i];
                topScores[i] = topScores[i - 1];
                topScores[i - 1] = temp;
            }
            else
            {
                break;
            }
        }

        if (topScores.Count > leaderboardSize)
        {
            topScores.RemoveAt(topScores.Count - 1);
        }
    }

    private void LoadLeaderboard()
    {
        topScores.Clear();
        for (int i = 0; i < leaderboardSize; i++)
        {
            int stored = PlayerPrefs.GetInt("TopScore_" + i, -1);
            if (stored >= 0) topScores.Add(stored);
        }
    }

    private void SaveLeaderboard()
    {
        for (int i = 0; i < topScores.Count; i++)
        {
            PlayerPrefs.SetInt("TopScore_" + i, topScores[i]);
        }
        PlayerPrefs.Save();
    }
}
