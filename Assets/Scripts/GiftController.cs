using System;
using UnityEngine;

public class GiftController : Singleton<GiftController> {
    private bool isAvailable;
    private const string GiftKey = "Gift";
    private const string GiftTimeKey = "GiftLastTime";
    private const int GiftCooldownHours = 24;
    public event Action OnGiftAvailable;
    public bool IsAvailable {
        get {
            // Check if enough time has passed since last gift, considering downtime
            long lastGiftTicks = long.Parse(PlayerPrefs.GetString(GiftTimeKey, "0"));
            System.DateTime lastGiftTime = new System.DateTime(lastGiftTicks);
            System.TimeSpan elapsed = System.DateTime.UtcNow - lastGiftTime;
            isAvailable = elapsed.TotalHours >= GiftCooldownHours;
            if (isAvailable) {
                OnGiftAvailable?.Invoke();
            }
            return isAvailable;
        }
        private set {
            isAvailable = value;
            PlayerPrefs.SetInt(GiftKey, isAvailable ? 1 : 0);
            if (!isAvailable) {
                // Set the last claimed time to now
                PlayerPrefs.SetString(GiftTimeKey, System.DateTime.UtcNow.Ticks.ToString());
            }
        }
    }

    public DateTime LastGiftCollected {
        get {
            long lastGiftTicks = long.Parse(PlayerPrefs.GetString(GiftTimeKey, "0"));
            return new DateTime(lastGiftTicks);
        }
        internal set {
            PlayerPrefs.SetString(GiftTimeKey, value.Ticks.ToString());
        }
    }
    public TimeSpan GiftCooldown {
        get {
            return TimeSpan.FromHours(GiftCooldownHours);
        }
        internal set {
            // Not used: cooldown is fixed, but setter provided for interface compatibility
        }
    }

    public event Action<GiftItemCreator.Item, int> OnGiftOpened;
    public void ClaimGift() {
        if (IsAvailable) {
            IsAvailable = false;
            bool canGiveLife = !PlayerHasMaxLifes();
            float rand = UnityEngine.Random.value;

            if (rand < 0.5f) {
                int coins = UnityEngine.Random.Range(1, 31);
                GameEconomics.Instance.Coins += coins;
                OnGiftOpened?.Invoke(GiftItemCreator.Item.coin, coins);
            } else if (rand < 0.9f) {
                int shields = UnityEngine.Random.Range(1, 4);
                ShieldController.Instance.Shield += shields;
                OnGiftOpened?.Invoke(GiftItemCreator.Item.shield, shields);
            } else if (canGiveLife) {
                LifeSaveManager.Instance.Lifes += 1;
                OnGiftOpened?.Invoke(GiftItemCreator.Item.heart, 1);
            } else {
                float reroll = UnityEngine.Random.value;
                if (reroll < 0.5f) {
                    int coins = UnityEngine.Random.Range(1, 31);
                    GameEconomics.Instance.Coins += coins;
                    OnGiftOpened?.Invoke(GiftItemCreator.Item.coin, coins);
                } else {
                    int shields = UnityEngine.Random.Range(1, 4);
                    ShieldController.Instance.Shield += shields;
                    OnGiftOpened?.Invoke(GiftItemCreator.Item.shield, shields);
                }
            }
        }
    }

    private bool PlayerHasMaxLifes() {
        return LifeSaveManager.Instance.Lifes >= LifeSaveManager.Instance.MaxLifes;
    }

    public void ResetGiftTimer() {
        PlayerPrefs.SetString(GiftTimeKey, "0");
        PlayerPrefs.SetInt(GiftKey, 1);
    }

    /// <summary>
    /// Returns the remaining time until the gift is available.
    /// </summary>
    /// <returns>TimeSpan representing the remaining time. If available, returns TimeSpan.Zero.</returns>
    public System.TimeSpan GetRemainingTime() {
        long lastGiftTicks = long.Parse(PlayerPrefs.GetString(GiftTimeKey, "0"));
        System.DateTime lastGiftTime = new System.DateTime(lastGiftTicks);
        System.TimeSpan elapsed = System.DateTime.UtcNow - lastGiftTime;
        System.TimeSpan cooldown = System.TimeSpan.FromHours(GiftCooldownHours);
        if (elapsed >= cooldown) {
            return System.TimeSpan.Zero;
        }
        return cooldown - elapsed;
    }

}