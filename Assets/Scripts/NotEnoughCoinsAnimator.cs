using Solo.MOST_IN_ONE;
using System;
using UnityEngine;

public class NotEnoughCoinsAnimator : MonoBehaviour {
    private const string PLAY = "PLAY";
    private Animator animator;
    private void Awake() {
        animator = GetComponent<Animator>();
    }
    private void OnEnable() {
        EventController.OnInsufficientFunds += EventController_OnInsufficientFunds;
    }
    private void OnDisable() {
        EventController.OnInsufficientFunds -= EventController_OnInsufficientFunds;
    }
    private void EventController_OnInsufficientFunds() {
        GetComponent<AudioSource>().Play();
        HapticFeedbacks.Instance.WarningHaptic();
        animator.SetTrigger(PLAY);
    }
    
}