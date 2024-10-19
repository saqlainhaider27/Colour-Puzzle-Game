using UnityEngine;

public class Paint : MonoBehaviour {

    [SerializeField] private Colour currentPaintColour;
    private ParticleSystem particle;



    private void Awake() {
        particle = GetComponentInChildren<ParticleSystem>();

        Player.Instance.OnPaintChanged += Player_OnPaintChanged;
    }

    private void Player_OnPaintChanged(object sender, Player.OnPaintChangedEventArgs e) {
        if (this != e.paint) {
            return;
        }
        particle.Play();

    }
    public Colour GetPaintColour() {
        return currentPaintColour;
    }
    public void DisablePaintAndDestroy() {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        Invoke("DestroySelf", 1f);
    }

    public void DestroySelf() {
        Destroy(gameObject);
    }
}
