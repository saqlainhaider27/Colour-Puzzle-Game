using System;
using UnityEngine;

public class UIController : Singleton<UIController> {

    [SerializeField] private GameObject winMenu;
    [SerializeField] private GameObject blur;

    public event EventHandler OnLoadNextLevel;
    public event EventHandler OnReplayButtonPressed;
    public event EventHandler OnHomeButtonPressed;

    private void Awake() {
        HideUIBlur();
        HideWinMenu();    
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
        
    }
}

