using System;
using UnityEngine;

public class ScoreController : Singleton<SceneController> {

    private const string MAX_SCORE_ACHIEVED = "MaxScoreAchieved";
    private readonly int maxScore = 3;
    private int maxScoreAchieved;
    private int currentLevelScore;

    public event EventHandler<OnScoreChangedEventArgs> OnScoreChanged;
    public class OnScoreChangedEventArgs : EventArgs {
        public int score;
    }

    private void Awake() {

        GameManager.Instance.OnWinState += GameManager_OnWinState;
        Player.Instance.OnStarCollected += Player_OnStarCollected;

    }

    private void GameManager_OnWinState(object sender, System.EventArgs e) {
        if (maxScoreAchieved < currentLevelScore){
            maxScoreAchieved = currentLevelScore;
            OnScoreChanged?.Invoke(this, new OnScoreChangedEventArgs {
                score = maxScoreAchieved
            });
            PlayerPrefs.SetInt(MAX_SCORE_ACHIEVED, maxScoreAchieved);
        }
        
    }


    private void Player_OnStarCollected(object sender, System.EventArgs e) {
        if (currentLevelScore < maxScore) {
            currentLevelScore++;
        }
    }
}