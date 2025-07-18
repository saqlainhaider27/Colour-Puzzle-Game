using UnityEngine;

public class Star : MonoBehaviour, ICollectable {
    private ParticleSystem particle;
    private void Awake() {
        particle = GetComponentInChildren<ParticleSystem>();
    }

    public void DisableStarAndDestroy() {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        Invoke(nameof(DestroySelf), 1f);
    }

    public void DestroySelf() {
        Destroy(gameObject);
    }

    public void Collect() {
        this.particle.Play();
        DisableStarAndDestroy();
        EventController.Invoke(EventController.OnStarCollected, this.transform.position);
    }
}
