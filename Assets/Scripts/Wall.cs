using UnityEngine;

public class Wall : MonoBehaviour {
    [SerializeField] private Colour wallColour;
    public Colour GetWallColour() {
        return wallColour;
    }
    public void DestroySelf() {
        Destroy(gameObject);
    }
}
