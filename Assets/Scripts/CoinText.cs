using TMPro;
using UnityEngine;

public class CoinText : MonoBehaviour {
    private TextMeshProUGUI TMP;
    private void Awake() {
        TMP = GetComponent<TextMeshProUGUI>();
        TMP.text = PlayerPrefs.GetInt("Coins", 0).ToString();
    }
    private void Start() {
        GameEconomics.Instance.OnCoinsValueChanged += GameEconomics_OnCoinsValueChanged;
    }

    private void GameEconomics_OnCoinsValueChanged(int obj) {
        TMP.text = obj.ToString();
    }
}
