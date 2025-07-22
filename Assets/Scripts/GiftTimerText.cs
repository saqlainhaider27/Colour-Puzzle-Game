using System;
using TMPro;
using UnityEngine;

public class GiftTimerText : MonoBehaviour {
    private TextMeshProUGUI timerText;
    private void Awake() {
        timerText = GetComponent<TextMeshProUGUI>();
    }

    void Update() {
        TimeSpan time = GiftController.Instance.GetRemainingTime();
        timerText.text = FormatTime(time);
    }

    private string FormatTime(TimeSpan time) {
        int totalSeconds = Mathf.Max(0, (int)time.TotalSeconds);
        if ( totalSeconds == 0) {
            return "Gift Available";
        }
        int days = totalSeconds / 86400;
        int hours = (totalSeconds % 86400) / 3600;
        int minutes = (totalSeconds % 3600) / 60;
        int seconds = totalSeconds % 60;

        if (days > 0)
            return $"{days}d {hours:00}h {minutes:00}m";
        if (hours > 0)
            return $"{hours:00}h {minutes:00}m {seconds:00}s";
        return $"{minutes:00}m {seconds:00}s";
    }
}
