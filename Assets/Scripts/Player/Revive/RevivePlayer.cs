using UnityEngine;

public class RevivePlayer : Singleton<RevivePlayer> {
    private void Awake() {
        
    }
    private void OnEnable() {
        AdsManager.Instance.RewardedAds.OnRewardedAdComplete += RewardedAds_OnRewardedAdComplete;
    }
    private void OnDisable() {
        AdsManager.Instance.RewardedAds.OnRewardedAdComplete -= RewardedAds_OnRewardedAdComplete;
    }
    private void RewardedAds_OnRewardedAdComplete(object sender, System.EventArgs e) {
        Player.Instance.Revive();
    }
}
