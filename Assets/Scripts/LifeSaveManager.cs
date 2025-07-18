using System;
using UnityEngine;

public class LifeSaveManager : Singleton<LifeSaveManager> {
    private int lifes = 5;
    private float lifeTimer = 0f;
    private const int maxLifes = 5;
    private const float lifeIncrementInterval = 10f; // 5 minutes in seconds
    private const string LifesKey = "Lifes";
    private const string LastQuitTimeKey = "LastQuitTime";
    private const string LifeTimerKey = "LifeTimer";

    public int Lifes {
        get {
            if (PlayerPrefs.HasKey(LifesKey)) {
                lifes = PlayerPrefs.GetInt(LifesKey);
            } else {
                lifes = maxLifes;
                PlayerPrefs.SetInt(LifesKey, lifes);
            }
            return lifes;
        }
        set {
            lifes = value;
            PlayerPrefs.SetInt(LifesKey, lifes);
        }
    }

    private void Start() {
        RecoverLifesOnResume();
        EventController.OnNextLevelStarted += EventController_OnNextLevelStarted;
    }

    private void Update() {
        if (Lifes < maxLifes) {
            lifeTimer += Time.deltaTime;
            if (lifeTimer >= lifeIncrementInterval) {
                Lifes = Mathf.Min(maxLifes, Lifes + 1);
                lifeTimer = 0f;
            }
        } else {
            lifeTimer = 0f;
        }
    }

    private void OnApplicationPause(bool pause) {
        if (pause) {
            SaveLifeState();
        } else {
            RecoverLifesOnResume();
        }
    }

    private void OnApplicationQuit() {
        SaveLifeState();
    }

    private void SaveLifeState() {
        PlayerPrefs.SetString(LastQuitTimeKey, DateTime.UtcNow.ToString("o"));
        PlayerPrefs.SetFloat(LifeTimerKey, lifeTimer);
    }

    private void RecoverLifesOnResume() {
        if (PlayerPrefs.HasKey(LastQuitTimeKey)) {
            string lastQuitTimeStr = PlayerPrefs.GetString(LastQuitTimeKey, "");
            if (DateTime.TryParse(lastQuitTimeStr, null, System.Globalization.DateTimeStyles.RoundtripKind, out DateTime lastQuitTime)) {
                TimeSpan timeAway = DateTime.UtcNow - lastQuitTime;
                int lifesToRecover = (int)(timeAway.TotalSeconds / lifeIncrementInterval);

                if (Lifes < maxLifes && lifesToRecover > 0) {
                    int newLifes = Mathf.Min(maxLifes, Lifes + lifesToRecover);
                    Lifes = newLifes;
                }

                // Calculate leftover time for timer
                float leftoverSeconds = (float)(timeAway.TotalSeconds % lifeIncrementInterval);

                // If lifes are maxed, reset timer, else continue from leftover
                if (Lifes >= maxLifes) {
                    lifeTimer = 0f;
                } else {
                    lifeTimer = PlayerPrefs.GetFloat(LifeTimerKey, 0f) + leftoverSeconds;
                    if (lifeTimer >= lifeIncrementInterval) {
                        Lifes = Mathf.Min(maxLifes, Lifes + 1);
                        lifeTimer = 0f;
                    }
                }
            }
        } else {
            lifeTimer = 0f;
        }
    }

    private void EventController_OnNextLevelStarted() {
        Lifes = Mathf.Max(0, Lifes - 1);
        Debug.Log("Lives: " + Lifes);
    }
}