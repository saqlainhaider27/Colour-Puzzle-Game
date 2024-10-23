using System;
using System.Collections;
using UnityEngine;

public class ScoreController : Singleton<ScoreController> {

    private const string MAX_SCORE_ACHIEVED = "MaxScoreAchieved";
    private const string COMPLETED_LEVELS = "CompletedLevels";
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

        maxScoreAchieved = PlayerPrefs.GetInt(MAX_SCORE_ACHIEVED);

        completedLevels = PlayerPrefs.GetInt(COMPLETED_LEVELS);
    }

    private void GameManager_OnWinState(object sender, System.EventArgs e) {
        int updateScore;
        if (maxScoreAchieved < currentLevelScore) {
            maxScoreAchieved = currentLevelScore;
            updateScore = maxScoreAchieved;
            PlayerPrefs.SetInt(MAX_SCORE_ACHIEVED, maxScoreAchieved);
        }
        else {
            updateScore = currentLevelScore;
            // Debug.Log("CurrentLevelScore: " + currentLevelScore);

        }
        StartCoroutine(InvokeUpdateEventAfterDelay(updateScore));
        // Also set this level to completed
        // Just updating the completed levels int value will set all the previous levels to complete
        if (completedLevels < SceneController.Instance.GetCurrentSceneIndex()) {
            // Only set a value that is already greater then the already completed levels
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