using UnityEngine;

public class Paint : MonoBehaviour {

    [SerializeField] private Colour currentPaintColour;

    public Colour GetPaintColour() {
        return currentPaintColour;
    }
    public void DestroySelf() {
        Destroy(gameObject);
    }
}
