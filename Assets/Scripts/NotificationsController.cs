using Assets.SimpleAndroidNotifications;
using Assets.SimpleAndroidNotifications.Helpers;
using System;
using UnityEngine;

public class NotificationsController : SingletonPersistent<NotificationsController> {

    private bool enableNotifications;
    public bool EnableNotifications {
        get {
            enableNotifications = 1 == PlayerPrefs.GetInt("EnableNotifications", 1);
            return enableNotifications;
        }
        set {
            enableNotifications = value;
            PlayerPrefs.SetInt("EnableNotifications", value ? 1 : 0);
        }
    }

    private int quitLifes;
    private float timeTogenerateAllLifes;

    public override void Awake() {
        base.Awake();
#if UNITY_ANDROID
        PermissionManager.RequestPostNotifications();
#endif
        EnableNotifications = 1 == PlayerPrefs.GetInt("EnableNotifications", 1);
    }
    private void Start() {
        NotificationManager.CancelAll();
    }
    private void OnApplicationQuit() {
        ScheduleLifeRefillNotification();
    }
    private void OnApplicationPause(bool pause) {
        if (!EnableNotifications) {
            return;
        }
#if UNITY_ANDROID
        if (pause) {
            ScheduleLifeRefillNotification();
        } else {
            NotificationManager.CancelAll();
        }
#endif
    }

    private void ScheduleLifeRefillNotification() {
        if (!EnableNotifications) return;

        // Get the lifes
        quitLifes = LifeSaveManager.Instance.Lifes;
        // calculate the lifes to generate
        int lifesToGenerate = LifeSaveManager.Instance.MaxLifes - quitLifes;
        if (lifesToGenerate <= 0) return;

        timeTogenerateAllLifes = LifeSaveManager.Instance.LifeIncrementInterval * lifesToGenerate;

#if UNITY_ANDROID
        int rnd = UnityEngine.Random.Range(1, 4);
        switch (rnd) {
            default:
            case 1:
                NotificationManager.SendWithAppIcon(
                    TimeSpan.FromSeconds(timeTogenerateAllLifes),
                    "Come Back! ",
                    "All your lifes have been refilled",
                    Color.red,
                    NotificationIcon.Heart);
                break;
            case 2:
                NotificationManager.SendWithAppIcon(
                    TimeSpan.FromSeconds(timeTogenerateAllLifes),
                    "Come Back! ",
                    "You have " + LifeSaveManager.Instance.MaxLifes + " lifes now",
                    Color.red,
                    NotificationIcon.Heart);
                break;
            case 3:
                NotificationManager.SendWithAppIcon(
                    TimeSpan.FromSeconds(timeTogenerateAllLifes),
                    "Come Back! ",
                    "You can play again!",
                    Color.red,
                    NotificationIcon.Heart);
                break;
        }
        ScheduleGiftReadyNotification();
#endif
    }
    private void ScheduleGiftReadyNotification() {
        if (!EnableNotifications) return;

#if UNITY_ANDROID
        // Assume GiftController.Instance.LastGiftCollected is a DateTime storing the last collection time
        // and GiftController.Instance.GiftCooldown is a TimeSpan (24 hours)
        DateTime lastCollected = GiftController.Instance.LastGiftCollected;
        TimeSpan cooldown = GiftController.Instance.GiftCooldown;
        DateTime nextGiftTime = lastCollected + cooldown;
        TimeSpan timeUntilGift = nextGiftTime - DateTime.Now;

        if (timeUntilGift.TotalSeconds <= 0)
            return; // Gift is already ready, no need to schedule

        NotificationManager.SendWithAppIcon(
            timeUntilGift,
            "Gift Ready!",
            "Your daily gift is waiting. Collect it now!",
            Color.cyan,
            NotificationIcon.Clock
        );
#endif
    }
}