using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : Singleton<MenuController> {

    public event EventHandler OnSceneChanged;

    [SerializeField] private Image playerImage;
    [SerializeField] private List<Sprite> sprites = new List<Sprite>();
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject levelsMenu;
    [SerializeField] private GameObject sceneTransition;
    [SerializeField] private AudioSource musicSource; // Reference to the music audio source
    [SerializeField] private float transitionDuration = 1.0f; // Duration of fade-out in seconds

    private void Start() {
        HideSceneTransition();
        ShowMainMenu();
        HideLevelMenu();
    }

    public void Play() {
        HideMainMenu();
        ShowLevelMenu();
    }

    public void Quit() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void Options() {
    }

    public void Player() {
        playerImage.sprite = sprites[UnityEngine.Random.Range(0, sprites.Count)];
    }

    public void ShowSceneTransition() {
        sceneTransition.SetActive(true);
    }

    public void HideSceneTransition() {
        sceneTransition.SetActive(false);
    }

    public void ShowMainMenu() {
        mainMenu.SetActive(true);
    }

    public void HideMainMenu() {
        mainMenu.SetActive(false);
    }

    public void ShowLevelMenu() {
        levelsMenu.SetActive(true);
    }

    public void HideLevelMenu() {
        levelsMenu.SetActive(false);
    }

    public void LoadLevel(int _level) {
        StartCoroutine(LoadSceneWithTransition(_level));
    }

    private IEnumerator LoadSceneWithTransition(int index) {
        ShowSceneTransition();
        StartCoroutine(MusicFade()); // Start the music fade-out
        OnSceneChanged?.Invoke(this, EventArgs.Empty);
        yield return new WaitForSeconds(transitionDuration); // Wait for fade to complete before changing scenes
        SceneManager.LoadScene(index);
    }

    private IEnumerator MusicFade() {
        float startVolume = musicSource.volume;

        while (musicSource.volume > 0) {
            musicSource.volume -= startVolume * Time.deltaTime / transitionDuration;
            yield return null;
        }

        musicSource.volume = 0; // Ensure volume is zero at the end
    }

    public void QuitGame() {
        Application.Quit();
    }
}
