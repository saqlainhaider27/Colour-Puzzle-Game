using System;
using TMPro;
using UnityEngine;

public class ReviveTimer : MonoBehaviour {
    private readonly float maxReviveTime = 3f;
    private float currentTime;
    private bool isTimerRunning = false;

    public event EventHandler OnReviveTimerComplete;
    [SerializeField] private GameObject reviveButton;

    private TextMeshProUGUI timerText;

    private void Awake() {
        timerText = GetComponent<TextMeshProUGUI>();

        UIController.Instance.OnMenuAppeared += UIController_OnMenuAppeared;
    }

    private void UIController_OnMenuAppeared(object sender, System.EventArgs e) {
        // Debug.Log("MenuAppeared");
        if (GameManager.Instance.State == GameStates.Lose) {
            // Start the timer
            currentTime = maxReviveTime; // Reset the timer
            reviveButton.SetActive(true);
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
                
                // OnReviveTimerComplete?.Invoke(this, new EventArgs());

                reviveButton.SetActive(false);
            }
            // Debug.Log("Revive timer: " + currentTime.ToString("F2")); // Shows float value with 2 decimal places
            // Update the TMP text with the current timer value
            UpdateTimerText();
        }
    }

    private void UpdateTimerText() {
        timerText.text = currentTime.ToString("F1") + "s"; // Format the time with 2 decimal places and 's' for seconds
    }
}
