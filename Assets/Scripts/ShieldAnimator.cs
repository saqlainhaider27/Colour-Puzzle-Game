using UnityEngine;

public class ShieldAnimator : MonoBehaviour {
    private const string PLAY = "Play";
    private Animator animator;
    private void Awake() {
        animator = GetComponent<Animator>();
    }
    private void OnEnable() {
        Player.Instance.OnShieldBreak += Player_OnShieldBreak;
    }
    private void OnDisable() {
        // animator.ResetTrigger(PLAY);
        Player.Instance.OnShieldBreak -= Player_OnShieldBreak;
    }

    private void Player_OnShieldBreak() {
        // animator.SetTrigger(PLAY);
        animator.Play("Shield_Break");
    }
}