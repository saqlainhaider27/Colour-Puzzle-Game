using System;
using UnityEngine;

public class LifeSaveManager : Singleton<LifeSaveManager> {
    private int lifes;
    private const int maxLifes = 5;
    public float LifeIncrementInterval { get; private set; } = 300f; // 5 minutes = 300 seconds

    private const string LifesKey = "Lifes";
    private const string LastTimeKey = "LastLifeUpdateTime";
    private const string UnprocessedTimeKey = "UnprocessedLifeTime";
    private bool unlimitedLifes = false;
    public event Action<int> OnLifeValueChanged;

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
            lifes = Mathf.Clamp(value, 0, maxLifes);
            PlayerPrefs.SetInt(LifesKey, lifes);
            OnLifeValueChanged?.Invoke(lifes);
        }
    }

    private float unprocessedTime;

    private void Awake() {
        DontDestroyOnLoad(this);
        RestoreLifeFromBackground();
    }

    private void OnApplicationQuit() {
        SaveTimeData();
    }

    private void Update() {
        if (Lifes == maxLifes) return;

        unprocessedTime += Time.deltaTime;

        if (unprocessedTime >= LifeIncrementInterval) {
            int livesToAdd = Mathf.FloorToInt(unprocessedTime / LifeIncrementInterval);
            Lifes += livesToAdd;
            unprocessedTime -= livesToAdd * LifeIncrementInterval;
        }
    }

    private void SaveTimeData() {
        PlayerPrefs.SetFloat(UnprocessedTimeKey, unprocessedTime);
        PlayerPrefs.SetString(LastTimeKey, DateTime.Now.ToString("o")); // ISO 8601 format
    }

    private void RestoreLifeFromBackground() {
        if (!PlayerPrefs.HasKey(LastTimeKey)) return;

        DateTime lastTime = DateTime.Parse(PlayerPrefs.GetString(LastTimeKey));
        Debug.Log(lastTime.ToString());
        Debug.Log(DateTime.Now.ToString());
        TimeSpan timePassed = DateTime.Now - lastTime;
        float savedUnprocessed = PlayerPrefs.GetFloat(UnprocessedTimeKey, 0f);
        float totalElapsedSeconds = (float)timePassed.TotalSeconds + savedUnprocessed;
        Debug.Log(totalElapsedSeconds);
        int livesToAdd = (int)(totalElapsedSeconds / LifeIncrementInterval);
        totalElapsedSeconds -= livesToAdd * LifeIncrementInterval;
        unprocessedTime = totalElapsedSeconds;
        Lifes += livesToAdd;
        if (Lifes > maxLifes ) {
            Lifes = maxLifes;
            unprocessedTime = 0f;
        }

    }

    private void DecrementLife() {
        if (unlimitedLifes) {
            return;
        }
        Lifes = Mathf.Max(0, Lifes - 1);
        EventController.OnLevelChanged -= DecrementLife;
    }

    public void SubscribeToOnLevelChanged() {
        EventController.OnLevelChanged += DecrementLife;
    }

    public float GetCurrentTimer() {
        return unprocessedTime;
    }
}
