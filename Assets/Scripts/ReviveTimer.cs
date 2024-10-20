using TMPro;
using UnityEngine;

public class ReviveTimer : MonoBehaviour {
    private float maxReviveTime = 3f;
    private float currentTime;
    private bool isTimerRunning = false;

    private TextMeshProUGUI timerText;

    private void Awake() {
        UIController.Instance.OnMenuAppeared += UIController_OnMenuAppeared;

        // Ensure the TextMeshProUGUI reference is assigned
    }

    private void UIController_OnMenuAppeared(object sender, System.EventArgs e) {
        if (GameManager.Instance.State == GameStates.Lose) {
            // Start the timer
            currentTime = maxReviveTime; // Reset the timer
            isTimerRunning = true;
            UpdateTimerText(); // Immediately update the text when the timer starts
        }
    }

    private void Update() {
        if (isTimerRunning) {
            currentTime -= Time.deltaTime; // Decrement the timer by the time passed since the last frame

            if (currentTime <= 0f) {
                currentTime = 0f; // Clamp the timer to zero when it reaches below 0
                isTimerRunning = false; // Stop the timer
                Debug.Log("Revive timer completed.");
            }
            Debug.Log("Revive timer: " + currentTime.ToString("F2")); // Shows float value with 2 decimal places
            // Update the TMP text with the current timer value
            UpdateTimerText();
        }
    }

    private void UpdateTimerText() {
        if (timerText != null) {
            timerText.text = currentTime.ToString("F2") + "s"; // Format the time with 2 decimal places and 's' for seconds
        }
    }
}
