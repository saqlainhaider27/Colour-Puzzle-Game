using TMPro;
using UnityEngine;

public class LifeTimerDisplay : MonoBehaviour {
    private TextMeshProUGUI timerText;
    private void Awake() {
        timerText = GetComponent<TextMeshProUGUI>();
    }
    private void Update() {
        if (LifeSaveManager.Instance == null || timerText == null)
            return;

        int currentLifes = LifeSaveManager.Instance.Lifes;

        if (currentLifes >= 5) {
            timerText.text = "Full Lives";
            return;
        }

        float remaining = LifeSaveManager.Instance.LifeIncrementInterval - LifeSaveManager.Instance.GetCurrentTimer();
        if (remaining < 0f) remaining = 0f;

        int minutes = Mathf.FloorToInt(remaining / 60f);
        int seconds = Mathf.FloorToInt(remaining % 60f);
        timerText.text = $"Next Life In: {minutes:D2}:{seconds:D2}";
    }
}
