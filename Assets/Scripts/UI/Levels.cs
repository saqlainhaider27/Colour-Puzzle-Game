using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Levels : MonoBehaviour {

    [SerializeField] private int level;
    [SerializeField] private List<Image> levelStars = new List<Image>();
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private LockImage lockSprite;
    private Button button;
    private void Awake() {

        levelText.text = level.ToString();
        lockSprite = GetComponentInChildren<LockImage>();
        button = GetComponent<Button>();

        button.onClick.AddListener(() => {
            if (LifeSaveManager.Instance.Lifes == 0) {
                EventController.Invoke(EventController.OnZeroLifes);
                return;
            }
            LifeSaveManager.Instance.SubscribeToOnLevelChanged();
            EventController.Invoke(EventController.OnLevelChanged);
            MenuController.Instance.LoadLevel(level);
        });

        ShowLockSprite();
        DisableButton();

        // Disable all stars in star images list
        foreach (Image level in levelStars) {
            level.enabled = false;
        }

        LevelController.Instance.OnLevelCompleted += LevelController_OnLevelCompleted;
    }
    
    private void OnDisable() {
        button.onClick.RemoveAllListeners();
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

    public List<Image> GetLevelStarImageList() {
        return levelStars;
    }

}
