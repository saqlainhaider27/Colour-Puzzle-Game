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

    private Menu previousMenu;
    private GameStates previousState;

    public event EventHandler OnLoadNextLevel;
    public event EventHandler OnReplayButtonPressed;
    public event EventHandler OnHomeButtonPressed;

    public event EventHandler OnMenuAppeared;
    public event EventHandler OnMenuDisappeared;

    public event EventHandler<OnMenuEnterEventArgs> OnMenuEnter;
    public class OnMenuEnterEventArgs : EventArgs {
        public Menu menu;
    }

    public event EventHandler<OnMenuExitEventArgs> OnMenuExit;
    public class OnMenuExitEventArgs : EventArgs {
        public Menu menu;
    }


    private void Awake() {

        GameManager.Instance.OnGameStart += GameManager_OnGameStart;
        GameManager.Instance.OnWinState += GameManager_OnWinState;
        GameManager.Instance.OnLoseState += GameManager_OnLoseState;


    }


    private void GameManager_OnGameStart(object sender, EventArgs e) {
        HideAllMenus();
        gameMenu.ShowMenu();
        GameManager.Instance.State = GameStates.Play;
    }

    private void HideAllMenus() {
        gameMenu.HideMenu();
        loseMenu.HideMenu();
        pauseMenu.HideMenu();
        settingsMenu.HideMenu();
        winMenu.HideMenu();
        HideUIBlur();

    }

    private void GameManager_OnLoseState(object sender, EventArgs e) {
        //HideAllMenus();
        gameMenu.HideMenu();
        ShowUIBlur();

        // OnMenuAppeared is an event to reset the revive timer of the lose menu
        loseMenu.ShowMenu();
        OnMenuEnter?.Invoke(this, new OnMenuEnterEventArgs { 
            menu = loseMenu
        });
        StartCoroutine(InvokeEventAfterDelay(OnMenuAppeared, 0.5f));
    }

    private void GameManager_OnWinState(object sender, EventArgs e) {
        //HideAllMenus();
        gameMenu.HideMenu();
        ShowUIBlur();
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
        ExitMenu();
        StartCoroutine(InvokeEventAfterDelay(OnLoadNextLevel, 0.5f));
    }

    private void ExitMenu(float delay = 0.5f) {
        if (CheckActiveMenu() == gameMenu) {
            return;
        }
        OnMenuDisappeared?.Invoke(this, EventArgs.Empty);
        HideUIBlur();
        InvokeOnExitEvent(CheckActiveMenu());
        // Disable object after delay
        StartCoroutine(DisableMenuAfterDelay(CheckActiveMenu(), delay));

    }

    public void Home() {
        if (GameManager.Instance.State == GameStates.Paused) {
            Resume();
        }
        ExitMenu();
        StartCoroutine(InvokeEventAfterDelay(OnHomeButtonPressed, 0.5f));

    }

    public void Replay() {
        if (GameManager.Instance.State == GameStates.Paused) {
            Resume();
        }
        ExitMenu();
        StartCoroutine(InvokeEventAfterDelay(OnReplayButtonPressed, 0.5f));
    }
    public void Pause() {
        gameMenu.HideMenu();
        ShowUIBlur();
        pauseMenu.ShowMenu();
        OnMenuEnter?.Invoke(this, new OnMenuEnterEventArgs {
            menu = pauseMenu
        });

        GameManager.Instance.State = GameStates.Paused;
        Time.timeScale = 0f;

    }
    public void Resume() {
        ExitMenu();
        Time.timeScale = 1f;
        GameManager.Instance.State = GameStates.Play;
        gameMenu.ShowMenu();
    }
    public void Settings() {
        previousMenu = CheckActiveMenu();
        previousState = GameManager.Instance.State;
        ExitMenu();

        GameManager.Instance.State = GameStates.Setting;

        //HideAllMenus();
        ShowUIBlur();
        StartCoroutine(ShowMenuAfterDelay(settingsMenu, 0.5f));

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
        OnMenuExit?.Invoke(this, new OnMenuExitEventArgs {
            menu = currentActiveMenu
        });
    }
    public void SaveSetting() {
        CancelSettings(); // Place holder for now
    }

    public void CancelSettings() {
        // If entered from pausemenu than back to win menu
        // If entered from winmenu than back to win menu
        // If entered from losemenu that back to lose menu
        ExitMenu();

        StartCoroutine(ShowMenuAfterDelay(previousMenu, 0.5f));
        StartCoroutine(ChangeStateAfterDelay(previousState, 0.5f));
        ShowUIBlur();

    }
    private IEnumerator InvokeEventAfterDelay(EventHandler eventHandler, float delay) {
        yield return new WaitForSecondsRealtime(delay);  // Use WaitForSecondsRealtime to work with Time.timeScale = 0
        eventHandler?.Invoke(this, EventArgs.Empty);  // Invoke event after delay
    }
    private IEnumerator DisableMenuAfterDelay(Menu menu,float delay) {
        yield return new WaitForSecondsRealtime(delay);  // Use WaitForSecondsRealtime to work with Time.timeScale = 0
        menu.HideMenu();
    }
    private IEnumerator ShowMenuAfterDelay(Menu menu, float delay) {
        yield return new WaitForSecondsRealtime(delay);  // Use WaitForSecondsRealtime to work with Time.timeScale = 0
        menu.ShowMenu();
        OnMenuEnter?.Invoke(this, new OnMenuEnterEventArgs {
            menu = menu
        });
    }
    private IEnumerator ChangeStateAfterDelay(GameStates state, float delay) {
        yield return new WaitForSecondsRealtime(delay);  // Use WaitForSecondsRealtime to work with Time.timeScale = 0
        GameManager.Instance.State = state;
    }


}

