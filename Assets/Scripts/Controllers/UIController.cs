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
    public event EventHandler OnPauseButtonPressed;
    public event EventHandler OnResumeButtonPressed;
    public event EventHandler OnSettingsButtonPressed;

    public event EventHandler OnMenuAppeared;

    public event EventHandler<OnMenuEnterEventArgs> OnMenuEnter;
    public class OnMenuEnterEventArgs : EventArgs {
        public Menu menu;
    }

    public event EventHandler<OnMenuExitEventArgs> OnMenuExit;
    public class OnMenuExitEventArgs : EventArgs {
        public Menu menu;
    }


    private void Awake() {
        HideUIBlur();
        winMenu.HideMenu();
        loseMenu.HideMenu();
        pauseMenu.HideMenu();
        settingsMenu.HideMenu();
        gameMenu.HideMenu();

        GameManager.Instance.OnGameStart += GameManager_OnGameStart;
        GameManager.Instance.OnWinState += GameManager_OnWinState;
        GameManager.Instance.OnLoseState += GameManager_OnLoseState;


        OnPauseButtonPressed += UIController_OnPauseButtonPressed;
        OnResumeButtonPressed += UIController_OnResumeButtonPressed;
        OnSettingsButtonPressed += UIController_OnSettingsButtonPressed;
    }

    private void UIController_OnSettingsButtonPressed(object sender, EventArgs e) {
        CheckActiveMenu().HideMenu();
    }

    private void UIController_OnResumeButtonPressed(object sender, EventArgs e) {
        pauseMenu.HideMenu();
        Time.timeScale = 1f;
    }

    private void UIController_OnPauseButtonPressed(object sender, EventArgs e) {
        Time.timeScale = 0f;
    }

    private void GameManager_OnGameStart(object sender, EventArgs e) {
        gameMenu.ShowMenu();
        OnMenuEnter?.Invoke(this, new OnMenuEnterEventArgs {
            menu = gameMenu
        });
    }

    private void GameManager_OnLoseState(object sender, EventArgs e) {
        ShowUIBlur();
        gameMenu.HideMenu();
        loseMenu.ShowMenu();
        OnMenuEnter?.Invoke(this, new OnMenuEnterEventArgs { 
            menu = loseMenu
        });
        StartCoroutine(InvokeAfterDelay(OnMenuAppeared, 0.5f));
    }

    private void GameManager_OnWinState(object sender, EventArgs e) {
        ShowUIBlur();
        gameMenu.HideMenu();
        winMenu.ShowMenu();
        OnMenuEnter?.Invoke(this, new OnMenuEnterEventArgs { 
            menu = winMenu
        });
    }

    
    private void ShowUIBlur() {
        blur.SetActive(true);
    }
    private void HideUIBlur() {
        blur.SetActive(false);
    }

   

    public void Next() {
        HideUIBlur();
        InvokeOnExitEvent(CheckActiveMenu());
        StartCoroutine(InvokeAfterDelay(OnLoadNextLevel, 0.5f));
    }

    public void Home() {
        if (GameManager.Instance.State == GameStates.Paused) {
            Resume();
        }
        HideUIBlur();
        InvokeOnExitEvent(CheckActiveMenu());

        StartCoroutine(InvokeAfterDelay(OnHomeButtonPressed, 0.5f));
    }

    public void Replay() {
        StartCoroutine(InvokeAfterDelay(OnReplayButtonPressed, 0.5f));
        InvokeOnExitEvent(CheckActiveMenu());
        HideUIBlur();
    }
    public void Pause() {
        GameManager.Instance.State = GameStates.Paused;

        ShowUIBlur();
        pauseMenu.ShowMenu();
        OnMenuEnter?.Invoke(this, new OnMenuEnterEventArgs {
            menu = pauseMenu
        });
        StartCoroutine(InvokeAfterDelay(OnPauseButtonPressed, 0.5f));

    }
    public void Resume() {
        StartCoroutine(InvokeAfterDelay(OnResumeButtonPressed, 0.5f));
        InvokeOnExitEvent(CheckActiveMenu());
        HideUIBlur();

    }
    public void Settings() {
        InvokeOnExitEvent(CheckActiveMenu());
        InvokeAfterDelay(OnSettingsButtonPressed, 0.5f);
        ShowUIBlur();
        GameManager.Instance.State = GameStates.Setting;

        settingsMenu.ShowMenu();
        OnMenuEnter?.Invoke(this, new OnMenuEnterEventArgs {
            menu = settingsMenu
        });
    }
    private Menu CheckActiveMenu() {
        
        switch (GameManager.Instance.State) {
            case GameStates.Win:
            return winMenu;
            case GameStates.Lose:
            return loseMenu;
            case GameStates.Paused:
            return pauseMenu;
            case GameStates.Setting:
            return settingsMenu;
            default:
            return gameMenu;
        }

    }

    private void InvokeOnExitEvent(Menu currentActiveMenu) {
        Debug.Log("Hiding " + currentActiveMenu.name);
        OnMenuExit?.Invoke(this, new OnMenuExitEventArgs {
            menu = currentActiveMenu
        });
    }
    public void SaveSetting() {
    
    }

    public void CancelSettings() {
    
    }
    private IEnumerator InvokeAfterDelay(EventHandler eventHandler, float delay) {
        yield return new WaitForSecondsRealtime(delay);  // Use WaitForSecondsRealtime to work with Time.timeScale = 0
        eventHandler?.Invoke(this, EventArgs.Empty);  // Invoke event after delay
    }



}

