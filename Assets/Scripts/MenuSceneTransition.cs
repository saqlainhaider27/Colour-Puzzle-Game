using UnityEngine;

public class MenuSceneTransition : MonoBehaviour {

    private const string START = "Start";
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();

        MenuController.Instance.OnSceneChanged += MenuController_OnSceneChanged;
    }

    private void MenuController_OnSceneChanged(object sender, System.EventArgs e) {
        animator.SetTrigger(START);
    }
}

