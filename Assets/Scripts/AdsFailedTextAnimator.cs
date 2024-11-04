using UnityEngine;

public class AdsFailedTextAnimator : MonoBehaviour {
    private const string ADS_FAILED = "AdsFailed";
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();

        AdsManager.Instance.RewardedAds.OnRewardedAdFailed += RewardedAds_OnRewardedAdFailed;
    }

    private void RewardedAds_OnRewardedAdFailed(object sender, System.EventArgs e) {
        animator.SetTrigger(ADS_FAILED);
    }
}

