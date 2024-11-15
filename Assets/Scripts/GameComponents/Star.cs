using UnityEngine;

public class Star : NonCollideable {
    private ParticleSystem particle;
    private void Awake() {
        particle = GetComponentInChildren<ParticleSystem>();

        Player.Instance.OnStarCollected += Player_OnStarCollected;
    }
    private void Player_OnStarCollected(object sender, Player.OnStarCollectedEventArgs e) {
        if (this != e.collidedStar) {
            return;
        }
        particle.Play();
        DisableStarAndDestroy();
    }

    public void DisableStarAndDestroy() {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        Invoke(nameof(DestroySelf), 1f);
    }

    public void DestroySelf() {
        Destroy(gameObject);
    }
}
