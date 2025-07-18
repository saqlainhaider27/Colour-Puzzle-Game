using System;
using UnityEngine;

public class GameEconomics : Singleton<GameEconomics> {
    private int _coins = 0;
    public int Coins {
        get {
            return _coins;
        }
        set { 
            _coins = value;
            PlayerPrefs.SetInt("Coins", value);
        }
    }
    private void Awake() {
        if (PlayerPrefs.HasKey("Coins")) {
            _coins = PlayerPrefs.GetInt("Coins");
        }
    }

}