using UnityEngine;

public class ItemsManager : Singleton<ItemsManager> {
    [SerializeField] private GameObject shopItemTemplete;
    [SerializeField] private ShopItemSO[] shopItemSOs;
    private void Awake() {
        foreach (var item in shopItemSOs) {
            GameObject itemGO = Instantiate(shopItemTemplete, transform);
            ShopItem shopItem = itemGO.GetComponent<ShopItem>();
            if (shopItem != null) {
                shopItem.ShopItemSO = item;
                shopItem.gameObject.SetActive(true);
            } else {
                Debug.LogError("ShopItem component not found on the template.", this);
            }
        }
    }

    public void RemoveAds() {
    }
}