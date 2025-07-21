using Solo.MOST_IN_ONE;
using System;
using UnityEngine;

public class NotEnoughCoinsAnimator : MonoBehaviour {
    private const string PLAY = "PLAY";
    private Animator animator;
    private void Awake() {
        animator = GetComponent<Animator>();
    }
    private void Start() {
        EventController.OnInsufficientFunds += EventController_OnInsufficientFunds;
    }

    private void EventController_OnInsufficientFunds() {
        MenuController.Instance.PlayFailedSound();
        HapticFeedbacks.Instance.WarningHaptic();
        animator.SetTrigger(PLAY);
    }

    private void OnDestroy() {
        EventController.OnZeroLifes -= EventController_OnInsufficientFunds;
    }
    
}