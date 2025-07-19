using UnityEngine;

public class AdsFailedTextAnimator : MonoBehaviour {
    private const string PLAY = "PLAY";
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();

        AdsManager.Instance.RewardedAds.OnRewardedAdFailed += RewardedAds_OnRewardedAdFailed;
    }

    private void RewardedAds_OnRewardedAdFailed(object sender, System.EventArgs e) {
        animator.SetTrigger(PLAY);
    }
}

