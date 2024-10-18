using System;
using UnityEngine;

public class UIController : Singleton<UIController> {

    [SerializeField] private GameObject winMenu;
    [SerializeField] private GameObject loseMenu;

    [SerializeField] private GameObject blur;

    public event EventHandler OnLoadNextLevel;
    public event EventHandler OnReplayButtonPressed;
    public event EventHandler OnHomeButtonPressed;

    private void Awake() {
        HideUIBlur();
        HideWinMenu();


        GameManager.Instance.OnWinState += GameManager_OnWinState;
        GameManager.Instance.OnLoseState += GameManager_OnLoseState;    
    }

    private void GameManager_OnLoseState(object sender, EventArgs e) {
        ShowLoseMenu();
    }

    private void GameManager_OnWinState(object sender, EventArgs e) {
        ShowWinMenu();
    }

    public void ShowWinMenu() {
        ShowUIBlur();
        winMenu.SetActive(true);
    }
    public void HideWinMenu() { 
        HideUIBlur();
        winMenu.SetActive(false);
    }
    private void ShowUIBlur() {
        blur.SetActive(true);
    }
    private void HideUIBlur() {
        blur.SetActive(false);
    }
    public void LoadNextLevel() {
        OnLoadNextLevel?.Invoke(this, EventArgs.Empty);
    }
    public void BackToHome() {
        OnHomeButtonPressed?.Invoke(this, EventArgs.Empty);
    }
    public void ReplayCurrentLevel() {
        OnReplayButtonPressed?.Invoke(this, EventArgs.Empty);
    }

    public void ShowLoseMenu() {

        ShowUIBlur();
        loseMenu.SetActive(true);
    }
}

