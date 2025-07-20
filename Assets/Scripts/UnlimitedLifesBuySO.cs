using UnityEngine;
[CreateAssetMenu(fileName = "UnlimitedLifesBuySO", menuName = "Scriptable Objects/UnlimitedLifesBuySO")]
public class UnlimitedLifesBuySO : Buyable {
    public override void Buy() {
        GetUnlimitedLifes();
    }
    public void GetUnlimitedLifes() {
        if (LifeSaveManager.Instance.UnlimitedLifes) {
            return;
        }
        LifeSaveManager.Instance.GetUnlimitedLifes();
    }

}