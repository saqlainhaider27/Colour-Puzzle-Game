using UnityEngine;

public class RevivePlayer : Singleton<RevivePlayer> {
    public Colour PreviousColour {
        get; set;
    }
    public Vector3 Position {
        get; set;
    }

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
