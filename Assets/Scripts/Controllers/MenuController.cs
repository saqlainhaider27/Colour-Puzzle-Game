using Solo.MOST_IN_ONE;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] private GameObject lifesMenu;
    [SerializeField] private GameObject UIBlur;
    [SerializeField] private GameObject shop;
    [SerializeField] private GameObject settings;
    [SerializeField] private AudioSource buttonClick;
    [SerializeField] private float transitionDuration = 1.0f; // Duration of fade-out in seconds
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private Slider slider;
    [SerializeField] private Toggle vibrationsToggle;
    [SerializeField] private Toggle notificationsToggle;
    [SerializeField] private Button gift;
    [SerializeField] private GameObject giftMenu;
    public event Action OnMenuExit;
    private float volume;
    [SerializeField] private AudioSource failedSource;
    [SerializeField] private AudioSource clickSource;

    private void Start() {
        HideShop();
        ShowMainMenu();
        HideLevelMenu();
        HideSettings();
        musicSource.volume = 0f;
        volume = PlayerPrefs.GetFloat("Music", 0.1f);
        vibrationsToggle.isOn = HapticFeedbacks.Instance.EnableNotifications; // Set toggle based on HapticFeedbacks instance state
        notificationsToggle.isOn = NotificationsController.Instance.EnableNotifications;
        
        slider.value = volume * 10;
        slider.onValueChanged.AddListener(OnSliderValueChanged);
        FadeInMusic();

    }
    public void FadeInMusic() {
        StartCoroutine(MusicFadeIn());
    }
   
    public enum Reward {
        Gift,
        Life
    }
    private Reward reward;

    private void OnEnable() {
        //AdsManager.Instance.RewardedAds.OnRewardedAdComplete += RewardedAds_OnRewardedAdComplete;
        vibrationsToggle.onValueChanged.AddListener(OnVibrationToggle);
        notificationsToggle.onValueChanged.AddListener(OnNotificationsToggle);
        gift.onClick.AddListener(OnGiftClicked);
    }
    public event Action OnGiftboxClicked;
    private int giftTapCount = 0;
    private const int requiredGiftTaps = 1;

    private void OnGiftClicked() {
        if (GiftController.Instance.IsAvailable) {
            giftTapCount++;
            OnGiftboxClicked?.Invoke();
            clickSource.Play();
            HapticFeedbacks.Instance.GenerateBasicHaptic(Most_HapticFeedback.HapticTypes.Selection);

            if (giftTapCount >= requiredGiftTaps) {
                GiftController.Instance.ClaimGift();
                giftTapCount = 0;
            }
        } else {
            giftTapCount = 0;
        }
    }
    public void ShowGiftMenu() {
        UIBlur.SetActive(true);
        giftMenu.SetActive(true);
    }

    private void OnNotificationsToggle(bool arg0) {
        HapticFeedbacks.Instance.EnableNotifications = arg0;
        clickSource.Play();
        HapticFeedbacks.Instance.GenerateBasicHaptic(Most_HapticFeedback.HapticTypes.Selection);
    }

    private void OnDisable() {
        vibrationsToggle.onValueChanged.RemoveAllListeners();
        notificationsToggle.onValueChanged.RemoveAllListeners();
        gift.onClick.RemoveAllListeners();
        //AdsManager.Instance.RewardedAds.OnRewardedAdComplete -= RewardedAds_OnRewardedAdComplete;
    }

    private void OnVibrationToggle(bool arg0) {
        HapticFeedbacks.Instance.EnableNotifications = arg0;
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
        if(Gley.MobileAds.API.IsRewardedInterstitialAvailable()) {
            Gley.MobileAds.API.ShowRewardedVideo((bool success) => {
                if(success) {
                    LifeSaveManager.Instance.Lifes += 1;
                    return;
                } else {
                }
            });
        }

    }
    public void GetGift() {
        if(Gley.MobileAds.API.IsRewardedInterstitialAvailable()) {
            Gley.MobileAds.API.ShowRewardedVideo((bool success) => {
                if(success) {
                    GiftController.Instance.IsAvailable = true;
                    return;
                } else {
                }
            });
        }
    }
    public void CloseLifesMenu() {
        OnMenuExit.Invoke();
        StartCoroutine(HideLifesMenuAfterDelay());
    }
    public void CloseGiftMenu() {
        OnMenuExit?.Invoke();
        StartCoroutine(HideGiftMenuAfterDelay());
    }

    private IEnumerator HideGiftMenuAfterDelay(float delay = 0.5f) {
        yield return new WaitForSeconds(delay);
        UIBlur.SetActive(false);
        giftMenu.SetActive(false);
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
    [SerializeField] private AudioSource giftAudio;
    internal void PlayGiftRevealSound() {
        giftAudio.Play();
    }
}
