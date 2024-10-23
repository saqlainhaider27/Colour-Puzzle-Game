using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : Singleton<LevelController> {

    private int completedLevels;
    public event EventHandler<OnLevelCompletedEventArgs> OnLevelCompleted;
    public class OnLevelCompletedEventArgs : EventArgs {
        
        public Levels completedLevel;
    }

    [SerializeField] private List<Levels> levels = new List<Levels>();

    private void Awake() {
        
        if (!PlayerPrefs.HasKey("CompletedLevels")) {
            PlayerPrefs.SetInt("CompletedLevels", 1);
        }
        Debug.Log(PlayerPrefs.GetInt("CompletedLevels").ToString());

        for (int i = 0; i < PlayerPrefs.GetInt("CompletedLevels"); i++) {
            OnLevelCompleted?.Invoke(this, new OnLevelCompletedEventArgs {
                completedLevel = levels[i]
            });
        }
    }

}
