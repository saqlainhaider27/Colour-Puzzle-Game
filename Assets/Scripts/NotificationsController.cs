using Assets.SimpleAndroidNotifications;
using Assets.SimpleAndroidNotifications.Helpers;
using System;
using UnityEngine;
using UnityEngine.Android;

public class NotificationsController : Singleton<NotificationsController> {

    private bool enableNotifications;
    public bool EnableNotifications {
        get { 
            return enableNotifications;
        }
        set {
            enableNotifications = value;
            PlayerPrefs.SetInt("EnableNotifications", value ? 1 : 0);
        }
    }

    private int quitLifes;
    private float timeTogenerateAllLifes;

    private void Awake() {
        Debug.Log("Hi");
        DontDestroyOnLoad(this);
#if UNITY_ANDROID
        PermissionManager.RequestPostNotifications();
#endif
        EnableNotifications = 1 == PlayerPrefs.GetInt("EnableNotifications", 1);
    }
    private void Start() {
        NotificationManager.CancelAll();
    }
    private void OnApplicationQuit() {
        // Get the lifes
        quitLifes = LifeSaveManager.Instance.Lifes;
        // calculate the lifes to generate
        int lifesToGenerate = LifeSaveManager.Instance.MaxLifes - quitLifes;
        timeTogenerateAllLifes = LifeSaveManager.Instance.LifeIncrementInterval * lifesToGenerate;
    }
    private void OnApplicationPause(bool pause) {
#if UNITY_ANDROID
        if (pause) {
            int rnd = UnityEngine.Random.Range(1, 3);
            switch (rnd) {
                default:
                case 1:
                    NotificationManager.SendWithAppIcon(TimeSpan.FromSeconds(timeTogenerateAllLifes), "Come Back! ", "All your lifes have been refilled", Color.red, NotificationIcon.Heart);
                    break;
                case 2:
                    NotificationManager.SendWithAppIcon(TimeSpan.FromSeconds(timeTogenerateAllLifes), "Come Back! ", "You have " + LifeSaveManager.Instance.MaxLifes + " lifes now", Color.red, NotificationIcon.Heart);
                    break;
                case 3:
                    NotificationManager.SendWithAppIcon(TimeSpan.FromSeconds(timeTogenerateAllLifes), "Come Back! ", "You can play again!", Color.red, NotificationIcon.Heart);
                    break;
            }
            // NotificationManager.SendWithAppIcon(TimeSpan.FromDays(1), "Daily reward is ready!", "Collect it now!", Color.cyan, NotificationIcon.Heart);

        }
#endif
    }
}