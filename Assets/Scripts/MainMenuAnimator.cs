using UnityEngine;

public class MainMenuAnimator : MonoBehaviour {
    private const string PLAY = "Play";
    private Animator animator;
    private void Awake() {
        animator = GetComponent<Animator>();
    }
    private void OnEnable() {
        MenuController.Instance.OnMenuExit += MenuController_OnMenuExit;
    }
    private void OnDisable() {
        animator.ResetTrigger(PLAY);
        MenuController.Instance.OnMenuExit -= MenuController_OnMenuExit;
    }
    private void MenuController_OnMenuExit() {
        animator.SetTrigger(PLAY);
    }

}
