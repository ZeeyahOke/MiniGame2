using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverPanel : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject panelRoot;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI leaderboardText;
    [SerializeField] private Button restartButton;

    void Start()
    {
        panelRoot.SetActive(false);
        GameManager.Instance.OnRoundEnded += HandleRoundEnded;
        restartButton.onClick.AddListener(RestartRound);
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnRoundEnded -= HandleRoundEnded;
        }
    }

    private void HandleRoundEnded()
    {
        panelRoot.SetActive(true);
        finalScoreText.text = "Final Score: " + GameManager.Instance.Score;
        BuildLeaderboardText();
    }

    private void BuildLeaderboardText()
    {
        var scores = GameManager.Instance.TopScores;
        string result = "TOP 5\n";

        for (int i = 0; i < scores.Count; i++)
        {
            result += (i + 1) + ". " + scores[i] + "\n";
        }

        leaderboardText.text = result;
    }

    private void RestartRound()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
