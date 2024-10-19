using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject levelsMenu;


    private void Start() {
        ShowMainMenu();
        HideLevelMenu();
        InputManager.Instance.OnGameStart += InputManager_OnGameStart;
    }

    private void InputManager_OnGameStart(object sender, System.EventArgs e) {
        HideMainMenu();
        ShowLevelMenu();
    }

    public void HideMainMenu() {
        mainMenu.SetActive(false);
    }
    public void ShowMainMenu() {
        mainMenu.SetActive(true); 
    }
    public void ShowLevelMenu() {
        levelsMenu.SetActive(true);
    }
    public void HideLevelMenu() { 
        levelsMenu.SetActive(false);
    }
    public void LoadLevel(int _level) {
        SceneManager.LoadScene(_level);
    }
    public void QuitGame() {
        Application.Quit();
    }

}