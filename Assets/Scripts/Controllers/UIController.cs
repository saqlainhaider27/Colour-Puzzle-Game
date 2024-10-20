using System;
using System.Collections;
using UnityEngine;

public class UIController : Singleton<UIController> {

    [SerializeField] private Menu winMenu;
    [SerializeField] private Menu loseMenu;
    [SerializeField] private Menu pauseMenu;
    [SerializeField] private Menu settingsMenu;
    [SerializeField] private Menu gameMenu;

    [SerializeField] private GameObject blur;

    public event EventHandler OnLoadNextLevel;
    public event EventHandler OnReplayButtonPressed;
    public event EventHandler OnHomeButtonPressed;

    public event EventHandler OnMenuEnter;
    public event EventHandler OnMenuExit;


    private void Awake() {
        HideUIBlur();

        winMenu.HideMenu();
        loseMenu.HideMenu();
        //settingsMenu.HideMenu();
        //pauseMenu.HideMenu();

        GameManager.Instance.OnWinState += GameManager_OnWinState;
        GameManager.Instance.OnLoseState += GameManager_OnLoseState;    
    }


    private void GameManager_OnLoseState(object sender, EventArgs e) {
        loseMenu.ShowMenu();
    }

    private void GameManager_OnWinState(object sender, EventArgs e) {
        winMenu.ShowMenu();
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
    public void PauseGame() {
        ShowUIBlur();
        pauseMenu.ShowMenu();
        Time.timeScale = 0f;

    }
    public void Resume() {
        Time.timeScale = 1f;
        HideUIBlur();
        pauseMenu.HideMenu();
    }

    private IEnumerator InvokeAfterDelay(EventHandler eventHandler, float delay) {
        yield return new WaitForSeconds(delay);  // Wait for 'delay' seconds
        eventHandler?.Invoke(this, EventArgs.Empty);  // Invoke event after delay
    }


}

