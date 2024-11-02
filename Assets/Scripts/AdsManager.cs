using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : Singleton<AdsManager>, IUnityAdsInitializationListener {

    [field: SerializeField]
    public RewardedAds RewardedAds {
        get; private set;
    }
    [SerializeField] private string androidGameID;
    [SerializeField] private string iosGameID;
    private string gameID;
    [SerializeField] private bool testing;



    private void Awake() {
        //#if UNITY_IOS
        //        gameID = iosGameID;
        //#elif UNITY_ANDROID
        //        gameID = androidGameID;
        //#elif UNITY_EDITOR
        //        gameID = androidGameID;
        //#endif
        gameID = androidGameID;
        if (!Advertisement.isInitialized && Advertisement.isSupported) {
            Advertisement.Initialize(gameID, testing, this);
        }

        RewardedAds.LoadRewardedAds();
    }


    public void OnInitializationComplete() {
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message) {
    }
}