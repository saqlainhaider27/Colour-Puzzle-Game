using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : Singleton<LevelController> {
    private const string COMPLETED_LEVELS = "CompletedLevels";
    private int completedLevels;
    public event EventHandler<OnLevelCompletedEventArgs> OnLevelCompleted;
    public class OnLevelCompletedEventArgs : EventArgs {
        
        public Levels completedLevel;
    }

    [SerializeField] private List<Levels> levels = new List<Levels>();

    private void Update() {
        if (!PlayerPrefs.HasKey(COMPLETED_LEVELS)) {
            PlayerPrefs.SetInt(COMPLETED_LEVELS, 0);
        }
        Debug.Log(PlayerPrefs.GetInt(COMPLETED_LEVELS).ToString());
        // Unlocks the levels that are completed and the next one along with it
        for (int i = 0; i < PlayerPrefs.GetInt(COMPLETED_LEVELS) + 1; i++) {
            OnLevelCompleted?.Invoke(this, new OnLevelCompletedEventArgs {
                completedLevel = levels[i]
            });
        }
    }

}
