using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour {

    [SerializeField] private int level;
    [SerializeField] private List<Image> levelStars = new List<Image>();
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private LockImage lockSprite;
    private Button button;
    private void Awake() {

        levelText.text = level.ToString();
        lockSprite = GetComponentInChildren<LockImage>();
        button = GetComponent<Button>();


        ShowLockSprite();
        DisableButton();

        // Disable all stars in star images list
        foreach (Image level in levelStars) {
            level.enabled = false;
        }

    }
    private void LifeSaveManager_OnLifeValueChanged(int obj) {
        //if (obj != 0) {
        //    button.onClick.AddListener(() => {
        //        if (obj == 0) {
        //            EventController.Invoke(EventController.OnZeroLifes);
        //            return;
        //        }
        //        LifeSaveManager.Instance.SubscribeToOnLevelChanged();
        //        EventController.Invoke(EventController.OnLevelChanged);
        //        MenuController.Instance.LoadLevel(level);
        //    });
        //} else {
        //    button.onClick.RemoveAllListeners();
        //    return;
        //}
    }
    private void OnEnable() {
        LifeSaveManager.Instance.OnLifeValueChanged += LifeSaveManager_OnLifeValueChanged;
        LevelController.Instance.OnLevelCompleted += LevelController_OnLevelCompleted;

        button.onClick.AddListener(() => {
            if (LifeSaveManager.Instance.Lifes == 0) {
                EventController.Invoke(EventController.OnZeroLifes);
                return;
            }
            //LifeSaveManager.Instance.SubscribeToOnLevelChanged();
            EventController.Invoke(EventController.OnLevelChanged);
            MenuController.Instance.LoadLevel(level);
        });
    }
    private void OnDisable() {
        button.onClick.RemoveAllListeners();
        LifeSaveManager.Instance.OnLifeValueChanged -= LifeSaveManager_OnLifeValueChanged;
        LevelController.Instance.OnLevelCompleted -= LevelController_OnLevelCompleted;
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
