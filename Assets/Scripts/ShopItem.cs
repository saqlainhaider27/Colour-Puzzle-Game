using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour {
    private ShopItemSO shopItemSO;
    public ShopItemSO ShopItemSO {
        get {
            return shopItemSO;
        }
        set {
            shopItemSO = value;
            itemNameText.text = ShopItemSO.itemName;
            itemImage.sprite = ShopItemSO.itemSprite;
            itemPriceText.text = ShopItemSO.itemPrice;
        }
    }
    [SerializeField] private Button buyButton;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemPriceText;

    private void OnEnable() {
        buyButton.onClick.AddListener(OnButtonClick);
        buyButton.onClick.AddListener(() => MenuController.Instance.PlayButtonClick());
    }
    private void OnDisable() {
        buyButton.onClick.RemoveAllListeners();
    }
    private void OnButtonClick() {
        shopItemSO.buyable.Buy();
    }
}