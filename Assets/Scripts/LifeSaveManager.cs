using System;
using UnityEngine;

public class LifeSaveManager : SingletonPersistent<LifeSaveManager> {
    private int lifes;
    public int MaxLifes { get; private set; } = 5;
    public float LifeIncrementInterval { get; private set; } = 300f; // 5 minutes

    private const string LifesKey = "Lifes";
    private const string LastTimeKey = "LastLifeUpdateTime";
    private const string UnprocessedTimeKey = "UnprocessedLifeTime";

    public bool UnlimitedLifes = false;
    public event Action<int> OnLifeValueChanged;

    private float unprocessedTime;

    public int Lifes {
        get => lifes;
        set {
            lifes = Mathf.Clamp(value, 0, MaxLifes);
            PlayerPrefs.SetInt(LifesKey, lifes);
            PlayerPrefs.Save(); // ✅ make sure it’s saved
            Debug.Log("Set: " + lifes);
            OnLifeValueChanged?.Invoke(lifes);
        }
    }

    public override void Awake() {
        base.Awake();
        lifes = PlayerPrefs.GetInt(LifesKey, MaxLifes);

        UnlimitedLifes = PlayerPrefs.GetInt("UnlimitedLifes", 0) == 1;
        RestoreLifeFromBackground();

        // ✅ Subscribe once here
        EventController.OnLevelChanged += DecrementLife;
    }


    private void OnApplicationQuit() => SaveTimeData();
    private void OnApplicationPause(bool pause) { if(pause) SaveTimeData(); }

    private void Update() {
        if(lifes >= MaxLifes) return; // ✅ use field, not property

        unprocessedTime += Time.deltaTime;

        if(unprocessedTime >= LifeIncrementInterval) {
            int livesToAdd = Mathf.FloorToInt(unprocessedTime / LifeIncrementInterval);
            Lifes += livesToAdd;
            unprocessedTime -= livesToAdd * LifeIncrementInterval;
        }
    }

    private void SaveTimeData() {
        PlayerPrefs.SetFloat(UnprocessedTimeKey, unprocessedTime);
        PlayerPrefs.SetString(LastTimeKey, DateTime.Now.ToString("o"));
        PlayerPrefs.Save(); // ✅ force save
    }

    private void RestoreLifeFromBackground() {
        if(!PlayerPrefs.HasKey(LastTimeKey)) return;

        DateTime lastTime = DateTime.Parse(PlayerPrefs.GetString(LastTimeKey));
        TimeSpan timePassed = DateTime.Now - lastTime;

        float savedUnprocessed = PlayerPrefs.GetFloat(UnprocessedTimeKey, 0f);
        float totalElapsedSeconds = (float)timePassed.TotalSeconds + savedUnprocessed;

        int livesToAdd = Mathf.FloorToInt(totalElapsedSeconds / LifeIncrementInterval);
        totalElapsedSeconds -= livesToAdd * LifeIncrementInterval;

        unprocessedTime = totalElapsedSeconds;
        Lifes += livesToAdd;

        if(lifes >= MaxLifes) {
            lifes = MaxLifes;
            unprocessedTime = 0f;
        }
    }

    private void DecrementLife() {
        if(UnlimitedLifes) return;
        Lifes = Mathf.Max(0, lifes - 1);
    }

    public void SubscribeToOnLevelChanged() {
        EventController.OnLevelChanged += DecrementLife;
    }

    private void OnDestroy() {
        EventController.OnLevelChanged -= DecrementLife; // ✅ cleanup here
    }

    public float GetCurrentTimer() => unprocessedTime;

    public void GetUnlimitedLifes() {
        Lifes = MaxLifes;
        UnlimitedLifes = true;
        PlayerPrefs.SetInt("UnlimitedLifes", 1);
        PlayerPrefs.Save();
    }
}
