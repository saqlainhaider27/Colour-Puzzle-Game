using GoogleMobileAds.Api;
using UnityEngine;

public class AdsManager : Singleton<AdsManager> {
    public RewardedAds RewardedAds { get; internal set; }

    private void Awake() {
        RewardedAds = RewardedAds.Instance;
        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        MobileAds.Initialize((InitializationStatus initStatus) => {
            if (initStatus == null) {
                Debug.LogError("Google Mobile Ads initialization failed.");
                return;
            }

            Debug.Log("Google Mobile Ads initialization complete.");
            // Don't call LoadRewardedAd() here
            RewardedAds.LoadRewardedAd();
        });
    }
}
