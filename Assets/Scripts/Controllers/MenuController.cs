using Solo.MOST_IN_ONE;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : Singleton<MenuController> {

    public event EventHandler OnSceneChanged;

    [SerializeField] private Image playerImage;
    [SerializeField] private List<Sprite> sprites = new List<Sprite>();
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject levelsMenu;
    [SerializeField] private GameObject sceneTransition;
    [SerializeField] private GameObject lifesMenu;
    [SerializeField] private GameObject UIBlur;
    [SerializeField] private GameObject shop;
    [SerializeField] private GameObject settings;
    [SerializeField] private AudioSource buttonClick;
    [SerializeField] private float transitionDuration = 1.0f; // Duration of fade-out in seconds
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private Slider slider;
    [SerializeField] private Toggle vibrationsToggle;
    public event Action OnMenuExit;
    private float volume;
    [SerializeField] private AudioSource failedSource;
    [SerializeField] private AudioSource clickSource;

    private void Start() {
        HideShop();
        ShowMainMenu();
        HideLevelMenu();
        HideSettings();
        NotificationsController.Instance.EnableNotifications = true;
        musicSource.volume = 0f;
        volume = PlayerPrefs.GetFloat("Music", 0.1f);
        vibrationsToggle.isOn = HapticFeedbacks.Instance.EnableNotifications; // Set toggle based on HapticFeedbacks instance state
        StartCoroutine(MusicFadeIn());
        slider.value = volume * 10;
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }
    private void OnEnable() {
        vibrationsToggle.onValueChanged.AddListener(OnVibrationToggle);
    }
    private void OnDisable() {
        vibrationsToggle.onValueChanged.RemoveAllListeners();
    }

    private void OnVibrationToggle(bool arg0) {
        if (arg0) {
            HapticFeedbacks.Instance.EnableNotifications = true;
        } else {
            HapticFeedbacks.Instance.EnableNotifications = false;
        }
        PlayerPrefs.Save();
        clickSource.Play();
        HapticFeedbacks.Instance.GenerateBasicHaptic(Most_HapticFeedback.HapticTypes.Selection);
    }

    private void OnSliderValueChanged(float arg0) {
        musicSource.volume = arg0 / 10;
        PlayerPrefs.SetFloat("Music", arg0 / 10);
    }

    public void PlayButtonClick() {
        buttonClick.Play();
    }
    public void Play() {
        HideShop();
        HideMainMenu();
        ShowLevelMenu();
    }
    public void Back() {
        HideShop();
        HideLevelMenu();
        ShowMainMenu();

    }
    public void Shop() {
        HideMainMenu();
        HideLevelMenu();
        ShowShop();
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
    public void ShowSettings() {
        UIBlur.SetActive(true);
        settings.SetActive(true);
    }
    public void HideSettings() {
        OnMenuExit?.Invoke();
        StartCoroutine(HideSettingsAfterDelay());
    }

    private IEnumerator HideSettingsAfterDelay(float delay = 0.5f) {
        yield return new WaitForSeconds(delay);
        UIBlur.SetActive(false);
        settings.SetActive(false);
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
    public void ShowShop() {
        shop.SetActive(true);
    }
    public void HideShop() {
        shop.SetActive(false);
    }

    public void LoadLevel(int _level) {
        StartCoroutine(LoadSceneWithTransition(_level));
    }
    public void Lifes() {
        UIBlur.SetActive(true);
        lifesMenu.SetActive(true);
    }
    public void GetLife() {
        LifeSaveManager.Instance.Lifes += 1;
    }
    public void CloseLifesMenu() {
        OnMenuExit.Invoke();
        StartCoroutine(HideLifesMenuAfterDelay());
    }

    private IEnumerator HideLifesMenuAfterDelay(float delay = 0.5f) {
        yield return new WaitForSeconds(delay);
        UIBlur.SetActive(false);
        lifesMenu.SetActive(false);
    }
    private IEnumerator LoadSceneWithTransition(int index) {
        StartCoroutine(MusicFadeOut()); // Start the music fade-out
        OnSceneChanged?.Invoke(this, EventArgs.Empty);
        yield return new WaitForSeconds(transitionDuration); // Wait for fade to complete before changing scenes
        SceneManager.LoadScene(index);
    }
    public IEnumerator MusicFadeIn() {
        float targetVolume = volume; // Define the target volume
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

    public void PlayFailedSound() {
        failedSource.Play();
    }
}
