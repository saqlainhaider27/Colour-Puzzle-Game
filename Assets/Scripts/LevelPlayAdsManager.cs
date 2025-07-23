using UnityEngine;
using Unity.Services.LevelPlay;
using System;

public class LevelPlayAdsManager : MonoBehaviour {
    private LevelPlayInterstitialAd interstitialAd;
    private LevelPlayRewardedAd rewardedAd;

    private System.Action OnInterstitialAdComplete;
    private System.Action OnRewardedAdComplete;

    private string interstitialAdUnitId = "cvimmrzw477ymypx";
    private string rewardedAdUnitId = "5u4alr0dsj2ai698";

    private void Awake() {
        DontDestroyOnLoad(this);
        // Replace with your actual app key
#if UNITY_ANDROID
        string appKey = "230369e85";
#else 
        string appKey = "unexpected_platform";
#endif
        LevelPlay.ValidateIntegration();
        LevelPlay.Init(appKey);
    }
    private void OnEnable() {
        LevelPlay.OnInitSuccess += LevelPlay_OnInitSuccess;

        rewardedAd.OnAdLoaded += RewardedOnAdLoadedEvent;
        rewardedAd.OnAdLoadFailed += RewardedOnAdLoadFailedEvent;
        rewardedAd.OnAdDisplayed += RewardedOnAdDisplayedEvent;
        rewardedAd.OnAdDisplayFailed += RewardedOnAdDisplayFailedEvent;
        rewardedAd.OnAdRewarded += RewardedOnAdRewardedEvent;
        rewardedAd.OnAdClosed += RewardedOnAdClosedEvent;

        interstitialAd.OnAdLoaded += InterstitialOnAdLoadedEvent;
        interstitialAd.OnAdLoadFailed += InterstitialOnAdLoadFailedEvent;
        interstitialAd.OnAdDisplayed += InterstitialOnAdDisplayedEvent;
        interstitialAd.OnAdDisplayFailed += InterstitialOnAdDisplayFailedEvent;
        interstitialAd.OnAdClicked += InterstitialOnAdClickedEvent;
        interstitialAd.OnAdClosed += InterstitialOnAdClosedEvent;
        interstitialAd.OnAdInfoChanged += InterstitialOnAdInfoChangedEvent;

    }
    void RewardedOnAdLoadedEvent(LevelPlayAdInfo adInfo) { }
    void RewardedOnAdLoadFailedEvent(LevelPlayAdError error) { }
    void RewardedOnAdDisplayedEvent(LevelPlayAdInfo adInfo) { }
    void RewardedOnAdDisplayFailedEvent(LevelPlayAdDisplayInfoError infoError) { }
    void RewardedOnAdRewardedEvent(LevelPlayAdInfo adInfo, LevelPlayReward adReward) {
        OnRewardedAdComplete?.Invoke();
    }
    void RewardedOnAdClosedEvent(LevelPlayAdInfo adInfo) { }

    void InterstitialOnAdLoadedEvent(LevelPlayAdInfo adInfo) { }
    void InterstitialOnAdLoadFailedEvent(LevelPlayAdError error) { }
    void InterstitialOnAdDisplayedEvent(LevelPlayAdInfo adInfo) { }
    void InterstitialOnAdDisplayFailedEvent(LevelPlayAdDisplayInfoError infoError) { }
    void InterstitialOnAdClickedEvent(LevelPlayAdInfo adInfo) { }
    void InterstitialOnAdClosedEvent(LevelPlayAdInfo adInfo) { }
    void InterstitialOnAdInfoChangedEvent(LevelPlayAdInfo adInfo) { }


    private void LevelPlay_OnInitSuccess(LevelPlayConfiguration obj) {
        LoadInterstitial();
        LoadRewarded();
        Debug.Log("LevelPlay initialized successfully with configuration: " + obj);
    }
    public void LoadRewarded() {

    }
    public void ShowRewardedAd() {

    }

    public void LoadInterstitial() {
        interstitialAd = new LevelPlayInterstitialAd(interstitialAdUnitId);
        interstitialAd.LoadAd();
    }
    public void ShowInterstitial() {
        if (interstitialAd.IsAdReady()) {
            interstitialAd.ShowAd();
        } else {
            Debug.Log("unity-script: Levelplay Interstital Ad Ready? - False");
        }
    }

    private void OnApplicationPause(bool pause) {
        LevelPlay.SetPauseGame(pause);
    }
    private void OnDisable() {
        interstitialAd?.DestroyAd();
        rewardedAd?.DestroyAd();
    }
}
