using System;
using UnityEngine;

public class GameEconomics : Singleton<GameEconomics> {
    public event Action<int> OnCoinsValueChanged;
    private int _coins = 0;
    public int Coins {
        get {
            return _coins;
        }
        set {
            int clampedValue = Mathf.Clamp(value, 0, 9999);
            _coins = clampedValue;
            PlayerPrefs.SetInt("Coins", clampedValue);
            OnCoinsValueChanged?.Invoke(Coins);
        }
    }
    private void Awake() {
        if (PlayerPrefs.HasKey("Coins")) {
            _coins = PlayerPrefs.GetInt("Coins");
        }
    }

}