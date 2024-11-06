using UnityEngine;

public class RevivePlayer : Singleton<RevivePlayer> {
    private void Awake() {
        AdsManager.Instance.RewardedAds.OnRewardedAdComplete += RewardedAds_OnRewardedAdComplete;
    }

    private void RewardedAds_OnRewardedAdComplete(object sender, System.EventArgs e) {
        Revive();
    }

    private void Revive() {
        Player.Instance.ShowSelf();
        // Also disable revive button
    }
}
