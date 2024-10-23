using UnityEngine;
using UnityEngine.UI;

public class Levels : MonoBehaviour {

    [SerializeField] private LockImage lockSprite;
    private Button button;
    private void Awake() {
        lockSprite = GetComponentInChildren<LockImage>();
        button = GetComponent<Button>();

        ShowLockSprite();
        DisableButton();

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


}
