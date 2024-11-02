using System;
using UnityEngine;
using UnityEngine.Advertisements;

public class RewardedAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener {

    [SerializeField] private string androidAdsUnitID;
    [SerializeField] private string iosAdsUnitID;

    private string adUnitID;

    public event EventHandler OnRewardedAdComplete;


    private void Awake() {
#if UNITY_IOS
    adUnitID = iosAdsUnitID;
#elif UNITY_ANDROID
    adUnitID = androidAdsUnitID;
#endif
    }

    public void LoadRewardedAds() {
        Advertisement.Load(adUnitID, this);
    }
    public void ShowRewardedAds() {
        Advertisement.Show(adUnitID, this);
        LoadRewardedAds();
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) {
    }

    public void OnUnityAdsShowStart(string placementId) {
    }

    public void OnUnityAdsShowClick(string placementId) {
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState) {
        if (placementId == adUnitID && showCompletionState == UnityAdsShowCompletionState.COMPLETED) {
            OnRewardedAdComplete?.Invoke(this, EventArgs.Empty);
        }
    }

    public void OnUnityAdsAdLoaded(string placementId) {
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message) {
    }
}
