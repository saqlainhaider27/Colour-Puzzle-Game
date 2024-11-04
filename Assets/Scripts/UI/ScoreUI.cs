using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour {


    [SerializeField] private List<Image> stars = new List<Image>();
    private void Awake() {
        ScoreController.Instance.OnScoreChanged += ScoreController_OnScoreChanged;
    }

    private void ScoreController_OnScoreChanged(object sender, ScoreController.OnScoreChangedEventArgs e) {
        StopAllCoroutines(); // Stop any ongoing animation to prevent overlapping
        StartCoroutine(AnimateStars(e.score));
    }

    private IEnumerator AnimateStars(int score) {
        for (int i = 0; i < score; i++) {
            stars[i].enabled = true;
            stars[i].GetComponent<StarAnimator>().PlayEntryAnimation();
            AudioController.Instance.PlayStarEntrySound();
            yield return new WaitForSeconds(0.2f);
        }
    }
}
