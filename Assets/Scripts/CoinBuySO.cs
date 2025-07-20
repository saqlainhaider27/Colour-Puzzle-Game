using UnityEngine;
[CreateAssetMenu(fileName = "CoinBuySO", menuName = "Scriptable Objects/CoinBuySO")]
public class CoinBuySO : Buyable {
    public int amount;
    public override void Buy() {
        AddCoins(amount);
    }
    public void AddCoins(int amount) {
        GameEconomics.Instance.Coins += amount;
    }
}
