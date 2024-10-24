using System;
using System.Collections;
using UnityEngine;

public class ScoreController : Singleton<ScoreController> {

    private const string MAX_SCORE_ACHIEVED = "MaxScoreAchieved";
    private const string COMPLETED_LEVELS = "CompletedLevels";
    private const string LEVEL = "Level";
    private readonly int maxScore = 3;
    private int maxScoreAchieved;
    private int currentLevelScore;

    private int completedLevels;

    public event EventHandler<OnScoreChangedEventArgs> OnScoreChanged;
    public class OnScoreChangedEventArgs : EventArgs {
        public int score;
    }

    private void Awake() {

        GameManager.Instance.OnWinState += GameManager_OnWinState;
        Player.Instance.OnStarCollected += Player_OnStarCollected;

        maxScoreAchieved = PlayerPrefs.GetInt(LEVEL + SceneController.Instance.GetCurrentSceneIndex());
        completedLevels = PlayerPrefs.GetInt(COMPLETED_LEVELS);
    }

    private void GameManager_OnWinState(object sender, System.EventArgs e) {
        int updateScore;
        // Always store the score, regardless of its value
        if (maxScoreAchieved < currentLevelScore) {
            maxScoreAchieved = currentLevelScore;
        }
        updateScore = currentLevelScore; // Store current score

        // PlayerPrefs.SetInt(MAX_SCORE_ACHIEVED, maxScoreAchieved);
        PlayerPrefs.SetInt(LEVEL + SceneController.Instance.GetCurrentSceneIndex(), maxScoreAchieved); // Store for current level

        StartCoroutine(InvokeUpdateEventAfterDelay(updateScore));

        // Update completed levels
        if (completedLevels < SceneController.Instance.GetCurrentSceneIndex()) {
            PlayerPrefs.SetInt(COMPLETED_LEVELS, SceneController.Instance.GetCurrentSceneIndex());
        }
    }


    private IEnumerator InvokeUpdateEventAfterDelay( int updateScore, float delay = 0.5f) {
        yield return new WaitForSecondsRealtime(delay);
        OnScoreChanged?.Invoke(this, new OnScoreChangedEventArgs {
            score = updateScore
        });
    }

    

    private void Player_OnStarCollected(object sender, System.EventArgs e) {
        if (currentLevelScore < maxScore) {
            currentLevelScore++;
        }
    }
}