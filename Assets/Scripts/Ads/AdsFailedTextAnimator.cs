using UnityEngine;
using Solo.MOST_IN_ONE;
public class AdsFailedTextAnimator : MonoBehaviour {
    private const string PLAY = "PLAY";
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();

        
    }
    private void OnEnable() {
        AdsManager.Instance.RewardedAds.OnRewardedAdFailed += RewardedAds_OnRewardedAdFailed;
    }
    private void OnDisable() {
        AdsManager.Instance.RewardedAds.OnRewardedAdFailed -= RewardedAds_OnRewardedAdFailed;
    }

    private void RewardedAds_OnRewardedAdFailed(object sender, System.EventArgs e) {
        GetComponent<AudioSource>().Play();
        HapticFeedbacks.Instance.WarningHaptic();
        animator.SetTrigger(PLAY);
    }
}

