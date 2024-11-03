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
    [SerializeField] private float transitionDuration = 1.0f; // Duration of fade-out in seconds
    [SerializeField] private AudioSource musicSource;

    private void Start() {
        ShowMainMenu();
        HideLevelMenu();
        musicSource.volume = 0f;
        StartCoroutine(MusicFadeIn());
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
        StartCoroutine(MusicFadeOut()); // Start the music fade-out
        OnSceneChanged?.Invoke(this, EventArgs.Empty);
        yield return new WaitForSeconds(transitionDuration); // Wait for fade to complete before changing scenes
        SceneManager.LoadScene(index);
    }
    public IEnumerator MusicFadeIn() {
        float targetVolume = 0.1f; // Define the target volume
        float startVolume = 0f; // Start from zero
        musicSource.volume = startVolume; // Ensure starting volume is zero
        while (musicSource.volume < targetVolume) {
            musicSource.volume += targetVolume * Time.deltaTime / transitionDuration;
            yield return null;
        }

        musicSource.volume = targetVolume; // Ensure volume is set to the target at the end
    }

    public IEnumerator MusicFadeOut() {
        float startVolume = musicSource.volume;

        while (musicSource.volume > 0) {
            musicSource.volume -= startVolume * Time.deltaTime / transitionDuration;
            yield return null;
        }

        musicSource.volume = 0; // Ensure volume is zero at the end
    }
    public void InvokeMusicFadeIn() {
        musicSource.Play();
        StartCoroutine(MusicFadeIn());
    }
    public void QuitGame() {
        Application.Quit();
    }
}
