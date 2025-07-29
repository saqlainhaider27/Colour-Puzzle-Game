using System;
using UnityEngine;

public class ShieldController : Singleton<ShieldController> {
    private int shields;
    private int minShields = 0;
    public event Action<int> OnShieldChanged;
    public int Shield {
        get {
            if (PlayerPrefs.HasKey(ShieldKey)) {
                shields = PlayerPrefs.GetInt(ShieldKey);
            } else {
                shields = minShields;
                PlayerPrefs.SetInt(ShieldKey, shields);
            }
            return shields;
        }
        set {
            shields = value;
            OnShieldChanged?.Invoke(shields);
            PlayerPrefs.SetInt(ShieldKey, shields);
        }
    }
    
    public string ShieldKey { get; private set; } = "Shields";

    private void Awake() {
        DontDestroyOnLoad(this);
    }

}