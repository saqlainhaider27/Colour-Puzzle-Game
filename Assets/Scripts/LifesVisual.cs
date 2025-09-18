using UnityEngine;

public class LifesVisual : MonoBehaviour {
    private Life[] lifes;
    private int savedLifes;
    private void Start() {
        lifes = GetComponentsInChildren<Life>();
        savedLifes = LifeSaveManager.Instance.Lifes;
        UpdateLivesVisual();
        LifeSaveManager.Instance.OnLifeValueChanged += LifeSaveManager_OnLifeValueChanged;

    }
    private void OnDestroy() {
        LifeSaveManager.Instance.OnLifeValueChanged -= LifeSaveManager_OnLifeValueChanged;
    }

    private void LifeSaveManager_OnLifeValueChanged(int obj) {
        lifes = GetComponentsInChildren<Life>();
        savedLifes = obj;
        UpdateLivesVisual();
    }

    private void UpdateLivesVisual() {
        for (int i = 0; i < savedLifes; i++) {
            lifes[i].GiveLife();
        }
        for (int i = savedLifes; i < lifes.Length; i++) {
            lifes[i].TakeLife();
        }
    }
}