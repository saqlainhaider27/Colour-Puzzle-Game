using UnityEngine;

public class AudioController : Singleton<AudioController> {

    [SerializeField] private AudioRefsSO audioRefs;

    private void Awake() {
        Player.Instance.OnStarCollected += Player_OnStarCollected;
        Player.Instance.OnPaintChanged += Player_OnPaintChanged;
        Player.Instance.OnPlayerHitWall += Player_OnPlayerHitWall;
        Player.Instance.OnPlayerTeleport += Player_OnPlayerTeleport;
        Player.Instance.OnWinPointReached += Player_OnWinPointReached;
        Player.Instance.OnPlayerLose += Player_OnPlayerLose;
        AdsManager.Instance.RewardedAds.OnRewardedAdComplete += RewardedAds_OnRewardedAdComplete;
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

    private void Player_OnPlayerHitWall(object sender, Player.OnPlayerHitWallEventArgs e) {
        PlaySound(audioRefs.hitWall, e.position);
    }

    private void Player_OnPaintChanged(object sender, Player.OnPaintChangedEventArgs e) {
        // Paint collected run audio of paint collected
        PlaySound(audioRefs.colourCollect, e.paint.transform.position);
    }

    private void Player_OnStarCollected(object sender, Player.OnStarCollectedEventArgs e) {
        // Star collected run audio of star collected
        PlaySound(audioRefs.starCollect, e.collidedStar.transform.position);
    }
    public void PlayStarEntrySound() {
        PlaySound(audioRefs.starUIEntry, transform.position);
    }


    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f) {
        PlaySound(audioClipArray[UnityEngine.Random.Range(0, audioClipArray.Length)], position, volume);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f) {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }




}