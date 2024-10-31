using UnityEngine;

public class SceneTransition : MonoBehaviour {
    private const string START = "Start";
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();

        SceneController.Instance.OnSceneChanged += SceneController_OnSceneChanged;
    }

    private void SceneController_OnSceneChanged(object sender, System.EventArgs e) {
        animator.SetTrigger(START);
    }
}