using TMPro;
using UnityEngine;

public class ScoreText : MonoBehaviour {

    private TextMeshProUGUI text;

    private void Awake() {
        ScoreController.Instance.OnScoreChanged += ScoreController_OnScoreChanged;
    }

    private void ScoreController_OnScoreChanged(object sender, SceneController.OnScoreChangedEventArgs e) {
    }
}
