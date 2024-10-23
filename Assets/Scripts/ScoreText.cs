using TMPro;
using UnityEngine;

public class ScoreText : MonoBehaviour {

    private TextMeshProUGUI text;
    private void Awake() {
        ScoreController.Instance.OnScoreChanged += ScoreController_OnScoreChanged;
        text = GetComponent<TextMeshProUGUI>();
    }

    private void ScoreController_OnScoreChanged(object sender, ScoreController.OnScoreChangedEventArgs e) {
        Debug.Log("Event Ran");
        text.text = e.score.ToString();
    }
}
