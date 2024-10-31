using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController> {

    public event EventHandler OnSceneChanged;

    private void Awake() {
        UIController.Instance.OnLoadNextLevel += UIController_OnLoadNextLevel;
        UIController.Instance.OnReplayButtonPressed += UIController_OnReplayButtonPressed;
        UIController.Instance.OnHomeButtonPressed += UIController_OnHomeButtonPressed;

    }


    private void UIController_OnHomeButtonPressed(object sender, System.EventArgs e) {
        LoadMainMenuScene();
    }

    private void LoadMainMenuScene() {
        int loadSceneIndex = 0;
        StartCoroutine(LoadSceneWithTransition(loadSceneIndex));
    }

    private void UIController_OnReplayButtonPressed(object sender, System.EventArgs e) {
        ResetCurrentScene();
    }

    private void ResetCurrentScene() {
        int loadSceneIndex = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(LoadSceneWithTransition(loadSceneIndex));
    }

    private void UIController_OnLoadNextLevel(object sender, System.EventArgs e) {
        LoadNextScene();
    }

    private void LoadNextScene() {
        int loadSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        StartCoroutine(LoadSceneWithTransition(loadSceneIndex));
    }

    private IEnumerator LoadSceneWithTransition(int index) {
        OnSceneChanged?.Invoke(this, new EventArgs());
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(index);
    }

    public int GetCurrentSceneIndex() {
        return SceneManager.GetActiveScene().buildIndex;
    }
}
