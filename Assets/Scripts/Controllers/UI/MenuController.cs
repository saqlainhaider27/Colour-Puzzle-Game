using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

    [SerializeField] private Image playerImage;
    [SerializeField] private List<Sprite> sprites = new List<Sprite>();

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject levelsMenu;


    private void Start() {
        ShowMainMenu();
        HideLevelMenu();
    }
    public void Play() {
        HideMainMenu();
        ShowLevelMenu();
    }
    public void Quit() {
#if UNITY_EDITOR
        // Stop playing the scene in the editor.
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Quit the application.
        Application.Quit();
#endif
    }
    public void Options() {

    }
    public void Player() {
        playerImage.sprite = sprites[(UnityEngine.Random.Range(0, sprites.Count))];
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