using System;
using System.Collections;
using UnityEngine;

public class UIController : Singleton<UIController> {

    [SerializeField] private GameObject winMenu;
    [SerializeField] private GameObject loseMenu;

    [SerializeField] private GameObject blur;

    public event EventHandler OnLoadNextLevel;
    public event EventHandler OnReplayButtonPressed;
    public event EventHandler OnHomeButtonPressed;

    public event EventHandler OnMenuEnter;
    public event EventHandler OnMenuExit;

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

        OnMenuEnter?.Invoke(this, EventArgs.Empty);

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
        HideUIBlur();
        OnMenuExit?.Invoke(this, EventArgs.Empty);
        StartCoroutine(InvokeAfterDelay(OnLoadNextLevel, 0.5f));
    }

    public void BackToHome() {
        HideUIBlur();
        OnMenuExit?.Invoke(this, EventArgs.Empty);
        StartCoroutine(InvokeAfterDelay(OnHomeButtonPressed, 0.5f));
    }

    public void ReplayCurrentLevel() {
        HideUIBlur();
        OnMenuExit?.Invoke(this, EventArgs.Empty);
        StartCoroutine(InvokeAfterDelay(OnReplayButtonPressed, 0.5f));
    }

    private IEnumerator InvokeAfterDelay(EventHandler eventHandler, float delay) {
        yield return new WaitForSeconds(delay);  // Wait for 'delay' seconds
        eventHandler?.Invoke(this, EventArgs.Empty);  // Invoke event after delay
    }

    public void ShowLoseMenu() {
        ShowUIBlur();
        loseMenu.SetActive(true);

        OnMenuEnter?.Invoke(this, EventArgs.Empty);

    }
}

