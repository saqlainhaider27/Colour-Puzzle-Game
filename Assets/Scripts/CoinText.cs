using TMPro;
using UnityEngine;

public class CoinText : MonoBehaviour {
    private TextMeshProUGUI TMP;
    private void Awake() {
        TMP = GetComponent<TextMeshProUGUI>();
        TMP.text = PlayerPrefs.GetInt("Coins", 0).ToString();
    }
}
