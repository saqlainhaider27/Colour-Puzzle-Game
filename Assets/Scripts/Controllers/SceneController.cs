using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController> {
    private void Awake() {
        UIController.Instance.OnLoadNextLevel += UIController_OnLoadNextLevel;
        UIController.Instance.OnReplayButtonPressed += UIController_OnReplayButtonPressed;
        UIController.Instance.OnHomeButtonPressed += UIController_OnHomeButtonPressed;

    }


    private void UIController_OnHomeButtonPressed(object sender, System.EventArgs e) {
        LoadMainMenuScene();
    }

    private static void LoadMainMenuScene() {
        SceneManager.LoadScene(0);
    }

    private void UIController_OnReplayButtonPressed(object sender, System.EventArgs e) {
        ResetCurrentScene();
    }

    private static void ResetCurrentScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void UIController_OnLoadNextLevel(object sender, System.EventArgs e) {
        LoadNextScene();
    }

    private static void LoadNextScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public int GetCurrentSceneIndex() {
        return SceneManager.GetActiveScene().buildIndex;
    }
}
