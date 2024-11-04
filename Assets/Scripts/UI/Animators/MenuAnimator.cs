using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MenuAnimator : MonoBehaviour {
    private const string MENU_ENTRY = "MenuEntry";
    private const string MENU_EXIT = "MenuExit";


    private void Awake() {
        UIController.Instance.OnMenuEnter += UIController_OnMenuEnter;
        UIController.Instance.OnMenuExit += UIController_OnMenuExit;
    }

    private void UIController_OnMenuExit(object sender, UIController.OnMenuExitEventArgs e) {
        e.menu.GetComponent<Animator>().Play(MENU_EXIT);
    }


    private void UIController_OnMenuEnter(object sender, UIController.OnMenuEnterEventArgs e) {
        e.menu.GetComponent<Animator>().Play(MENU_ENTRY);
    }
}