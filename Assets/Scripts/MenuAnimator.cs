using UnityEngine;

public class MenuAnimator : MonoBehaviour {
    private const string MENU_ENTRY = "MenuEntry";
    private const string MENU_EXIT = "MenuExit";


    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
        UIController.Instance.OnMenuEnter += UIController_OnMenuEnter;
        UIController.Instance.OnMenuExit += UIController_OnMenuExit;
    }

    private void UIController_OnMenuExit(object sender, System.EventArgs e) {
        animator.Play(MENU_EXIT);
    }

    private void UIController_OnMenuEnter(object sender, System.EventArgs e) {
        animator.Play(MENU_ENTRY);
    }
}