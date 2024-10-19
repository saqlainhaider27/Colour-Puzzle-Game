using UnityEngine;

public class WinPoint : Singleton<WinPoint> {

    private ParticleSystem particle;

    private void Awake() {
        particle = GetComponentInChildren<ParticleSystem>();
        Player.Instance.OnWinPointReached += Player_OnWinPointReached;
    }

    private void Player_OnWinPointReached(object sender, System.EventArgs e) {
        particle.Play();
    }
}
