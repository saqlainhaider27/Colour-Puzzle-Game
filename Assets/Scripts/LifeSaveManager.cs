using System;
using UnityEngine;

public class LifeSaveManager : Singleton<LifeSaveManager> {
    private int lifes;
    public int MaxLifes{
        get;
        private set;
    } = 5; // Default maximum number of lifes
    public float LifeIncrementInterval { get; private set; } = 300f; // 5 minutes = 300 seconds

    private const string LifesKey = "Lifes";
    private const string LastTimeKey = "LastLifeUpdateTime";
    private const string UnprocessedTimeKey = "UnprocessedLifeTime";
    public bool UnlimitedLifes = false;
    public event Action<int> OnLifeValueChanged;

    public int Lifes {
        get {
            if (PlayerPrefs.HasKey(LifesKey)) {
                lifes = PlayerPrefs.GetInt(LifesKey);
            } else {
                lifes = MaxLifes;
                PlayerPrefs.SetInt(LifesKey, lifes);
            }
            return lifes;
        }
        set {
            lifes = Mathf.Clamp(value, 0, MaxLifes);
            PlayerPrefs.SetInt(LifesKey, lifes);
            OnLifeValueChanged?.Invoke(lifes);
        }
    }

    private float unprocessedTime;

    private void Awake() {
        DontDestroyOnLoad(this);
        if ( PlayerPrefs.HasKey("UnlimitedLifes")) {
            int i = PlayerPrefs.GetInt("UnlimitedLifes");
            if (i == 0) {
                UnlimitedLifes = false;
            } else {
                UnlimitedLifes = true;
            }
        }
        RestoreLifeFromBackground();
    }

    private void OnApplicationQuit() {
        SaveTimeData();
    }

    private void Update() {
        if (Lifes == MaxLifes) return;

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
    private void OnApplicationPause(bool pause) {
        if (pause) {
            SaveTimeData();
        }
    }

    private void RestoreLifeFromBackground() {
        if (!PlayerPrefs.HasKey(LastTimeKey)) return;

        DateTime lastTime = DateTime.Parse(PlayerPrefs.GetString(LastTimeKey));
        //Debug.Log(lastTime.ToString());
        //Debug.Log(DateTime.Now.ToString());
        TimeSpan timePassed = DateTime.Now - lastTime;
        float savedUnprocessed = PlayerPrefs.GetFloat(UnprocessedTimeKey, 0f);
        float totalElapsedSeconds = (float)timePassed.TotalSeconds + savedUnprocessed;
        //Debug.Log(totalElapsedSeconds);
        int livesToAdd = (int)(totalElapsedSeconds / LifeIncrementInterval);
        totalElapsedSeconds -= livesToAdd * LifeIncrementInterval;
        unprocessedTime = totalElapsedSeconds;
        Lifes += livesToAdd;
        if (Lifes > MaxLifes ) {
            Lifes = MaxLifes;
            unprocessedTime = 0f;
        }

    }

    private void DecrementLife() {
        if (UnlimitedLifes) {
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
    public void GetUnlimitedLifes() {
        Lifes = MaxLifes;
        UnlimitedLifes = true;
        PlayerPrefs.SetInt("UnlimitedLifes", UnlimitedLifes ? 1 : 0);
    }
}
