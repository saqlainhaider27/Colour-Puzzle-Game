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
        Revive();
    }

    private void Revive() {
        Player.Instance.ShowSelf();
        // Also disable revive button
    }
}
