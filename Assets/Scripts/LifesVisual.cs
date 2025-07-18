using UnityEngine;

public class LifesVisual : MonoBehaviour {
    [SerializeField] private float lifeGiveCoolDown = 5f;
    private Life[] lifes;
    private int savedLifes;
    private void Awake() {
        // Get All lives in childern
        lifes = GetComponentsInChildren<Life>();
        savedLifes = LifeSaveManager.Instance.Lifes;
        UpdateLivesVisual();
    }
    private void UpdateLivesVisual() {
        for (int i = 0; i < lifes.Length; i++) {
            if (i < savedLifes) {
                lifes[i].GiveLife();
            } else {
                lifes[i].TakeLife();
            }
        }
    }
}