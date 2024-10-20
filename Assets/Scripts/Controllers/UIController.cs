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
    }

    private void GameManager_OnGameStart(object sender, EventArgs e) {
        gameMenu.ShowMenu();
        OnMenuEnter?.Invoke(this, new OnMenuEnterEventArgs {
            menu = gameMenu
        });
    }

    private void GameManager_OnLoseState(object sender, EventArgs e) {
        ShowUIBlur();
        loseMenu.ShowMenu();
        OnMenuEnter?.Invoke(this, new OnMenuEnterEventArgs { 
            menu = loseMenu
        });

        gameMenu.HideMenu();
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
        StartCoroutine(InvokeAfterDelay(OnHomeButtonPressed, 0.5f));
    }

    public void Replay() {
        HideUIBlur();
        InvokeOnExitEvent(CheckActiveMenu());
        StartCoroutine(InvokeAfterDelay(OnReplayButtonPressed, 0.5f));
    }
    public void Pause() {
        GameManager.Instance.State = GameStates.Paused;

        ShowUIBlur();
        pauseMenu.ShowMenu();
        OnMenuEnter?.Invoke(this, new OnMenuEnterEventArgs {
            menu = pauseMenu
        });
        Time.timeScale = 0f;

    }
    public void Resume() {
        Time.timeScale = 1f;
        InvokeOnExitEvent(CheckActiveMenu());
        HideUIBlur();
        pauseMenu.HideMenu();
    }
    public void Settings() {
        if (GameManager.Instance.State == GameStates.Paused) {
            Resume();
            InvokeOnExitEvent(CheckActiveMenu());
            GameManager.Instance.State = GameStates.Setting;
            pauseMenu.HideMenu();
        }
        ShowUIBlur();
        settingsMenu.ShowMenu();
        OnMenuEnter?.Invoke(this, new OnMenuEnterEventArgs {
            menu = settingsMenu
        });
        Pause();
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
        yield return new WaitForSeconds(delay);  // Wait for 'delay' seconds
        eventHandler?.Invoke(this, EventArgs.Empty);  // Invoke event after delay
    }


}

