using System;
using UnityEngine;
using Unity.Services.LevelPlay;

public class RewardedAds : Singleton<RewardedAds>{

    [SerializeField] private string rewardedAdUnitId = "5u4alr0dsj2ai698";
    private LevelPlayRewardedAd rewardedAd;

    public event EventHandler OnRewardedAdComplete;
    public event EventHandler OnRewardedAdFailed;

    private void OnEnable() {
        rewardedAd.OnAdLoaded += RewardedOnAdLoadedEvent;
        rewardedAd.OnAdLoadFailed += RewardedOnAdLoadFailedEvent;
        rewardedAd.OnAdDisplayed += RewardedOnAdDisplayedEvent;
        rewardedAd.OnAdDisplayFailed += RewardedOnAdDisplayFailedEvent;
        rewardedAd.OnAdRewarded += RewardedOnAdRewardedEvent;
        rewardedAd.OnAdClosed += RewardedOnAdClosedEvent;
    }
    void RewardedOnAdLoadedEvent(LevelPlayAdInfo adInfo) { }
    void RewardedOnAdLoadFailedEvent(LevelPlayAdError error) { }
    void RewardedOnAdDisplayedEvent(LevelPlayAdInfo adInfo) { }
    void RewardedOnAdDisplayFailedEvent(LevelPlayAdDisplayInfoError infoError) {
        OnRewardedAdFailed?.Invoke(this, EventArgs.Empty);
    }
    void RewardedOnAdRewardedEvent(LevelPlayAdInfo adInfo, LevelPlayReward adReward) {
        OnRewardedAdComplete?.Invoke(this, EventArgs.Empty);
    }
    void RewardedOnAdClosedEvent(LevelPlayAdInfo adInfo) { }

    public void LoadRewardedAds() {
        rewardedAd = new LevelPlayRewardedAd(rewardedAdUnitId);
        rewardedAd.LoadAd();
    }
    public void ShowRewardedAds() {
        if (rewardedAd.IsAdReady()) {
            rewardedAd.ShowAd();
        } else {
            Debug.Log("unity-script: Levelplay Rewarded Ad Ready? - False");
        }
    }
}
