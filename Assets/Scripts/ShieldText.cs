using TMPro;
using UnityEngine;

public class ShieldText : MonoBehaviour {
    private TextMeshProUGUI TMP;
    private void Awake() {
        TMP = GetComponent<TextMeshProUGUI>();
        TMP.text = PlayerPrefs.GetInt("Shields", 0).ToString();
    }
    private void Start() {
        ShieldController.Instance.OnShieldChanged += ShieldController_OnShieldChanged;
    }

    private void ShieldController_OnShieldChanged(int obj) {
        TMP.text = obj.ToString();
    }
}