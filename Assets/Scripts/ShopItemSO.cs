using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ShopItemSO", menuName = "Scriptable Objects/ShopItemSO")]
public class ShopItemSO : ScriptableObject {
    public string itemName;
    public Sprite itemSprite;
    public string itemPrice;
    public Buyable buyable;
}
