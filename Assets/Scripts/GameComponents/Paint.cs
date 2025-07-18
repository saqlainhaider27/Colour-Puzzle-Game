using UnityEngine;

public class Paint : MonoBehaviour, ICollectable {

    [SerializeField] private Colour currentPaintColour;
    private ParticleSystem particle;



    private void Awake() {
        particle = GetComponentInChildren<ParticleSystem>();
    }
    public Colour GetPaintColour() {
        return currentPaintColour;
    }
    public void DisablePaintAndDestroy() {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        Invoke(nameof(DestroySelf), 1f);
    }

    public void DestroySelf() {
        Destroy(gameObject);
    }

    public void Collect() {
        bool switchColour = ColourSwitcher.Instance.IsColourDifferent(this);
        if (switchColour) {
            ColourSwitcher.Instance.SwitchColour(this);
            EventController.Invoke(EventController.OnPaintCollected, this.transform.position);
            this.particle.Play();
        }
        
    }
}
