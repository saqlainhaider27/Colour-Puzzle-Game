using System;
using UnityEngine;
using UnityEngine.UI;

public class GiftItemCreator : MonoBehaviour {
    [SerializeField] private GameObject itemTemplete;
    [SerializeField] private Sprite coin;
    [SerializeField] private Sprite shield;
    [SerializeField] private Sprite heart;

    public enum Item {
        coin,
        shield, 
        heart
    }
    private void Start() {
        GiftController.Instance.OnGiftOpened += GiftController_OnGiftOpened;
    }

    private void GiftController_OnGiftOpened(Item item, int arg2) {
        MenuController.Instance.PlayGiftRevealSound();
        StartCoroutine(SpawnGiftItemWithDelay(item, arg2));
    }

    private System.Collections.IEnumerator SpawnGiftItemWithDelay(Item item, int amount) {
        yield return new WaitForSeconds(0.5f);
        GameObject itemObject = Instantiate(itemTemplete, transform);
        GiftItemTemplete giftItem = itemObject.GetComponent<GiftItemTemplete>();
        giftItem.item.sprite = item switch {
            Item.coin => coin,
            Item.shield => shield,
            Item.heart => heart,
            _ => throw new ArgumentOutOfRangeException(nameof(item), item, null)
        };
        giftItem.amountText.text = "+ " + amount.ToString();
    }
}