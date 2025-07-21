using Solo.MOST_IN_ONE;
using System;
using UnityEngine;

public class NoMoreLifesTextAnimator : MonoBehaviour {
    private const string PLAY = "PLAY";
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();   
    }
    private void Start() {
        EventController.OnZeroLifes += EventController_OnZeroLifes;
    }
    private void OnDestroy() {
        EventController.OnZeroLifes -= EventController_OnZeroLifes;
    }
    private void EventController_OnZeroLifes() {
        GetComponent<AudioSource>().Play();
        HapticFeedbacks.Instance.WarningHaptic();
        animator.SetTrigger(PLAY);
    }
}