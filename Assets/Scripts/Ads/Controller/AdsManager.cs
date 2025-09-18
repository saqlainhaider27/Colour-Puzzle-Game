using UnityEngine;

public class AdsManager : Singleton<AdsManager> {
    public RewardedAds RewardedAds { get; internal set; }
    public bool Remove { get; private set; }
    private void Awake() {
        //DontDestroyOnLoad(this.gameObject);
        Gley.MobileAds.API.Initialize();
        //RewardedAds = RewardedAds.Instance;
        //MobileAds.RaiseAdEventsOnUnityMainThread = true;
        //MobileAds.Initialize((InitializationStatus initStatus) => {
        //    if (initStatus == null) {
        //        Debug.LogError("Google Mobile Ads initialization failed.");
        //        return;
        //    }

        //    Debug.Log("Google Mobile Ads initialization complete.");
        //    // Don't call LoadRewardedAd() here
        //    RewardedAds.LoadRewardedAd();
        //});
    }
    private void Start() {
        Gley.MobileAds.Events.onInitialized += Events_onInitialized;
    }

    private void Events_onInitialized() {
        Debug.Log("Ads Initialized");
    }
    public void RemoveAds() {
        Gley.MobileAds.API.RemoveAds(Remove);
        //SaveSystem.Save();
    }
}
