using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Timer")]
    [SerializeField] private TextMeshProUGUI timerText;

    void Update()
    {
        if (GameManager.Instance == null) return;

        float timeLeft = GameManager.Instance.TimeRemaining;
        int totalSeconds = Mathf.CeilToInt(timeLeft);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }
}
