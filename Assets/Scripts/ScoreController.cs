using System;
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
        if (maxScoreAchieved < currentLevelScore) {
            maxScoreAchieved = currentLevelScore;
            OnScoreChanged?.Invoke(this, new OnScoreChangedEventArgs {
                score = maxScoreAchieved
            });
            PlayerPrefs.SetInt(MAX_SCORE_ACHIEVED, maxScoreAchieved);
        }
        else {
            OnScoreChanged?.Invoke(this, new OnScoreChangedEventArgs {
                score = currentLevelScore
            });
        }

        // Also set this level to completed
        // Just updating the completed levels int value will set all the previous levels to complete
        if (completedLevels > SceneController.Instance.GetCurrentSceneIndex()) {
            // Only set a value that is already greater then the already completed levels
            PlayerPrefs.SetInt(COMPLETED_LEVELS, SceneController.Instance.GetCurrentSceneIndex());
        }
    }


    private void Player_OnStarCollected(object sender, System.EventArgs e) {
        if (currentLevelScore < maxScore) {
            currentLevelScore++;
        }
    }
}