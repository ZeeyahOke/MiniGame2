using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Timer")]
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Score")]
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Health")]
    [SerializeField] private TextMeshProUGUI healthText;

    [Header("Threat")]
    [SerializeField] private TextMeshProUGUI threatText;
    [SerializeField] private float warningDistance = 1f;

    private Transform playerTransform;

    void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnScoreChanged += HandleScoreChanged;
            GameManager.Instance.OnHealthChanged += HandleHealthChanged;
        }
    }

    void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnScoreChanged -= HandleScoreChanged;
            GameManager.Instance.OnHealthChanged -= HandleHealthChanged;
        }
    }

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerTransform = player.transform;

        HandleScoreChanged(GameManager.Instance.Score);
    }

    void Update()
    {
        UpdateTimer();
        UpdateThreat();
    }

    private void HandleScoreChanged(int newScore)
    {
        scoreText.text = "Score: " + newScore;
    }

    private void HandleHealthChanged(int current, int max)
    {
        healthText.text = "HP: " + current + "/" + max;
    }

    private void UpdateTimer()
    {
        if (GameManager.Instance == null) return;

        float timeLeft = GameManager.Instance.TimeRemaining;
        int totalSeconds = Mathf.CeilToInt(timeLeft);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    private void UpdateThreat()
    {
        if (EnemySpawner.Instance == null || playerTransform == null) return;

        float distance = EnemySpawner.Instance.GetDistanceToNearestEnemy(playerTransform.position);

        if (distance < 0f)
        {
            threatText.text = "Closest threat: --";
            threatText.color = Color.white;
            return;
        }

        threatText.text = "Closest threat: " + distance.ToString("F1") + "m";
        threatText.color = distance < warningDistance ? Color.red : Color.white;
    }
}
