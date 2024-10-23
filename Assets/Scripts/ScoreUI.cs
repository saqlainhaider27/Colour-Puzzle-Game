using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour {

    //private TextMeshProUGUI text;
    [SerializeField] private List<Image> stars = new List<Image>();
    private void Awake() {
        ScoreController.Instance.OnScoreChanged += ScoreController_OnScoreChanged;
        // text = GetComponent<TextMeshProUGUI>();
    }

    private void ScoreController_OnScoreChanged(object sender, ScoreController.OnScoreChangedEventArgs e) {
        //text.text = e.score.ToString();
        for (int i = 0; i < e.score; i++) {
            stars[i].enabled = true;
            stars[i].GetComponent<StarAnimator>().PlayEntryAnimation();
        }
    }
}
