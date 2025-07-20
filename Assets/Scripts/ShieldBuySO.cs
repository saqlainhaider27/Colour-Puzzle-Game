using UnityEngine;
[CreateAssetMenu(fileName = "ShieldBuySO", menuName = "Scriptable Objects/ShieldBuySO")]
public class ShieldBuySO : Buyable {
    public int shieldAmount;
    public int cost;
    public override void Buy() {
        AddShield(shieldAmount);
    }
    public void AddShield(int amount) {
        if (GameEconomics.Instance.Coins >= cost) {
            GameEconomics.Instance.Coins -= cost;
            ShieldController.Instance.Shield += amount;
        } else {
            EventController.Invoke(EventController.OnInsufficientFunds);
        }
    }
}