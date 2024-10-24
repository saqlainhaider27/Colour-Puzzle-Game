using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Levels : MonoBehaviour {

    private int level;
    [SerializeField] private List<Image> levelStars = new List<Image>();

    [SerializeField] private LockImage lockSprite;
    private Button button;
    private void Awake() {
        lockSprite = GetComponentInChildren<LockImage>();
        button = GetComponent<Button>();

        ShowLockSprite();
        DisableButton();

        // Disable all stars in star images list
        foreach (Image level in levelStars) {
            level.enabled = false;
        }

        LevelController.Instance.OnLevelCompleted += LevelController_OnLevelCompleted;
    }

    private void LevelController_OnLevelCompleted(object sender, LevelController.OnLevelCompletedEventArgs e) {
        if (e.completedLevel == this) {
            HideLockSprite();
            EnableButton();
        }
    }

    public void EnableButton() {
        button.enabled = true;
    }
    public void DisableButton() {
        button.enabled = false;
    }
    public void ShowLockSprite() {
        lockSprite.enabled = true;
    }
    public void HideLockSprite() {
        lockSprite.enabled= false;
    }

    public int GetLevel() {
        return level;
    }
    public List<Image> GetLevelStarImageList() {
        return levelStars;
    }

}
