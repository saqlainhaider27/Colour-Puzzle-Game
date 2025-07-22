using System;
using UnityEngine;

public class GiftAnimator : MonoBehaviour {
    private Animator animator;


    private void Awake() {
        animator = GetComponent<Animator>();
    }
    private void OnEnable() {
        GiftController.Instance.OnGiftAvailable += GiftController_OnGiftAvailable;
        GiftController.Instance.OnGiftOpened += GiftController_OnGiftOpened;
        MenuController.Instance.OnGiftboxClicked += MenuController_OnGiftboxClicked;
    }

    private void GiftController_OnGiftOpened(GiftItemCreator.Item item, int arg2) {
        animator.SetBool("Open", true);
    }
    private void MenuController_OnGiftboxClicked() {
        animator.SetTrigger("Tap");
    }

    private void GiftController_OnGiftAvailable() {
        animator.SetTrigger("Available");
    }
}