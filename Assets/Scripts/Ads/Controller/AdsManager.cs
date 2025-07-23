using UnityEngine;
using Unity.Services.LevelPlay;

public class AdsManager : Singleton<AdsManager>{
    public RewardedAds RewardedAds {
        get; private set;
    }
    [SerializeField] private string appKey = "230369e85";
    private string gameID;
    public bool Testing;
    private void Start() {
        RewardedAds = RewardedAds.Instance;
        DontDestroyOnLoad(this);
#if UNITY_ANDROID
        gameID = appKey;
#elif UNITY_EDITOR
            gameID = "unexpected_platform"; //Only for testing the functionality in the Editor
#endif
        LevelPlay.ValidateIntegration();
        LevelPlay.Init(gameID);

        LevelPlay.OnInitSuccess += LevelPlay_OnInitSuccess;
        LevelPlay.OnInitFailed += LevelPlay_OnInitFailed;
    }

    private void LevelPlay_OnInitFailed(LevelPlayInitError obj) {
        Debug.Log(obj);
    }

    private void LevelPlay_OnInitSuccess(LevelPlayConfiguration configuration) {
        Debug.Log("Success");
        RewardedAds.LoadRewardedAds();
    }
}