using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "GiftItem", menuName = "Scriptable Objects/GiftItem")]
public class GiftItemSO : ScriptableObject {
    public Image item;
    public TextMeshProUGUI amountText;
}
