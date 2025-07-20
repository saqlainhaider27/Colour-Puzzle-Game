using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : Singleton<LevelController> {
    private const string COMPLETED_LEVELS = "CompletedLevels";
    private int completedLevels;
    public event EventHandler<OnLevelCompletedEventArgs> OnLevelCompleted;
    public class OnLevelCompletedEventArgs : EventArgs {
        
        public Level completedLevel;
    }

    [SerializeField] private List<Level> levels = new List<Level>();

    private void Update() {
        if (!PlayerPrefs.HasKey(COMPLETED_LEVELS)) {
            PlayerPrefs.SetInt(COMPLETED_LEVELS, 0);
        }
        // Unlocks the levels that are completed and the next one along with it
        for (int i = 0; i <= PlayerPrefs.GetInt(COMPLETED_LEVELS); i++) {
            if (i == levels.Count) {
                break;
            }
            OnLevelCompleted?.Invoke(this, new OnLevelCompletedEventArgs {
                completedLevel = levels[i]
            });
        }

        // Enable stars gained for each level

        for (int i = 0; i < levels.Count; i++) {
            // This value has the max stars collected in that level
            int maxStars = PlayerPrefs.GetInt("Level" + (i + 1));
            // Now we have the saved value of the levels
            // We enable these amount of stars in the i level
            for (int j = 0; j < maxStars; j++) {
                List<Image> starImagesList = levels[i].GetLevelStarImageList();
                starImagesList[j].enabled = true;
            }
        }

    }

}
