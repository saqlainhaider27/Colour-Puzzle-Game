using UnityEngine;

public class Coin : MonoBehaviour, ICollectable {
    private ParticleSystem particle;
    [SerializeField] private string coinId; // Unique ID per coin instance

    private void Awake() {
        particle = GetComponentInChildren<ParticleSystem>();

        //// Check if this coin was already collected
        //if (PlayerPrefs.HasKey(GetCoinKey())) {
        //    if (PlayerPrefs.GetInt(GetCoinKey()) == 1) {
        //        Destroy(gameObject);
        //    }
        //}
    }

    public void Collect() {
        // Mark this coin as collected
        PlayerPrefs.SetInt(GetCoinKey(), 1);
        PlayerPrefs.Save(); // Ensure it's written to disk

        EventController.Invoke(EventController.OnCoinCollected, this.transform.position);
        GameEconomics.Instance.Coins += 1;

        this.particle.Play();
        DisablePaintAndDestroy();
    }

    private void DisablePaintAndDestroy() {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        Invoke(nameof(DestroySelf), 1f);
    }

    private void DestroySelf() {
        Destroy(gameObject);
    }

    // Helper method to get a unique key for PlayerPrefs
    private string GetCoinKey() {
        return $"CoinCollected_{coinId}";
    }
}
