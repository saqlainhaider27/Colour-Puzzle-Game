using System;
using UnityEngine;

public class AudioController : Singleton<AudioController> {

    [SerializeField] private AudioRefsSO audioRefs;
    private float vol;
    [SerializeField] private AudioSource clickSource;
    private void Awake() {
        EventController.OnStarCollected += EventController_OnStarCollected;
        EventController.OnPaintCollected += EventController_OnPaintCollected;
        EventController.OnCoinCollected += EventController_OnCoinCollected;
        EventController.OnWallCollision += EventController_OnWallCollision;


        Player.Instance.OnPlayerTeleport += Player_OnPlayerTeleport;
        Player.Instance.OnWinPointReached += Player_OnWinPointReached;
        Player.Instance.OnPlayerLose += Player_OnPlayerLose;
        Player.Instance.OnShieldActivated += Player_OnShieldActivated;
        Player.Instance.OnShieldBreak += Player_OnShieldBreak;

        
        vol = PlayerPrefs.GetFloat("SFX", 1);
        // Debug.Log("Volume: " + vol);
        UIController.Instance.SetSFXSliderValue(vol);
    }
    private void OnEnable() {
        AdsManager.Instance.RewardedAds.OnRewardedAdComplete += RewardedAds_OnRewardedAdComplete;
    }
    private void OnDisable() {
        AdsManager.Instance.RewardedAds.OnRewardedAdComplete -= RewardedAds_OnRewardedAdComplete;
    }
    public void PlayClick() {
        clickSource.Play();
    }
    private void Player_OnShieldBreak() {
        PlaySound(audioRefs.shieldBreak, Player.Instance.transform.position);
    }

    private void Player_OnShieldActivated() {
        PlaySound(audioRefs.shieldActive, Player.Instance.transform.position);
    }

    private void EventController_OnWallCollision(Vector2 vector) {
        PlaySound(audioRefs.hitWall, vector);
    }

    private void EventController_OnCoinCollected(Vector2 vector) {
        PlaySound(audioRefs.coinCollect, vector);
    }

    private void RewardedAds_OnRewardedAdComplete(object sender, System.EventArgs e) {
        PlaySound(audioRefs.revive, transform.position);
    }

    private void Player_OnPlayerLose(object sender, Player.OnPlayerLoseEventArgs e) {
        PlaySound(audioRefs.lose, e.position);
    }

    private void Player_OnWinPointReached(object sender, Player.OnWinPointReachedEventArgs e) {
        PlaySound(audioRefs.winPoint, e.position);
    }

    private void Player_OnPlayerTeleport(object sender, Player.OnPlayerTeleportEventArgs e) {
        PlaySound(audioRefs.teleport, e.position);
    }
    private void EventController_OnPaintCollected(Vector2 e) {
        PlaySound(audioRefs.colourCollect, e);
    }

    private void EventController_OnStarCollected(Vector2 e) {
        // Star collected run audio of star collected
        PlaySound(audioRefs.starCollect, e);
    }
    public void PlayStarEntrySound() {
        PlaySound(audioRefs.starUIEntry, transform.position);
    }
    public void SFXVolume(float volume) {
        vol = volume;
        PlayerPrefs.SetFloat("SFX", vol);
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f) {
        PlaySound(audioClipArray[UnityEngine.Random.Range(0, audioClipArray.Length)], position, volume);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f) {
        volume = PlayerPrefs.GetFloat("SFX", 1);
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }




}